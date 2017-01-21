using System;
using System.Data.SqlClient;
using Aci.X.ServerLib;

namespace Aci.X.Database
{
  public class DalSqlDb : IDisposable
  {
    protected SqlConnection _conn;

    public SqlTransaction Transaction;
    public int CommandTimeoutSecs;

    public DalSqlDb(SqlConnection conn = null, bool boolUseTransaction = false, int intCommandTimeoutSecs = 0)
    {
      if (conn == null)
        conn = WebServiceConfig.WebServiceSqlConnection;
      _conn = conn;
      if (boolUseTransaction)
        Transaction = conn.BeginTransaction();
      CommandTimeoutSecs = intCommandTimeoutSecs;
    }

    public SqlConnection Connection
    {
      get { return _conn; }
    }

    public void Dispose()
    {
      if (_conn != null)
      {
        _conn.Dispose();
        _conn = null;
      }
    }
  }
}
