﻿using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spOrderCreate : MyStoredProc
  {
    public spOrderCreate(DbConnection conn)
      : base(strProcName: "spOrderCreate", conn: conn)
    {
    }

    public int Execute(
      int intSiteID, 
      int intUserID,
      string strOrderXml, 
      int? intQueryID=null, 
      string strProfileID=null,
      bool boolIsStorefrontXml=true,
      bool boolIsMultipleOrders=false)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@OrderXml", strOrderXml);
      Parameters.AddWithValue("@QueryID", intQueryID);
      Parameters.AddWithValue("@ProfileID", strProfileID);
      Parameters.AddWithValue("@IsStorefrontXml", boolIsStorefrontXml);
      Parameters.AddWithValue("@IsMultipleOrders", boolIsMultipleOrders);
      Parameters.Add(new SqlParameter("@OrderID", SqlDbType.Int)
      {
        Direction = ParameterDirection.ReturnValue
      });
      ExecuteNonQuery();
      return (int) Parameters["@OrderID"].Value;
    }
  }
}



