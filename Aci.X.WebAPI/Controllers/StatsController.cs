using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;

using Aci.X.IwsLib;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.ClientLib;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;
using Aci.X.ServerLib;
using Solishine.CommonLib;
using System.Data.SqlClient;

namespace Aci.X.WebAPI
{
  [RoutePrefix("stats")]
  public class StatsController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    [POST("update"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _POST_stats_update([FromBody] Cli.ApiStats stats)
    {
      string strStatsXml = Solishine.CommonLib.XmlHelper.Serialize(stats, System.Text.UnicodeEncoding.Unicode);
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spStatsUpdate(conn).Execute(strStatsXml);
      }
        return HttpStatusOK();
    }
  }
}
