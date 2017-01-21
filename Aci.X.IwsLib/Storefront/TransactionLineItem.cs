using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class TransactionLineItem
  {
    public int lineItemId;
    public int parentLineItemId;
    public Product product;
    public List<ReportCredential> reportCredentials;
  }
}
