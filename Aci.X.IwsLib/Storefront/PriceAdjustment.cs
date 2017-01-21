using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class PriceAdjustment
  {
    public string code; // PRICE, PERCENT, ABSOLUTE
    public decimal amount;
    public string type; // CLUBINTELIUS, PROMOTION, BUNDLE, USER, VOUCHER
    public string text;
  }
}
