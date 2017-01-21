using Aci.X.ServerLib;

namespace Aci.X.Database
{
  public partial class ProfileDB : DalSqlDb
  {
    public ProfileDB()
      : base(conn: WebServiceConfig.ProfileSqlConnection)
    {
      
    }
  }
}
