using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class ValidateRequest
  {
    public List<ValidateProductOffering> productOfferings;
    public PaymentMethod payment;
    public TransactionRequestDetails transactionDetails;
    public List<string> options; //DISABLE_TAX, DISABLE_AUTO_DEBIT
  }
}
