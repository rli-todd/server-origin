using System;
using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.ClientLib.Exceptions;
using Aci.X.Database;
using Aci.X.IwsLib;
using Aci.X.ServerLib;

namespace Aci.X.Business
{
  public class Cart
  {
    public static void Update(CallContext context, ClientLib.Cart cart)
    {
      context.DBVisit.Cart = cart;
      Visit.Cache.Set(context.DBVisit);
    }

    public static ClientLib.Cart Get(CallContext context)
    {
      return context.DBVisit.Cart ?? new ClientLib.Cart();
    }

    public static ClientLib.Cart AddSku(
      CallContext context, 
      int intSkuID, 
      string strProfileID, 
      string strFirstName,
      string strLastName,
      string strState,
      string strPromoCode)
    {
      var catalog = CatalogCache.Singleton.Get(context.SiteID);
      var products = catalog.DictSkuProducts[intSkuID].ToArray();
      var dbVisit = context.DBVisit;

      // Deprecating the reliance on the saved QueryID
      if (strFirstName != null && strLastName != null)
      {
        var response = ProfileHelper.GetPreviews(
          new ClientLib.PersonQuery
          {
            FirstName = strFirstName,
            LastName = strLastName,
            State = strState
          });
        if (response != null & response.QueryID != 0)
        {
          Visit.SetStateAndQueryID(context, strState, response.QueryID);
        }
      }

      for (int idxProduct = 0; idxProduct < products.Length; ++idxProduct)
      {
          var product = products[idxProduct];
          if (product.RequireQueryID && context.DBVisit.CurrentQueryID == 0)
          {
              throw new QueryRequiredException();
          }
          if (product.RequireState && String.IsNullOrEmpty(context.DBVisit.CurrentQueryState) && String.IsNullOrEmpty(strState))
          {
              throw new StateRequiredException();
          }
          if (product.RequireProfileID && String.IsNullOrEmpty(strProfileID))
          {
              throw new ProfileRequiredException();
          }
          /*
           * THIS WILL NEED TO BE OPTIMIZED
           */
          if (strPromoCode != null && product.ProductToken != null)
          {
              var sfCli = new Aci.X.IwsLib.Storefront.StorefrontClient(context);
              var ret = sfCli.ApplyPromoItem(product.ProductToken.Replace("\n",""), strPromoCode);
          }

      }
      var cart = Get(context);
      cart.Items = (from p 
                          in catalog.DictSkuProducts[intSkuID] 
                        select Product.GetOrderItem(context, p, strProfileID: strProfileID, strState: strState, intQueryID: context.DBVisit.CurrentQueryID))
                        .ToArray();
      Update(context, cart);
      using (var db=new AciXDB())
      {
        db.spCartUpdate(
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          intProductID: cart.Items[0].ProductID,
          intQueryID: cart.Items[0].QueryID,
          strProfileID: cart.Items[0].ProfileID,
          strStateAbbr: cart.Items[0].State);
      }
      return Get(context);
    }

    public static ClientLib.Cart RemoveSku(CallContext context, int intSkuID)
    {
      var cart = Get(context);
      cart.Remove(intSkuID);
      Update(context, cart);
      return Get(context);
    }
  }
}
