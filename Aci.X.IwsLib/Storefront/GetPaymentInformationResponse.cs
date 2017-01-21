using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class GetPaymentInformationResponse
  {
    public Wallet wallet;
    public ResponseDetail responseDetail;
    public string resplacementToken;
    public string responseCode;
  }
}
