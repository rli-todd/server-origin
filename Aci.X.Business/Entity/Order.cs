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
                   SkuID = i.SkuID,
                   ProductName = i.ProductName +
                    (i.FirstName == null 
                      ? ""
                      : " for " + i.FirstName +
                                    (String.IsNullOrEmpty(i.MiddleInitial.Trim()) ? "" : " " + i.MiddleInitial) +
                                    " " + i.LastName +
                                    (String.IsNullOrWhiteSpace(i.State) ? "" : " in " + GeneralPurposeCache.Singleton.StateByAbbr(i.State).StateName)),
                   Quantity = i.Quantity,
                   RegularPrice = i.RegularPrice,
                   Price = i.Price,
                   DiscountAmount = i.DiscountAmount,
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
      /*
       * Note that while the IWS API supports orders containing multiple
       * different product IDs, there is no way for us to attach our own 
       * identifiers to individual Order Items, so this effectively prevents 
       * us from ever purchasing more than one item at a time.
       * 
       * We are half-way modeled around a structure that could support multiple
       * purchased items, so to work within this IWS constraint is a bit of a hack.
       */
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

      IwsTransactionClient tranCli = new IwsTransactionClient(context);
      var storefrontCli = new SF.StorefrontClient(context);
      var catalog = CatalogCache.Singleton.Get(context.SiteID);
      try
      {
        var sfRet = storefrontCli.Checkout();
        string strOrderXml = Solishine.CommonLib.XmlHelper.Serialize(sfRet);

        strOrderXml = tranCli.Checkout(catalog.DictSkuProducts);
        var intOrderID = CreateFromIwsOrder(context, strOrderXml, intQueryID: intQueryID, strProfileID: strProfileID);
        /*
          * Hacky: We can't get all the order item details from the IWS XML,
          * so we'll set the OrderItem level attributes one at a time.
          */
        foreach (var sku in context.DBVisit.Cart.Items)
        {
          //db.spOrderUpdateItem(
          //  intOrderID: intOrderID,
          //  intProductSkuID: sku.)
        }
        var order = Get(context, new int[] { intOrderID })[0];
        MandrillClient.NewMessage(context, "PurchaseReceipt", strVarName: "order", oVarValue: order).Send();
        return order;
      }
      catch (IwsException iex)
      {
        var reason = iex.Reason.ToLower();
        if (reason != "decline_alreadyinservice" && reason.Contains("decline"))
        {
          /*
           * Clear the users's "has valid payment method" status
           */
          using (var db = new AciXDB())
          {
            db.spUserUpdate(
              intVisitID: context.VisitID,
              intUserID: context.AuthorizedUserID,
              intSiteID: context.SiteID,
              boolHasValidPaymentMethod: false);
          }
          throw new CreditCardDeclinedException();
        }
        throw;
      }

    }

    public static int CreateFromIwsOrder(CallContext context, string strOrderXml, int? intQueryID = null, string strProfileID = null)
    {
      using (var db = new AciXDB())
      {
        var intOrderID = db.spOrderCreate(
          intSiteID: context.SiteID,
          strOrderXml: strOrderXml,
          intQueryID: intQueryID,
          strProfileID: strProfileID);

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
