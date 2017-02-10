using System;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;
using Aci.X.ClientLib;
using Aci.X.ClientLib.Exceptions;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;

namespace Aci.X.WebAPI
{
  [RoutePrefix("report")]
  public class ReportController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Pings the report controller
    /// </summary>
    /// <returns></returns>
    [GET("ping"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_report_ping()
    {
      return HttpStatusOK();
    }

    /// <summary>
    /// Searches for reports in the user's report history with optional filters
    /// </summary>
    /// <param name="query_id"></param>
    /// <param name="profile_id"></param>
    /// <returns></returns>
    [GET("search?{firstname?}&{lastname?}&{state?}&{profile_id?}&{order_id:int?}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Report[]>))]
    public HttpResponseMessage _GET_report_search(
      string firstname = null,
      string lastname = null,
      string state = null,
      string profile_id = null,
      int? order_id = null)
    {
      return HttpStatusOK<Cli.Report[]>(
        Business.Report.Search(
          context: CallContext,
          strFirstName: firstname,
          strLastName: lastname,
          strState: state,
          strProfileID: profile_id,
          intOrderID: order_id));
    }

    /// <summary>
    /// Returns the specified report.
    /// </summary>
    /// <param name="report_id"></param>
    /// <returns></returns>
    [GET("{report_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Report>))]
    public HttpResponseMessage _GET_report_X(int report_id)
    {
      var intAccessibleReportIds = Business.Report.ValidateReportAccess(CallContext, new int[] { report_id });
      var reports = Business.Report.Get(
          context: CallContext,
          keys: intAccessibleReportIds);
      if (reports.Length == 0)
        throw new ReportNotFoundException();

      return HttpStatusOK<Cli.Report>(reports[0]);
    }

    /// <summary>
    /// Returns the reports specified by the list of integer report IDs in the request body.
    /// </summary>
    /// <returns></returns>
    [POST("list"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.Report[]>))]
    public HttpResponseMessage _POST_report_list([FromBody] int[] report_ids)
    {
      var intAccessibleReportIDs = Business.Report.ValidateReportAccess(CallContext, report_ids);
      var reports = Business.Report.Get(
          context: CallContext,
          keys: intAccessibleReportIDs);
      return HttpStatusOK<Cli.Report[]>(reports);
    }

    /// <summary>
    /// Creates and returns a new report based on the user's most recent
    /// query (/search/preview), and optionally for a specific profile in 
    /// the most recent query's results.
    /// </summary>
    /// <param name="report_id"></param>
    /// <returns></returns>
    [GET("new?{order_id:int}&{profile_id}&{query_id:int?}&{type_code?}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Report>))]
    public HttpResponseMessage _GET_report_new(int order_id, string profile_id, int? query_id=null, string type_code=null)
    {
      var report = Business.Report.Create(
        context: CallContext,
        strTypeCode: type_code,
        intOrderID: order_id,
        strProfileID: profile_id,
        intQueryID: query_id??0);
      return HttpStatusOK<Cli.Report>(report);
    }


  }
}
