using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spReportCreate : MyStoredProc
  {
    public spReportCreate(DbConnection conn)
      : base(strProcName: "spReportCreate", conn: conn)
    {
    }

    public int Execute(
      int intSiteID, 
      int intUserID,
      int intOrderID,
      int? intQueryID,
      string strProfileID,
      string strState,
      string strReportTypeCode,
      int intJsonLen,
      byte[] tCompressedJson,
      int intHtmlLen,
      byte[] tCompressedHtml,
      int intReportCreationMsecs)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@OrderID", intOrderID);
      Parameters.AddWithValue("@QueryID", intQueryID);
      Parameters.AddWithValue("@ProfileID", strProfileID);
      Parameters.AddWithValue("@State", strState);
      Parameters.AddWithValue("@ReportTypeCode", strReportTypeCode);
      Parameters.AddWithValue("@JsonLen", intJsonLen);
      Parameters.AddWithValue("@CompressedJson", tCompressedJson);
      Parameters.AddWithValue("@HtmlLen", intHtmlLen);
      Parameters.AddWithValue("@CompressedHtml", tCompressedHtml);
      Parameters.AddWithValue("@ReportCreationMsecs", intReportCreationMsecs);
      Parameters.Add(new SqlParameter("@ReportID", SqlDbType.Int)
      {
        Direction = ParameterDirection.ReturnValue
      });
      ExecuteNonQuery();
      return (int)Parameters["@ReportID"].Value;
    }
  }
}



