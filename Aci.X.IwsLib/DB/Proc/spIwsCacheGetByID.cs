using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spIwsCacheGetByID : MyStoredProc
  {
    public spIwsCacheGetByID(DbConnection conn)
      : base("spIwsCacheGetByID", conn)
    {
      Parameters.Clear();
      Parameters.Add("@QueryID", System.Data.SqlDbType.Int);
    }

    public DBIwsCachedPreview Execute(int intQueryID)
    {
      Parameters["@QueryID"].Value = intQueryID;

      using (MySqlDataReader reader = base.ExecuteReader())
      {
        DBIwsCachedPreview[] results = reader.GetResults<DBIwsCachedPreview>();
        return results.FirstOrDefault();
      }
    }
  }
}
