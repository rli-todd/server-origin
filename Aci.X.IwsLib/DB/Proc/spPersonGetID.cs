using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spPersonGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spPersonGetID(DbConnection conn)
      : base("spPersonGetID", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@ProfileID", System.Data.SqlDbType.VarChar);
      Parameters.Add("@FirstNameID", System.Data.SqlDbType.Int);
      Parameters.Add("@MiddleNameID", System.Data.SqlDbType.Int);
      Parameters.Add("@LastNameID", System.Data.SqlDbType.Int);
      Parameters.Add("@DateOfBirth", System.Data.SqlDbType.Date);
      Parameters.Add("@PhoneCount", System.Data.SqlDbType.TinyInt);
      Parameters.Add("@EmailCount", System.Data.SqlDbType.TinyInt);
      Parameters.Add("@ListSchoolIDs", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ListRelativeIDs", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ListCompanyIDs", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ListGeoLocationIDs", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ListAliasIDs", System.Data.SqlDbType.VarChar);
    }

    public int Execute(
      string strProfileID,
      int? intFirstNameID=null,
      int? intMiddleNameID=null,
      int? intLastNameID=null,
      DateTime? dtBirth=null,
      int? intPhoneCount=null,
      int? intEmailCount=null,
      string strListSchoolIDs=null,
      string strListRelativeIDs=null,
      string strListCompanyIDs=null,
      string strListGeoLocationIDs=null,
      string strListAliasIDs=null
      )
    {
      Parameters["@ProfileID"].Value = strProfileID;
      Parameters["@FirstNameID"].Value = intFirstNameID;
      Parameters["@MiddleNameID"].Value = intMiddleNameID;
      Parameters["@LastNameID"].Value = intLastNameID;
      Parameters["@DateOfBirth"].Value = dtBirth;
      Parameters["@PhoneCount"].Value = intPhoneCount;
      Parameters["@EmailCount"].Value = intEmailCount;
      Parameters["@ListSchoolIDs"].Value = strListSchoolIDs;
      Parameters["@ListCompanyIDs"].Value = strListCompanyIDs;
      Parameters["@ListRelativeIDs"].Value = strListRelativeIDs;
      Parameters["@ListGeoLocationIDs"].Value = strListGeoLocationIDs;
      Parameters["@ListAliasIDs"].Value = strListAliasIDs;

      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
