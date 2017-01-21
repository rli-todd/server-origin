using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class ValidateTransactionSplit
  {
    public decimal totalPrice;
    public decimal totalTax;
    public decimal totalAmount;
    public decimal totalPriceAdjustment;
    public DelayedBilling delayedBilling;
    public TransactionDetails transactionDetails;
    public List<Product> products;
  }
}
