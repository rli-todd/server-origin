using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class TransactTransaction
  {
    public int transactionId;
    public string transactionDate;
    public decimal totalPrice;
    public decimal totalTax;
    public decimal totalAmount;
    public decimal totalPriceAdjustment;
    public TransactionDetails transactionDetails;
    public List<TransactionLineItem> lineItems;
    public TransactTransactionSplit split;
  }
}
