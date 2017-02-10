using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.ClientLib;
using Aci.X.ClientLib.Exceptions;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.IwsLib;
using SF=Aci.X.IwsLib.Storefront;
using Aci.X.ServerLib;
using Aci.X.ServerLib.Mandrill;
using System.Xml;

namespace Aci.X.Business
{
  public class Order
  {
    public static ClientLib.Order Render(DBOrder dbOrder)
    {
      return new ClientLib.Order
      {
        OrderID = dbOrder.OrderID,
        OrderExternalID = dbOrder.OrderExternalID,
        OrderDate = dbOrder.OrderDate,
        Subtotal = dbOrder.Subtotal,
        Discount = dbOrder.Discount,
        Tax = dbOrder.Tax,
        OrderTotal = dbOrder.OrderTotal,
        VisitID = dbOrder.VisitID,
        UserID = dbOrder.UserID,
        Items = (from i in dbOrder.Items
                 select new OrderItem
                 {
                   OrderItemID = i.OrderItemID,
                   OrderItemExternalID = i.OrderItemExternalID,
                   OrderID = i.OrderID,
                   ProductID = i.ProductID,
                   ProductExternalID = i.ProductExternalID,
                   ProductToken = i.ProductToken,
                   OfferToken = i.OfferToken,
                   SkuID = i.SkuID,
                   ProductName = i.ProductName +
                    (i.FirstName == null 
                      ? ""
                      : " for " + i.FirstName +
                                    (String.IsNullOrEmpty(i.MiddleInitial.Trim()) ? "" : " " + i.MiddleInitial) +
                                    " " + i.LastName +
                                    (String.IsNullOrWhiteSpace(i.State) ? "" : " in " + GeneralPurposeCache.Singleton.StateByAbbr(i.State).StateName)),
                   
                   ProductType=i.ProductType,
                   ProductCode=i.ProductCode,
                   SkuCode=i.SkuCode,
                   Quantity = i.Quantity,
                   RegularPrice = i.RegularPrice,
                   Price = i.Price,
                   DiscountAmount = i.DiscountAmount,
                   Tax = i.Tax,
                   DiscountDescription = i.DiscountDescription,
                   RecurringPrice = i.RecurringPrice,
                   FirstName = i.FirstName,
                   MiddleInitial = i.MiddleInitial,
                   LastName = i.LastName,
                   State = i.State,
                   ProfileID = i.ProfileID,
                   QueryID = i.QueryID??0,
                   ReportTypeCode = i.ReportTypeCode,
                 }).ToArray()
      };
    }

    public static ClientLib.Order Checkout(CallContext context)
    {
      if (context.DBVisit.Cart == null || context.DBVisit.Cart.Items.Length == 0)
      {
        throw new CartEmptyException();
      }
      string strProfileID = null;
      int intQueryID = context.DBVisit.CurrentQueryID;
      foreach (var item in context.DBVisit.Cart.Items)
      {
        if (strProfileID == null && item.ProfileID != null)
        {
          strProfileID = item.ProfileID;
        }
        if (intQueryID == 0 && item.QueryID != 0)
        {
          intQueryID = item.QueryID;
        }
      }

      var storefrontCli = new SF.StorefrontClient(context);
      using (var db = new AciXDB())
      {
        var catalog = db.spProductGet(
          intSiteID: context.SiteID,
          intUserID: context.AuthorizedUserID,
          intProductIDs: null);

        /*
         * If the order has only "free" items, i.e. due to a valid subscription,
         * then we won't create an IWS order.  We'll use the order for the valid 
         * subscription period.
         */
        int intOrderID = 0;
        ClientLib.Order order = null;
        bool boolFreeOrder = true;
        int intSubscriptionOrderID = 0;
        int intSubscriptionItemSkuID = 0;

        foreach (var item in context.DBVisit.Cart.Items)
        {
          if (item.SubscriptionOrderID != 0)
          {
            intSubscriptionOrderID = item.SubscriptionOrderID;
            intSubscriptionItemSkuID = item.SkuID;
          }

          if (item.SubscriptionOrderID == 0 || item.SubscriptionQuantityRemaining <= 0)
          {
            boolFreeOrder = false;
          }
        }
        if (boolFreeOrder)
        {
          db.spUserSubscriptionItemDebit(
            intSiteID: context.SiteID,
            intUserID: context.AuthorizedUserID,
            intItemSkuID: intSubscriptionItemSkuID);

          order = Get(context, new int[] { intSubscriptionOrderID }).First();
        }
        else
        {
          try
          {
            var sfRet = storefrontCli.Checkout();
            string strOrderXml = Solishine.CommonLib.XmlHelper.Serialize(sfRet, System.Text.UnicodeEncoding.Unicode);
            intOrderID = CreateFromIwsOrder(
              context: context,
              strOrderXml: strOrderXml,
              intQueryID: intQueryID,
              strProfileID: strProfileID,
              boolIsStorefrontXml: true,
              boolIsMultipleOrders: false);

            order = Get(context, new int[] { intOrderID })[0];
            MandrillClient.NewMessage(context, "PurchaseReceipt", strVarName: "order", oVarValue: order).Send();

            /*
             * If the user just purchased a subscription, we need to get that in synch now..
             */
            var subscriptionItem = (from i in order.Items where i.ProductType == "Subscription" select i).FirstOrDefault();
            if (subscriptionItem != null)
            {
              User.GetSubscriptions(context);
            }
          }
          catch (IwsException iex)
          {
            var reason = iex.Reason.ToLower();
            if (reason != "decline_alreadyinservice" && reason.Contains("decline"))
            {
              /*
               * Clear the users's "has valid payment method" status
               */
              db.spUserUpdate(
                intVisitID: context.VisitID,
                intUserID: context.AuthorizedUserID,
                intSiteID: context.SiteID,
                boolHasValidPaymentMethod: false);

              throw new CreditCardDeclinedException();
            }
            throw;
          }
        }
        return order;
      }
    }

    public static int CreateFromIwsOrder(
      CallContext context, 
      string strOrderXml, 
      int? intQueryID = null, 
      string strProfileID = null,
      bool boolIsStorefrontXml=true,
      bool boolIsMultipleOrders=false)
    {
      using (var db = new AciXDB())
      {
        var intOrderID = db.spOrderCreate(
          intSiteID: context.SiteID,
          intUserID: context.AuthorizedUserID,
          strOrderXml: strOrderXml,
          intQueryID: intQueryID,
          strProfileID: strProfileID,
          boolIsStorefrontXml: boolIsStorefrontXml,
          boolIsMultipleOrders: boolIsMultipleOrders);

        return intOrderID;
      }
    }

    public static ClientLib.Order[] Search(
      CallContext context,
      string strFirstName = null,
      string strLastName = null,
      string strState = null,
      string strProfileID = null,
      int? intUserID=null,
      int[] intExternalOrderIDs = null)
    {
      using (var db = new AciXDB())
      {
        var orderIDs = db.spOrderSearch(
          intSiteID: context.SiteID,
          intUserID: context.AuthorizedUserID,
          strFirstName: strFirstName,
          strLastName: strLastName,
          strState: strState,
          strProfileID: strProfileID,
          intSelectedUserID: intUserID,
          intExternalOrderIDs: intExternalOrderIDs);
        return Get(context, orderIDs);
      }
    }

    /*
     * This should be called without business rules enforcing access.
     */
    public static ClientLib.Order Get(CallContext context, int intOrderID)
    {
      var orders = Get(context, new int[] { intOrderID });
      return orders.Length > 0 ? orders[0] : null;
    }

    /*
     * This public be called without business rules enforcing access.
     */
    public static ClientLib.Order[] Get(CallContext context, int[] keys)
    {
      var dbOrders = SiteOrderCache.ForSite(context.SiteID).Get(keys);
      return (from o in dbOrders select Render(o)).ToArray();
    }

    public static ClientLib.Order GetByExternalID(CallContext context, int intExternalID)
    {
      var orders = Search(context: context, intExternalOrderIDs: new int[] { intExternalID} );
      return orders.Length > 0 ? orders[0] : null;
    }

    public static int[] ValidateOrderAccess(CallContext context, int[] intOrderIDs)
    {
      using (var db = new AciXDB())
      {
        return db.spOrderValidateAccess(
          intAuthorizedUserID: context.AuthorizedUserID,
          intSiteID: context.SiteID,
          intKeys: intOrderIDs);
      }
    }

  }
}
