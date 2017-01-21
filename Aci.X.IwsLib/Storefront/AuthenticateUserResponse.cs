using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class AuthenticateUserResponse
  {
    public int responseCode;
    public UserAuthentication userAuthentication;
    public ResponseDetail responseDetail;
  }
}
