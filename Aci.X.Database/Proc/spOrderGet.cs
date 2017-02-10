using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spOrderGet : MyStoredProc
  {
    public spOrderGet(DbConnection conn)
      : base(strProcName: "spOrderGet", conn: conn)
    {
    }

    public DBOrder[] Execute(int intSiteID, int[] intKeys)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter
      {
        ParameterName = "@Keys",
        SqlDbType = SqlDbType.Structured,
        Value = new IDTable(intKeys)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        var dbOrders = reader.GetResults<DBOrder>();
        var dbOrderItems = reader.GetResults<DBOrderItem>();

        foreach (var dbOrder in dbOrders)
        {
          dbOrder.Items = (from i in dbOrderItems where i.OrderID == dbOrder.OrderID select i).ToArray();
        }
        return dbOrders;
      }
    }
  }
}



