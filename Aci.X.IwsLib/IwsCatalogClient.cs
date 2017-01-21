using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.DatabaseEntity;
using Catalog = Aci.X.IwsLib.Commerce.v2_1.Catalog;
using NLog;



namespace Aci.X.IwsLib
{
  public class IwsCatalogClient : CommerceClientBase
  {
    private Catalog.CatalogServicesPortTypeClient _iwsClient;
    private static NLogger _logger = new NLogger(LogManager.GetCurrentClassLogger());

    public IwsCatalogClient()
    {
      _iwsClient = new Catalog.CatalogServicesPortTypeClient(
      "CatalogServicesPort",
      IwsConfig.CatalogApiBaseUrl);
    }

    private Catalog.ClientAuthType ClientAuth
    {
      get 
      {
        return new Catalog.ClientAuthType
        {
          ClientID = base.ClientID,
          AuthID = base.AuthID,
          AuthKey = base.AuthKey
        };
      }
    }

    public string GetProductsXml(int[] intExternalProductIDs)
    {
      List<Catalog.ProductItemType> listRequestedProducts = new List<Catalog.ProductItemType>();
      foreach (int intExternalProductID in intExternalProductIDs)
      {
        listRequestedProducts.Add(
          new Catalog.ProductItemType
          {
            ProductID = intExternalProductID,
            ProductIDSpecified = true
          });
      }

      /******* ******** TEMP TEMP TEMP
       *
       */
      listRequestedProducts.Clear();
      listRequestedProducts.Add(
        new Catalog.ProductItemType
        {
          ProductToken = "1-FufqyMzUX35wotSwJkZyPJFmjwzDCoQWXAHSOtcqK3pAZKHFu45rP_wcC4YT7pkmIgTIH7KoeqJ-8gGVpeIa4MecrEwXbv56HPf2bwE5Jkzu4JFHA3ameaUFdZkosRAJfi-VlOUR2DxwC4BMI9_DEw"
        });
      
      var requestedProducts = listRequestedProducts.ToArray();

      Catalog.GetProductRequest catalogRequest = new Catalog.GetProductRequest
      {
        ClientAuth = ClientAuth,
        ProductList = requestedProducts
      };
      try
      {

        Catalog.GetProductResponse productResponse = _iwsClient.GetProductAsync(catalogRequest).Result;
        if (productResponse.CompletionResponse.CompletionCode == "1000")
        {
          /*
           * Need to do a little bit of hackery here.   
           * For subscriptions, IWS has two products: one for the initial signup, 
           * and one for the recurring order.  We are only interested in the first one.
           *
           * Also, for some reason, IWS returns 329 for the IDP product, but 420 is correct.
           */
          Dictionary<string, Catalog.Product> dictProducts = new Dictionary<string, Catalog.Product>();

          foreach (var product in productResponse.Products)
          {
            var productReferenceID = product.ProductReferenceID;
            if (productReferenceID == "329")
            {
              product.ProductReferenceID = "420";
              dictProducts[product.ProductReferenceID] = product;
            }
            else if (product.ProductType == "Subscription")
            {
              int intServiceGroupID = product.SubscriptionDetails.ServiceGroupID;
              if (dictProducts.ContainsKey(productReferenceID) == false ||
                dictProducts[productReferenceID].SubscriptionDetails.ServiceGroupID > intServiceGroupID)
              {
                dictProducts[productReferenceID] = product;
              }
            }
            else dictProducts[productReferenceID] = product;
          }
          string strXml = Solishine.CommonLib.XmlHelper.Serialize(dictProducts.Values.ToArray(), System.Text.UnicodeEncoding.Unicode);
          return strXml;
        }
        else
        {
          throw new Exception(
            productResponse.CompletionResponse.CompletionCode + ":" +
            productResponse.CompletionResponse.ResponseDetail);
        }
      }
      catch (Exception ex)
      {
        _logger.LogEvent(LogLevel.Error, "{0} at {1}", ex.Message, ex.StackTrace);
        throw;
      }
    }
  }
}
