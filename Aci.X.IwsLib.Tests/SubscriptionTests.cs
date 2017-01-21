#define TESTING_IN_PRODUCTION
using System.Linq;
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
    private Category[] _categories;

    [TestInitialize]
    public void TestInitialize()
    {
      User user;
      _client = new WebServiceClient(CLIENT_SECRET, "127.0.0.1");
      VisitControllerTests.CreateTestVisit(_client);
      _categories = _client.Get<Category[]>("catalog/categories");
      _client.AssertHttpOK();

#if TESTING_IN_PRODUCTION
      UserCredentials creds = new UserCredentials
      {
        EmailAddress = "aci-cr-test1@intelius.com",
        Password = "88l/obggZ,)Peowc"
      };
      user = _client.Post<User>("user/authenticate", creds);
      _client.AssertHttpOK();

      user = _client.Get<User>("user/current");
      _client.AssertHttpOK();
#else
      UserControllerTests.CreateTestUser(_client, "Password1.");
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
      User user = _client.Post<User>("user/card", cc);
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

    [TestMethod]
    public void SUBSCRIPTION_Create()
    {
      var order = CreateOrder("Sub_SCC");
      var sub = _client.Get("user/subscriptions");
    }

    [TestMethod]
    public void SUBSCRIPTION_Get_Order_X()
    {
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
      string strCategoryCode,
      string strFirstName=null,
      string strLastName=null,
      string strState=null)
    {
      string strProfileID = null;

      var category = (
              from p in _categories
              where p.CategoryCode.Equals(strCategoryCode)
              select p).First();

      var productSkuID = category.SKUs[0].SkuID;

      foreach (var sku in category.SKUs)
      {
        foreach (var product in sku.Products)
        {
          if (product.RequireQuery || product.RequireProfile)
          {
            // First add a query to the visit context.
            var srp = _client.Get<SearchResultsPage>("search/preview?firstname={0}&lastname={1}&state={2}",
              strFirstName, strLastName, strState);
            _client.AssertHttpOK();

            // Now add a profile ID 
            strProfileID = srp.Profiles.Profile[0].ProfileID;
            break;
          }
        }
      }


      Cart cart = _client.Get<Cart>("cart/add/{0}?profile_id={1}", productSkuID, strProfileID);
      _client.AssertHttpOK();

      var order = _client.Get<Order>("cart/checkout");
      _client.AssertHttpOK();
      return order;
    }

  }
}
