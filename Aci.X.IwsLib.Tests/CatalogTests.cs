using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Catalog = Aci.X.IwsLib.Commerce.v2_1.Catalog;
using Customer = Aci.X.IwsLib.Commerce.v2_1.Customer;
using Aci.X.ClientLib;

namespace Aci.X.IwsLib.Tests
{
  [TestClass]
  public class CatalogTests
  {
    private const string _strEmailAddress = "p-trobertson@intelius.com";
    private const string _strPassword = "publicrecords500";

    private static Catalog.ClientAuthType _catalogClientAuth = new Catalog.ClientAuthType
    {
      // The following credentials were in Justin's sample code
      ClientID = "100010",
      AuthID = "7693969",
      AuthKey = "Ra#d@#87"
    };

    private Catalog.UserContextType GetUserContext(string strUserToken)
    {
      return new Catalog.UserContextType
      {
        UserToken = strUserToken,
        EmailAddress = _strEmailAddress
      };
    }

    private Customer.LoginRequest GetLoginRequest()
    {
      return new Customer.LoginRequest
      {
        UserName = _strEmailAddress,
        Password = _strPassword,
        RemoteIp = "127.0.0.1"
      };
    }

    private static Customer.ClientAuthType _customerClientAuth = new Customer.ClientAuthType
    {
      // The following credentials were in Justin's sample code
      ClientID = "100010",
      AuthID = "7693969",
      AuthKey = "Ra#d@#87"
    };

    private Customer.AuthenticateUserRequest GetAuthenticateRequest()
    {
      return new Customer.AuthenticateUserRequest
      {
        ClientAuth = _customerClientAuth,
        LoginRequest = GetLoginRequest(),
      };
    }

    /*
ProductGroup=1, OrderLocation=0, Ref=451, Title=<b>Background Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=1, OrderLocation=0, Ref=451, Title=<b>Background Report</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=7, OrderLocation=0, Ref=452, Title=<b>Statewide Criminal Check</b>, Group=, Type=AlaCart, Desc=
ProductGroup=7, OrderLocation=0, Ref=452, Title=<b>Statewide Criminal Check</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=11, OrderLocation=0, Ref=453, Title=<b>Nationwide Criminal Check</b>, Group=, Type=AlaCart, Desc=
ProductGroup=11, OrderLocation=0, Ref=453, Title=<b>Nationwide Criminal Check</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=26, OrderLocation=0, Ref=450, Title=<b>Instant People Lookup Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=26, OrderLocation=0, Ref=450, Title=<b>Instant People Lookup Report</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=27, OrderLocation=0, Ref=572, Title=<b>24 Hour Pass</b>, Group=, Type=AlaCart, Desc=
ProductGroup=27, OrderLocation=0, Ref=572, Title=<b>24 Hour Pass</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=37, OrderLocation=0, Ref=505, Title=Reverse Phone Lookup, Group=, Type=AlaCart, Desc=
ProductGroup=50, OrderLocation=0, Ref=595, Title=<b>Property & Neighborhood Report </b>, Group=, Type=AlaCart, Desc=
ProductGroup=50, OrderLocation=0, Ref=595, Title=<b>Property & Neighborhood Report </b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=82, OrderLocation=0, Ref=517, Title=<b>Business People Search Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=92, OrderLocation=0, Ref=288, Title=<b>Instant Email Lookup Report</b> , Group=, Type=AlaCart, Desc=
ProductGroup=128, OrderLocation=0, Ref=571, Title=<b>Social Net Search by Screen Name</b>, Group=, Type=AlaCart, Desc=
ProductGroup=138, OrderLocation=0, Ref=597, Title=Single Marriage and Divorce Record Report, Group=, Type=AlaCart, Desc=
ProductGroup=138, OrderLocation=0, Ref=597, Title=Single Marriage and Divorce Record Report, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=139, OrderLocation=0, Ref=518, Title=Death Records, Group=, Type=AlaCart, Desc=
ProductGroup=142, OrderLocation=0, Ref=570, Title=<b>Social Net Lookup</b> <fname>, Group=, Type=AlaCart, Desc=
ProductGroup=163, OrderLocation=0, Ref=454, Title=<b>On-Site County Criminal Record Search</b>, Group=, Type=AlaCart, Desc=
ProductGroup=163, OrderLocation=0, Ref=454, Title=<b>On-Site County Criminal Record Search</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=164, OrderLocation=0, Ref=455, Title=<b>On-site NY Statewide Criminal Record Search</b>, Group=, Type=AlaCart, Desc=
ProductGroup=164, OrderLocation=0, Ref=455, Title=<b>On-site NY Statewide Criminal Record Search</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=194, OrderLocation=0, Ref=577, Title=<b>Public Records Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=194, OrderLocation=0, Ref=577, Title=<b>Public Records Report</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=278, OrderLocation=0, Ref=450, Title=<b>Instant People Lookup Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=278, OrderLocation=0, Ref=450, Title=<b>Instant People Lookup Report</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=279, OrderLocation=0, Ref=572, Title=<b>24 Hour Pass</b>, Group=, Type=AlaCart, Desc=
ProductGroup=279, OrderLocation=0, Ref=572, Title=<b>24 Hour Pass</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=316, OrderLocation=0, Ref=996, Title=<b>People Search Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=316, OrderLocation=0, Ref=996, Title=<b>People Search Report</b>, Group=, Type=AlaCartWithSubscription, Desc=
ProductGroup=317, OrderLocation=0, Ref=997, Title=<b>Online Voucher For People Search Report</b>, Group=, Type=AlaCart, Desc=
ProductGroup=317, OrderLocation=0, Ref=997, Title=<b>Online Voucher For People Search Report</b>, Group=, Type=AlaCartWithSubscription, Desc=
     */
    [TestMethod]
    public async Task Catalog_GetProduct()
    {
      Catalog.CatalogServicesPortTypeClient catalogClient = new Catalog.CatalogServicesPortTypeClient(
        "CatalogServicesPort",
        "https://iiwscommerce.intelius.com/commerce/2.1/catalog.php");
      
      Catalog.GetProductRequest catalogRequest = new Catalog.GetProductRequest
      {
        ClientAuth = _catalogClientAuth
        //,
        //SearchCriteria = new Catalog.SearchCriteriaType
        //{
        //  FirstName = "brian",
        //  MiddleName = "l",
        //  LastName = "smith"
        //}
        //ProductGroupID = 26,
        
        ,ProductList = new Catalog.ProductItemType[] { 
          new Catalog.ProductItemType { ProductID = 450, ProductIDSpecified=true}, // Instant People Lookup Report
          //new Catalog.ProductItemType { ProductID = 451, ProductIDSpecified=true}, // Background Report
          new Catalog.ProductItemType { ProductID = 452, ProductIDSpecified=true}, // Statewide criminal check
          new Catalog.ProductItemType { ProductID = 453, ProductIDSpecified=true} // Nationwide criminal check
          //new Catalog.ProductItemType { ProductID = 476, ProductIDSpecified=true}, // Email Lookup Report
          //new Catalog.ProductItemType { ProductID = 507, ProductIDSpecified=true}, // Instant Email Lookup Report
          //new Catalog.ProductItemType { ProductID = 572, ProductIDSpecified=true} // 24 Hour Pass
        }
      };
      try
      {

        Catalog.GetProductResponse productResponse = await catalogClient.GetProductAsync(catalogRequest);
        if (productResponse.CompletionResponse.CompletionCode == "1000")
        {
          foreach (Catalog.Product product in productResponse.Products)
          {
            System.Diagnostics.Debug.WriteLine(
              "ProductReferenceID={0}, "+
              "ProductType={1}, "+
              "Title={2}, "+
              "Quantity={3}, "+
              "Units={4}, "+
              "MSRPPrice={5}, "+
              "Price={6}",
              product.ProductReferenceID,
              product.ProductType,
              product.Title,
              product.Quantity,
              product.Units,
              product.PriceListings[0].MSRPPrice,
              product.PriceListings[0].Price);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }


    [TestMethod]
    public async Task Catalog_GetAddons()
    {
      Catalog.CatalogServicesPortTypeClient catalogClient = new Catalog.CatalogServicesPortTypeClient(
        "CatalogServicesPort",
        "https://iiwscommerce.intelius.com/commerce/2.1/catalog.php");

      Catalog.GetAddonsRequest catalogRequest = new Catalog.GetAddonsRequest
      {
        ClientAuth = _catalogClientAuth,
        SearchCriteria = new Catalog.SearchCriteriaType
        {
          FirstName = "brian",
          MiddleName = "l",
          LastName = "smith",
          RecordID = "08CK12K54VN" // brian lee smith
        },
        OrderLocation = 21, // Pre-Purchase
        ProductID=450 // instant people lookup report
      };
      try
      {
        foreach (var productID in new int[] {450, 452, 453})
        {
          catalogRequest.ProductID = productID;
          Catalog.GetAddonsResponse addonsResponse = await catalogClient.GetAddonsAsync(catalogRequest);
          if (addonsResponse.CompletionResponse.CompletionCode == "1000")
          {
            foreach (Catalog.Product product in addonsResponse.Addons)
            {
              System.Diagnostics.Debug.WriteLine(
                "Ref={0}, Title={1}, Group={2}, Type={3}, Desc={4}",
                product.ProductReferenceID,
                product.Title,
                product.GroupName,
                product.ProductType,
                product.Description);
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

  }
}
