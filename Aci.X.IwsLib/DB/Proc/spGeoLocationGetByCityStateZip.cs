using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spGeoLocationGetByCityStateZip : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter( "@ReturnVal", System.Data.SqlDbType.Int );

    public spGeoLocationGetByCityStateZip(DbConnection conn)
      : base("spGeoLocationGetByCityStateZip", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@CityName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@StateCode", System.Data.SqlDbType.VarChar);
      Parameters.Add("@Zip", System.Data.SqlDbType.VarChar);
    }

    public int Execute(string strCityName, string strStateCode, string strZip)
    {
      Parameters["@CityName"].Value = strCityName;
      Parameters["@StateCode"].Value = strStateCode;
      Parameters["@Zip"].Value = strZip;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
