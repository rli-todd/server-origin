using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;

using Aci.X.IwsLib;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;

namespace Aci.X.WebAPI
{
  /// <summary>
  /// Block Controller
  /// </summary>
  [RoutePrefix("block")]
  public class BlockController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Returns all currently defined blocks
    /// </summary>
    /// <param name="block_code"></param>
    /// <param name="where"></param>
    /// <returns></returns>
    [GET("all"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Block[]>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_block_all()
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBBlock[] dbBlocks = new DB.spBlockGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID);
        return HttpStatusOK<Block[]>(Business.Block.ToClient(dbBlocks));
      }
    }

    /// <summary>
    /// Creates a new content block
    /// </summary>
    /// <returns></returns>
    [POST("new"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_block_new([FromBody] Block block)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        int intRetVal = new DB.spBlockCreate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          strBlockName: block.BlockName,
          strBlockType: block.BlockType,
          isEnabled: block.IsEnabled);
        return HttpStatusCreated(null, intRetVal.ToString());
      }
    }

    /// <summary>
    /// Updates the passed block
    /// </summary>
    /// <param name="block_id"></param>
    /// <returns></returns>
    [POST("{block_id:int}"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_block_X(int block_id, [FromBody] Block block)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spBlockUpdate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intBlockID: block_id,
          strBlockName: block.BlockName,
          strBlockType: block.BlockType,
          isEnabled: block.IsEnabled);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns the specified content block
    /// </summary>
    /// <param name="block_id"></param>
    /// <returns></returns>
    [GET("{block_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Block>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_block_X(int block_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBBlock[] dbBlocks = new DB.spBlockGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID);
        return HttpStatusOK<Block>(Business.Block.ToClient(dbBlocks)[0]);
      }
    }

    /// <summary>
    /// Deletes the specified content block
    /// </summary>
    /// <param name="block_id"></param>
    /// <returns></returns>
    [DELETE("{block_id:int}"), HttpDelete]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _DELETE_block_X(int block_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spBlockDelete(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intBlockID: block_id);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns all content blocks associated with the specified page
    /// </summary>
    /// <param name="block_id"></param>
    /// <returns></returns>
    [GET("for_page/{page_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<ContentBlock[]>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_block_for_page_X(int page_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBBlock[] dbBlocks = new DB.spBlockGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intPageID: page_id);
        return HttpStatusOK<Block[]>(Business.Block.ToClient(dbBlocks));
      }
    }

    [GET("{block_id:int}/add_page/{page_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _GET_block_X_add_page_X(int block_id, int page_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spBlockAddPage(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intBlockID: block_id, 
          intPageID: page_id);
        return HttpStatusOK();
      }
    }

    [GET("{block_id:int}/remove_from_page/{page_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _GET_block_X_remove_page_X(int block_id, int page_id)
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


