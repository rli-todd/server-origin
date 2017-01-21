using Aci.X.ServerLib;

namespace Aci.X.Database
{
  public partial class GeoDB: DalSqlDb
  {
    public GeoDB()
      : base(conn: WebServiceConfig.GeoSqlConnection)
    {

    }
  }
}
