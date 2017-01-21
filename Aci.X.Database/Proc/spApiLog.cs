using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spApiLog : MyStoredProc
  {
    public spApiLog(DbConnection conn)
      : base(strProcName: "spApiLog", conn: conn)
    {
    }

    public void Execute(
      int intSiteID,
      int intVisitID,
      int intUserID,
      int intClientIP,
      int intUserIP,
      string strRequestMethod,
      string strRequestBody,
      string strResponseJson,
      short shHttpStatusCode,
      int intDurationMsecs,
      string strServerHostname,
      string strPathAndQuery,
      string strRequestTemplate,
      string strUserAgent,
      string strErrorType,
      string strErrorMessage,

      ref short shServerID,
      ref int intApiLogPathID,
      ref short shApiLogTemplateID,
      ref int intUserAgentID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@ClientIP", intClientIP);
      Parameters.AddWithValue("@UserIP", intUserID);
      Parameters.AddWithValue("@RequestMethod", strRequestMethod);
      Parameters.AddWithValue("@RequestBody", strRequestBody);
      Parameters.AddWithValue("@ResponseJson", strResponseJson);
      Parameters.AddWithValue("@HttpStatusCode", shHttpStatusCode);
      Parameters.AddWithValue("@DurationMsecs", intDurationMsecs);
      Parameters.AddWithValue("@ServerName", strServerHostname);
      Parameters.AddWithValue("@PathAndQuery", strPathAndQuery);
      Parameters.AddWithValue("@RequestTemplate", strRequestTemplate);
      Parameters.AddWithValue("@UserAgent", strUserAgent);
      Parameters.AddWithValue("@ErrorType", strErrorType);
      Parameters.AddWithValue("@ErrorMessage", strErrorMessage);

      Parameters.Add(new SqlParameter("@ServerID", SqlDbType.SmallInt)
      {
        Direction = ParameterDirection.InputOutput,
        Value = shServerID
      });
      Parameters.Add(new SqlParameter("@ApiLogPathID", SqlDbType.Int)
      {
        Direction = ParameterDirection.InputOutput,
        Value = intApiLogPathID
      });
      Parameters.Add(new SqlParameter("@APiLogTemplateID", SqlDbType.SmallInt)
      {
        Direction = ParameterDirection.InputOutput,
        Value = shApiLogTemplateID
      });
      Parameters.Add(new SqlParameter("@UserAgentID", SqlDbType.Int)
      {
        Direction = ParameterDirection.InputOutput,
        Value=intUserAgentID
      });

      ExecuteNonQuery();
      shServerID = (short) Parameters["@ServerID"].Value;
      intApiLogPathID = (int) Parameters["@ApiLogPathID"].Value;
      shApiLogTemplateID = (short) Parameters["@ApiLogTemplateID"].Value;
      intUserAgentID = (int) Parameters["@UserAgentID"].Value;
    }
  }
}



