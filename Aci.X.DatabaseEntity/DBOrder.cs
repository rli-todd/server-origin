using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBOrder : MySqlResult
  {
    public int SiteID;
    public int OrderID;
    public int OrderExternalID;
    public int UserID;
    public int VisitID;
    public DateTime OrderDate;
    public decimal Subtotal;
    public decimal Tax;
    public decimal OrderTotal;
    public DBOrderItem[] Items;

    public override void Read()
    {
      SiteID = Value<byte>("SiteID");
      OrderID = Value<int>("OrderID");
      OrderExternalID = Value<int>("OrderExternalID");
      UserID= Value<int>("UserID");
      VisitID = Value<int>("VisitID");
      OrderDate = Value<DateTime>("OrderDate");
      Subtotal = Value<decimal>("Subtotal");
      Tax = Value<decimal>("Tax");
      OrderTotal = Value<decimal>("OrderTotal");
    }
  }

  public class DBOrderItem : MySqlResult
  {
    public int SiteID;
    public int OrderItemID;
    public int OrderItemExternalID;
    public int OrderID;
    public int SkuID;
    public int ProductID;
    public int ProductExternalID;
    public string ProductToken;
    public string ProductName;
    public int Quantity;
    public decimal RegularPrice;
    public decimal Price;
    public decimal DiscountAmount;
    public string DiscountDescription;
    public decimal RecurringPrice;
    public int? QueryID;
    public string ProfileID;
    public string FirstName;
    public string MiddleInitial;
    public string LastName;
    public string State;
    public string ReportTypeCode;


    public override void Read()
    {
      SiteID = Value<byte>("SiteID");
      OrderID = Value<int>("OrderID");
      OrderItemID = Value<int>("OrderItemID");
      OrderItemExternalID = Value<int>("OrderItemExternalID");
      SkuID = Value<int>("SkuID");
      ProductID= Value<int>("ProductID");
      ProductExternalID = Value<int>("ProductExternalID");
      ProductToken = Value<string>("ProductToken");
      ProductName = Value<string>("CategoryName");
      Quantity = Value<short>("Quantity");
      RegularPrice = Value<decimal>("RegularPrice");
      Price = Value<decimal>("Price");
      DiscountAmount = Value<decimal>("DiscountAmount");
      DiscountDescription = Value<string>("DiscountDesciption");
      RecurringPrice = Value<decimal>("RecurringPrice");
      ReportTypeCode = Value<string>("ReportTypeCode");
      QueryID = Value<int?>("QueryID");
      ProfileID = Value<string>("ProfileID");
      FirstName = Value<string>("FirstName");
      MiddleInitial = Value<string>("MiddleInitial");
      LastName = Value<string>("LastName");
      State = Value<string>("State");
    }
  }
}
