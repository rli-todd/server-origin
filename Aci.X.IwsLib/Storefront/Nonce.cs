using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class Nonce
  {
    public string nonce;
    public string signednonce;
    public string timestamp;
  }
  public class NonceResponce
  {
    public Nonce nonce;
    public int responseCode;
  }
}
