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
  [RoutePrefix("section")]
  public class SectionController : ControllerBase
  {
    /// <summary>
    /// Creates a new content section
    /// </summary>
    /// <returns></returns>
    [POST("new"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_section_new([FromBody] Section section)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        int intRetVal = new DB.spSectionCreate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intBlockID: section.BlockID,
          strSectionName: section.SectionName,
          strSectionType: section.SectionType,
          isEnabled: section.IsEnabled);
        return HttpStatusCreated(null, intRetVal.ToString());
      }
    }

    /// <summary>
    /// Updates the passed section
    /// </summary>
    /// <param name="section_id"></param>
    /// <returns></returns>
    [POST("{section_id:int}"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_section_X(int section_id, [FromBody] Section section)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spSectionUpdate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intSectionID: section.SectionID,
          strSectionName: section.SectionName,
          strSectionType: section.SectionType,
          isEnabled: section.IsEnabled);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns the specified content section
    /// </summary>
    /// <param name="section_id"></param>
    /// <returns></returns>
    [GET("{section_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Section>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_section_X(int section_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBSection[] dbSections = new DB.spSectionGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intSectionID: section_id);
        return HttpStatusOK<Section>(Business.Section.ToClient(dbSections)[0]);
      }
    }

    /// <summary>
    /// Deletes the specified content section
    /// </summary>
    /// <param name="section_id"></param>
    /// <returns></returns>
    [DELETE("{section_id:int}"), HttpDelete]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _DELETE_section_X(int section_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spSectionDelete(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intSectionID: section_id);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns all content sections associated with the specified block
    /// </summary>
    /// <param name="section_id"></param>
    /// <returns></returns>
    [GET("for_block/{block_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Section[]>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_section_for_block_X(int block_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBSection[] dbSections = new DB.spSectionGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intBlockID: block_id);
        return HttpStatusOK<Section[]>(Business.Section.ToClient(dbSections));
      }
    }
  }
}