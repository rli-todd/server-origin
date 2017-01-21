using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class ReportCredential
  {
    public string reportCredential;
    public string expiration;
    public string create;
    public bool active;
    public int userId;
    public string type; // SINGLE, UNLIMITED
    public string stragey; // DEFAULT, PURCHASE, SHELLREPORT, UNKNOWN
    public List<int> bundleIds;
    public List<int> moduleIds;
    public int availableCount;
  }
}
