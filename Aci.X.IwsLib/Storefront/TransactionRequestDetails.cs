using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class TransactionRequestDetails
  {
    public string pointOfOrigin; // STORE, PRODUCT_ADDONS, INTERSTITIAL_ADDONS
    public int referrer;
    public string adword;
    public int parentPurchaseId;
    public string context; // INTERSTITIAL, TOPDOWN, INCART, PARTNER
    public int abTestCaseId;
    public string referenceId;
    public string descriptor;
    public string userAgent;
    public string timeZone;
    public string clientIp;
  }
}
