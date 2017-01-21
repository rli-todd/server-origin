using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  public class spApiClientIsAuthorized : MyStoredProc
  {
    public spApiClientIsAuthorized(DbConnection conn)
      : base(strProcName: "spApiClientIsAuthorized", conn: conn)
    {
      Parameters.Add("@ClientSecret", System.Data.SqlDbType.VarChar );
      SqlParameter param = new SqlParameter("@RetVal", System.Data.SqlDbType.Int);
      param.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Add(param);
    }
    public bool Execute( string strClientSecret)
    {
      Parameters["@ClientSecret"].Value = strClientSecret;
      using (MySqlDataReader reader = ExecuteReader())
      {
        return 0 != (int)Parameters["@RetVal"].Value;
      }
    }
  }
}



