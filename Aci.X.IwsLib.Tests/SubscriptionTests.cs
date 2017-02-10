using System.Linq;
using System;
using Aci.X.ClientLib;
using Aci.X.WebAPI.Tests;
using Aci.X.WebAPI.Tests.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Solishine.X.WebAPI.Tests.Controllers
{
  [TestClass]
  public class OrderControllerTests
  {
    const string CLIENT_SECRET = "3DF07A4B-9C67-4EC8-9C2F-7760AF23361C";
    private WebServiceClient _client;
    private Product[] _products;

    private class ExpectedProduct
    {
      public string ProductCode;
      public decimal Price;

      public override string ToString()
      {
        return String.Format("{0}: {1:0.00}", ProductCode, Price);
      }
    }

    [TestInitialize]
    public void TestInitialize()
    {
      User user;
      _client = new WebServiceClient(CLIENT_SECRET, "127.0.0.1");
      VisitControllerTests.CreateTestVisit(_client);
      user = UserControllerTests.CreateTestUser(_client, "Password1.");

      _products = _client.Get<Product[]>("catalog/products");
      _client.AssertHttpOK();

#if !TESTING_IN_PRODUCTION
      const string CC = "4444424444444440";

      CreditCard cc = new CreditCard
      {
        CreditCardNumber = CC,
        CVV = "111",
        ExpirationDate = "0715",
        CardHolderName = "Joe Test",
        Address = "123 Any street",
        City = "Los Angeles",
        State = "CA",
        Zip = "90049"
      };
      // should fail because of bad state
      user = _client.Post<User>("user/card", cc);
      _client.AssertHttpOK();
#endif
      Assert.IsTrue(user.HasValidPaymentMethod);
    }

    [TestMethod]
    public void SUBSCRIPTION_CancelAll()
    {
      _client.Get("user/subscriptions/cancel-all");
      _client.AssertHttpOK();
    }

    private void VerifyCatalog(Product[] actualProducts, ExpectedProduct[] expectedProducts)
    {
      Assert.AreEqual(expectedProducts.Length, actualProducts.Length, "Catalog size is wrong");
      foreach (var expectedProduct in expectedProducts)
      {
        var actualProduct = (
          from p in actualProducts
          where (p.ProductCode == expectedProduct.ProductCode
            && p.Skus.Length == 1
            && p.Skus[0].Price == expectedProduct.Price)
          select p).FirstOrDefault();
        Assert.IsNotNull(actualProduct, "Did not find " + expectedProduct.ToString());
      }
    }

    [TestMethod]
    public void SUBSCRIPTION_Test()
    {
      _client.Get("user/subscriptions/cancel-all");
      _client.AssertHttpOK();
      var order = CreateOrder("SCC_Sub", strReportTypeCode: "SCC");
      _client.Get("user/subscriptions");
      _client.AssertHttpOK();

      _client.Get("user/subscriptions/cancel-all");
      _client.AssertHttpOK();
      order = CreateOrder("PL_Sub", strReportTypeCode: "PL");
      _client.Get("user/subscriptions");
      _client.AssertHttpOK();

    }

    [TestMethod]
    public void SUBSCRIPTION_Test_PL_Sub()
    {
      TestSubscription(
        strSubscriptionProductCode: "PL_Sub",
        strFreeProductCode: "PL",
        intFreeProductQuantity: 10,
        dOtherProductDiscount: 0.30m);
    }

    [TestMethod]
    public void SUBSCRIPTION_Test_SCC_Sub()
    {
      TestSubscription(
        strSubscriptionProductCode: "SCC_Sub",
        strFreeProductCode: "SCC",
        intFreeProductQuantity: 3,
        dOtherProductDiscount: 0.50m);
    }

    private void TestSubscription(
      string strSubscriptionProductCode,
      string strFreeProductCode,
      int intFreeProductQuantity,
      decimal dOtherProductDiscount)
    {
      var defaultProducts = new ExpectedProduct[] {
        new ExpectedProduct { ProductCode="PL", Price=2.00m},
        new ExpectedProduct { ProductCode="SCC", Price=20.00m},
        new ExpectedProduct { ProductCode="NCC", Price=40.00m},
        new ExpectedProduct { ProductCode="PL_Sub", Price=9.99m},
        new ExpectedProduct { ProductCode="SCC_Sub", Price=19.99m}
      };
      var dictDefaultProducts = defaultProducts.ToDictionary(n => n.ProductCode);

      SUBSCRIPTION_CancelAll();
      /*
       * Initial catalog should include a PL, SCC, NCC, PL_Sub, SCC_Sub
       */
      _products = _client.Get<Product[]>("catalog/products");
      var expectedProducts = defaultProducts;
      VerifyCatalog(_products, expectedProducts);
      /*
       * Purchase the subscription
       */
      var subscriptionOrder = CreateOrder(strSubscriptionProductCode, strFreeProductCode);
      var sub = _client.Get("user/subscriptions");
      /*
       * Refresh the catalog
       */
      _products = _client.Get<Product[]>("catalog/products");
      _client.AssertHttpOK();

      /*
       * For now, a user can only have one subscription at a time,
       * so they shouldn't see any subscriptions now.
       */
      expectedProducts = new ExpectedProduct[] {
        new ExpectedProduct 
        { 
          ProductCode="PL", 
          Price=(strFreeProductCode=="PL")
            ? 0.00m
            : dictDefaultProducts["PL"].Price
        }, 
        new ExpectedProduct 
        { 
          ProductCode="SCC", 
          Price=(strFreeProductCode=="SCC")
            ? 0.00m
            : dictDefaultProducts["SCC"].Price * (1.00m - dOtherProductDiscount)
        }, // discounted
        new ExpectedProduct 
        { 
          ProductCode="NCC", 
          Price=dictDefaultProducts["NCC"].Price * (1.00m - dOtherProductDiscount)
        }, // discounted
      };
      VerifyCatalog(_products, expectedProducts);

      /*
       * Verify that we can get the right number of free reports.
       */
      int intRemainingFree = intFreeProductQuantity;
      while (intRemainingFree>0)
      {
        _products = _client.Get<Product[]>("catalog/products");
        _client.AssertHttpOK();
        var prod = (from p in _products where p.ProductCode == strFreeProductCode select p).First();
        Assert.AreEqual(1, prod.Skus.Length);
        Assert.AreEqual(intRemainingFree, prod.Skus[0].SubscriptionQuantityRemaining);
        Assert.AreEqual(0, prod.Skus[0].Price);
        var order = CreateOrder(
          strProductCode: strFreeProductCode, 
          strReportTypeCode: strFreeProductCode, 
          strFirstName: "John", 
          strLastName: "Weston", 
          strState: "CA");
        Assert.AreEqual(subscriptionOrder.OrderID, order.OrderID);
        Assert.AreEqual(subscriptionOrder.OrderTotal, order.OrderTotal);

        /*
         * The order is either the original order purchasing the subscription,
         * or it's a subscription rebill.   It should NOT have a line time for
         * the free product type.
         */
        var freeLineItems = (from i in order.Items where i.ProductCode == strFreeProductCode select i);
        var subLineItems = (from i in order.Items where i.ProductCode == strSubscriptionProductCode select i);
        Assert.AreEqual(0, freeLineItems.Count());
        Assert.AreEqual(1, subLineItems.Count());
        intRemainingFree--;
      }
      /*
       * Now we've used up our free reports
       */
      _products = _client.Get<Product[]>("catalog/products");
      _client.AssertHttpOK();
      expectedProducts = new ExpectedProduct[] {
        new ExpectedProduct { ProductCode="PL", Price=dictDefaultProducts["PL"].Price}, // not free anymore
        new ExpectedProduct { ProductCode="SCC", Price=dictDefaultProducts["SCC"].Price * (1.00m - dOtherProductDiscount)}, // discounted
        new ExpectedProduct { ProductCode="NCC", Price=dictDefaultProducts["NCC"].Price * (1.00m - dOtherProductDiscount)}, // discounted
      };
      VerifyCatalog(_products, expectedProducts);

      /*
       * let's create an order for each product at the prices above
       */
      foreach (var prod in expectedProducts)
      {
        var newOrder = CreateOrder(
          strProductCode: prod.ProductCode, 
          strReportTypeCode: prod.ProductCode, 
          strFirstName: "John", 
          strLastName: "Weston", 
          strState: "CA");
        Assert.AreEqual(prod.Price, newOrder.Subtotal);
      }


    }

    [TestMethod]
    public void SUBSCRIPTION_Get_Order_X()
    {
      _client.Get("user/subscriptions/cancel-all");
      _client.AssertHttpOK();

      var statewideOrder = CreateOrder("SCC","Todd", "Jones", "CA");
      
      var nationwideOrder = CreateOrder("NCC","Todd", "Jones", null);

      var order = _client.Get<Order>("order/{0}", statewideOrder.OrderID);
      _client.AssertHttpOK();
      Assert.AreEqual(statewideOrder.OrderID, order.OrderID);

      order = _client.Get<Order>("order/{0}", nationwideOrder.OrderID);
      _client.AssertHttpOK();
      Assert.AreEqual(nationwideOrder.OrderID, order.OrderID);

    }

    [TestMethod]
    public void SUBSCRIPTION_Get_Subscriptions()
    {
      _client.Get("user/subscriptions");
      _client.AssertHttpOK();
    }

    [TestMethod]
    public void SUBSCRIPTION_GET_Catalog_Refresh()
    {
      WebServiceClient client = new WebServiceClient(CLIENT_SECRET, "127.0.0.1");
      VisitControllerTests.CreateTestVisit(client);
      client.Get("catalog/refresh");
      client.AssertHttpOK();
    }

    private Order CreateOrder(
      string strProductCode,
      string strReportTypeCode,
      string strFirstName=null,
      string strLastName=null,
      string strState=null)
    {
      string strProfileID = null;
      int intQueryID=0;

      var product = (
              from p in _products
              where p.ProductCode.Equals(strProductCode)
              select p).First();

      var productSkuID = product.Skus[0].SkuID;

      if (product.RequireQuery || product.RequireProfile)
      {
        // First add a query to the visit context.
        var srp = _client.Get<SearchResultsPage>("search/preview?firstname={0}&lastname={1}&state={2}",
          strFirstName, strLastName, strState);
        _client.AssertHttpOK();
      

        // Now add a profile ID 
        strProfileID = srp.Profiles.Profile[0].ProfileID;
        intQueryID = srp.QueryID;
      }


      Cart cart = _client.Get<Cart>("cart/add/{0}?profile_id={1}", productSkuID, strProfileID);
      _client.AssertHttpOK();

      var order = _client.Get<Order>("cart/checkout");
      _client.AssertHttpOK();

      var report = _client.Get<Report>(
        "report/new?order_id={0}&profile_id={1}&query_id={2}&type_code={3}",
         order.OrderID, strProfileID, intQueryID, strReportTypeCode);
      return order;
    }

  }
}
