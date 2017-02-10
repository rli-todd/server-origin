using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class TransactionResponse
  {
    public int responseCode;
    public ResponseDetail responseDetail;
    public TransactTransaction transaction;
  }

  public class TransactionGetResponse
  {
    public int responseCode;
    public ResponseDetail responseDetail;
    public List<TransactTransaction> transactions;
  }
}
