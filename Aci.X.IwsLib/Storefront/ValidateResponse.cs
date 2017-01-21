using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class ValidateResponse
  {
    public int responseCode;
    public ResponseDetail responseDetail;
    public string token;
    public ValidateTransaction transaction;
    public decimal totalPrice;
    public decimal totalTax;
    public decimal totalAmount;
    public decimal totalPriceAdjustment;
  }
}
