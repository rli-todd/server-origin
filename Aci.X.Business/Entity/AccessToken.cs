using System;

namespace Aci.X.Business.Entity
{
  public class AccessToken
  {
    public string Token { get; set; }
    public int UserID { get; set; }
    public string[] Roles { get; set; }
    public DateTime ExpiryDate { get; set; }
  }
}
