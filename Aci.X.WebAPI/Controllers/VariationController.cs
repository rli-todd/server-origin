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
  [RoutePrefix("variation")]
  public class VariationController : ControllerBase
  {
    /// <summary>
    /// Creates a new content variation
    /// </summary>
    /// <returns></returns>
    [POST("new"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_variation_new([FromBody] Variation variation)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        int intRetVal = new DB.spVariationCreate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intSectionID: variation.SectionID,
          strDescription: variation.Description,
          strMultirowPrefix: variation.MultirowPrefix,
          strMultirowSuffix: variation.MultirowSuffix,
          strMultirowDelimiter: variation.MultirowDelimiter,
          strHeaderTemplate: variation.HeaderTemplate,
          strBodyTemplate: variation.BodyTemplate,
          strViewName: variation.ViewName,
          strViewFieldNames: variation.ViewFieldNames,
          isEnabled: variation.IsEnabled);
        return HttpStatusCreated(null, intRetVal.ToString());
      }
    }

    /// <summary>
    /// Updates the passed variation
    /// </summary>
    /// <param name="variation_id"></param>
    /// <returns></returns>
    [POST("{variation_id:int}"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _POST_variation_X(int variation_id, [FromBody] Variation variation)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spVariationUpdate(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intVariationID: variation_id,
          strDescription: variation.Description,
          strMultirowPrefix: variation.MultirowPrefix,
          strMultirowSuffix: variation.MultirowSuffix,
          strMultirowDelimiter: variation.MultirowDelimiter,
          strHeaderTemplate: variation.HeaderTemplate,
          strBodyTemplate: variation.BodyTemplate,
          strViewName: variation.ViewName,
          strViewFieldNames: variation.ViewFieldNames,
          isEnabled: variation.IsEnabled);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns the specified content variation
    /// </summary>
    /// <param name="variation_id"></param>
    /// <returns></returns>
    [GET("{variation_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Variation>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_variation_X(int variation_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBVariation[] dbVariations = new DB.spVariationGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intVariationID: variation_id);
        return HttpStatusOK<Variation>(Business.Variation.ToClient(dbVariations[0]));
      }
    }

    /// <summary>
    /// Deletes the specified content variation
    /// </summary>
    /// <param name="variation_id"></param>
    /// <returns></returns>
    [DELETE("{variation_id:int}"), HttpDelete]
    [ReturnValue(typeof(WebServiceResponse))]
    [Authorize(Roles = "BackofficeWriter")]
    public HttpResponseMessage _DELETE_variation_X(int variation_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        new DB.spVariationDelete(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intVariationID: variation_id);
        return HttpStatusOK();
      }
    }

    /// <summary>
    /// Returns all content variations associated with the specified section
    /// </summary>
    /// <param name="variation_id"></param>
    /// <returns></returns>
    [GET("for_section/{section_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Variation[]>))]
    [Authorize(Roles = "BackofficeReader")]
    public HttpResponseMessage _GET_variation_for_section_X(int section_id)
    {
      using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
      {
        DBVariation[] dbVariations = new DB.spVariationGet(conn).Execute(
          intAuthorizedUserID: CallContext.AuthorizedUserID,
          intSectionID: section_id);
        return HttpStatusOK<Variation[]>(Business.Variation.ToClient(dbVariations));
      }
    }

  }
}

