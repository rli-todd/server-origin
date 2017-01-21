using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class Product
  {
    public string productOfferingId;
    public string reference;
    public string productId;
    public string bundleId;
    public int quantity;
    public decimal price;
    public decimal tax;
    public decimal amount;
    public List<PriceAdjustment> priceAdjustments;
    public string title;
    public decimal msrp;
    public decimal listPrice;
    public List<ProductBase> addons;
  }
}