using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using Aci.X.Business.Cache;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;

using Aci.X.IwsLib;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.ClientLib;
using Aci.X.ClientLib.Exceptions;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;

namespace Aci.X.WebAPI
{
  /// <summary>
  /// Page Controller
  /// </summary>
  [RoutePrefix("page")]
  public class PageController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Returns the content for the given page
    /// </summary>
    /// <returns></returns>
    [GET("{page_code}/content?{where?}", ControllerPrecedence = 1), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<ContentBlock[]>))]
    public HttpResponseMessage _GET_page_X_content(string page_code, string where=null)
    {
      return HttpStatusOK<ContentBlock[]>(Business.Page.GetContent(
        context: CallContext,
        strPageCode: page_code,
        strWhere: where,
        keyValuePairs: Request.GetQueryNameValuePairs()));
    }

    /// <summary>
    /// Returns all currently defined pages
    /// </summary>
    /// <param name="page_code"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    [GET("all", ControllerPrecedence=2), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Page[]>))]
    [Authorize(Roles="BackofficeReader")]
    public HttpResponseMessage _GET_page_all()
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBPage[] dbPages = new DB.spPageGet(conn).Execute(intAuthorizedUserID: CallContext.AuthorizedUserID);
        return HttpStatusOK<Page[]>(Business.Page.ToClient(dbPages));
      }
    }

    /// <summary>
    /// Creates a new page
    /// </summary>
    /// <returns></returns>
    [POST("new"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_page_new([FromBody] Page page)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        int intRetVal = new DB.spPageCreate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          strPageCode: page.PageCode,
          strDescription: page.Description);
        return HttpStatusCreated(null, intRetVal.ToString());
      }
    }

    /// <summary>
    /// Updates the passed page
    /// </summary>
    /// <param name="page_id"></param>
    /// <returns></returns>
    [POST("{page_id:int}"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_page_X(int page_id, [FromBody] Page page)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spPageUpdate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intPageID: page_id,
          strPageCode: page.PageCode,
          strDescription: page.Description);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns the specified page
    /// </summary>
    /// <param name="page_id"></param>
    /// <returns></returns>
    [GET("{page_id:int}", ControllerPrecedence = 2), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Page>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_page_X(int page_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBPage[] dbPages = new DB.spPageGet(conn).Execute(
          intPageID: page_id,
          intAuthorizedUserID: CallContext.AuthorizedUserID);
        return HttpStatusOK<Page>(Business.Page.ToClient(dbPages[0]));
      }
    }

    /// <summary>
    /// Deletes the specified page
    /// </summary>
    /// <param name="page_id"></param>
    /// <returns></returns>
    [DELETE("{page_id:int}"), HttpDelete]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _DELETE_page_X(int page_id)
    {
      Page retVal = null;
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spPageDelete(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intPageID: page_id);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Adds the specified block to the specified page
    /// </summary>
    /// <param name="block_id"></param>
    /// <param name="page_id"></param>
    /// <returns></returns>
    [GET("{page_id:int}/add_block/{block_id:int}", ControllerPrecedence = 2), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _GET_page_X_add_block_X(int block_id, int page_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spBlockAddPage(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intPageID: page_id, 
          intBlockID: block_id);
        return HttpStatusOK();
      }
    }

    /// <summary>
    ///  Removes the specfied block from the specified page
    /// </summary>
    /// <param name="block_id"></param>
    /// <param name="page_id"></param>
    /// <returns></returns>
    [GET("{page_id:int}/remove_block/{block_id:int}", ControllerPrecedence = 2), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _GET_page_X_remove_block_X(int block_id, int page_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spBlockRemovePage(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intBlockID: block_id, 
          intPageID: page_id);
        return HttpStatusOK();
      }
    }

  }
}


