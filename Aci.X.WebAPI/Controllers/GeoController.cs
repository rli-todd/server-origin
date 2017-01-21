using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;

using Aci.X.IwsLib;
using Aci.X.Database;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.ClientLib;
using Aci.X.ServerLib;
using Cli = Aci.X.ClientLib;

namespace Aci.X.WebAPI
{
  [RoutePrefix("geo")]
  public class GeoController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Returns an array of all US States
    /// </summary>
    /// <returns></returns>
    [GET("states"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GeoState[]>))]
    public HttpResponseMessage _GET_geo_states()
    {
      using (SqlConnection conn = WebServiceConfig.GeoSqlConnection)
      {
        spGeoStateGet sp = new spGeoStateGet(conn);
        Cli.GeoState[] retVal = sp.Execute();
        return HttpStatusOK<Cli.GeoState[]>(retVal);
      }
    }

    /// <summary>
    /// Returns an array of all cities in the specified state.
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GeoCity[]>))]
    public HttpResponseMessage _GET_geo_states_X_cities(int state_fips)
    {
      using (SqlConnection conn = WebServiceConfig.GeoSqlConnection)
      {
        spGeoCityGet sp = new spGeoCityGet(conn);
        Cli.GeoCity[] retVal = sp.Execute(
          bStateFips: (byte)state_fips);
        return HttpStatusOK<Cli.GeoCity[]>(retVal);
      }
    }

    /// <summary>
    /// Returns an array of all counties in the specified state.
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/counties"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GeoCounty[]>))]
    public HttpResponseMessage _GET_geo_states_X_counties(int state_fips)
    {
      using (SqlConnection conn = WebServiceConfig.GeoSqlConnection)
      {
        spGeoCountyGet sp = new spGeoCountyGet(conn);
        Cli.GeoCounty[] retVal = sp.Execute(
          bStateFips: (byte)state_fips);
        return HttpStatusOK<Cli.GeoCounty[]>(retVal);
      }
    }

    /// <summary>
    /// Returns an array of all cities in the specified state and county
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="county_fips">County FIPS code</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/counties/{county_fips:int}/cities"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GeoCity[]>))]
    public HttpResponseMessage _GET_geo_states_X_counties_X_cities(int state_fips, int county_fips)
    {
      using (SqlConnection conn = WebServiceConfig.GeoSqlConnection)
      {
        spGeoCityGet sp = new spGeoCityGet(conn);
        Cli.GeoCity[] retVal = sp.Execute(
          bStateFips: (byte)state_fips,
          shCountyFips: (short)county_fips);
        return HttpStatusOK<Cli.GeoCity[]>(retVal);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="city_fips">City FIPS code</param>
    /// <param name="template">Template name</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities/{city_fips:int}/content/{template}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GpdNode>))]
    public HttpResponseMessage _GET_geo_states_X_cities_X_content_X(int state_fips, int city_fips, string template)
    {
      using (SqlConnection conn = WebServiceConfig.ProfileSqlConnection)
      {
        spGpdNodeGet sp = new spGpdNodeGet(conn);
        Cli.GpdNode retVal = sp.Execute(
          bStateFips: (byte)state_fips,
          intCityFips: city_fips,
          strLastName: "",
          strFirstName: null);
        return HttpStatusOK<Cli.GpdNode>(retVal);
      }
    }

    /// <summary>
    /// Returns a "top-level" GpdNode (GeoPersonDirectory node) for the given city.
    /// This node will contain the Top 100 last names in the city, with the NextLetters
    /// attribute containing the first letters of all last names not included in the Top 100.
    /// Recipient traverse down the directory from the Top 100 last names, use the 
    /// <code>geo/states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname}/firstname</code> API.
    /// Recipient traverse down from the NextLetters, use the 
    /// <code>geo/states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname_starts_with}</code> API.
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="city_fips">City FIPS code</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities/{city_fips:int}/lastname"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GpdNode>))]
    public HttpResponseMessage _GET_geo_states_X_cities_X_lastname(int state_fips, int city_fips)
    {
      using (SqlConnection conn = WebServiceConfig.ProfileSqlConnection)
      {
        spGpdNodeGet sp = new spGpdNodeGet(conn);
        Cli.GpdNode retVal = sp.Execute(
          bStateFips: (byte)state_fips,
          intCityFips: city_fips,
          strLastName: "",
          strFirstName: null);
        return HttpStatusOK<Cli.GpdNode>(retVal);
      }
    }

    /// <summary>
    /// Returns a GpdNode (GeoPersonDirectory node) for the given city and lastnames
    /// that start with the <code>{lastname_starts_with}</code> parameter.  The returned node will contain the 
    /// Top 100 last names in the city that start with <code>{lastname_starts_with}</code>, and the NextLetters
    /// will contain  the next letters of all the last names not included in the Top 100.
    /// Recipient traverse down the directory from the Top 100 last names, use the 
    /// <code>geo/states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname}/firstname</code> API.
    /// Recipient traverse down from the NextLetters, use this same API, adding a single "next letter"
    /// to the <code>{lastname_starts_with}</code> string.
    /// 
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="city_fips">City FIPS code</param>
    /// <param name="lastname_starts_with">Starting characters of last name (may be passed as empty string)</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname_starts_with}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GpdNode>))]
    public HttpResponseMessage _GET_geo_states_X_cities_X_lastname_X(int state_fips, int city_fips, string lastname_starts_with)
    {
      using (SqlConnection conn = WebServiceConfig.ProfileSqlConnection)
      {
        spGpdNodeGet sp = new spGpdNodeGet(conn);
        Cli.GpdNode retVal = sp.Execute(
          bStateFips: (byte)state_fips,
          intCityFips: city_fips,
          strLastName: lastname_starts_with??"",
          strFirstName: null);
        return HttpStatusOK<Cli.GpdNode>(retVal);
      }
    }

    /// <summary>
    /// Returns a "top-level" GpdNode (GeoPersonDirectory node) for the given city and specific last name.
    /// This node will contain the Top 100 first names in the city with the specified last name, with the NextLetters
    /// attribute containing the first letters of all first names not included in the Top 100.
    /// Recipient traverse down the directory from the Top 100 first names, use the <code>search/preview</code> API.
    /// Recipient traverse down from the NextLetters, use the 
    /// <code>geo/states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname}/firstname/{firstname_starts_with}</code> API.
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="city_fips">City FIPS code</param>
    /// <param name="lastname">Full last name</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname}/firstname"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GpdNode>))]
    public HttpResponseMessage _GET_geo_states_X_cities_X_lastname_X_firstname(int state_fips, int city_fips, string lastname)
    {
      using (SqlConnection conn = WebServiceConfig.ProfileSqlConnection)
      {
        spGpdNodeGet sp = new spGpdNodeGet(conn);
        Cli.GpdNode retVal = sp.Execute(
          bStateFips: (byte)state_fips,
          intCityFips: city_fips,
          strLastName: lastname,
          strFirstName: "");
        return HttpStatusOK<Cli.GpdNode>(retVal);
      }
    }

    /// <summary>
    /// Returns a "top-level" GpdNode (GeoPersonDirectory node) for the given city, specific last name, 
    /// and first names that start with <code>{firstname_starts_with}</code>.
    /// This node will contain the Top 100 first names in the city with the specified last name and starting
    /// first name characters.  The NextLetters
    /// attribute will containh the first letters of all first names not included in the Top 100, given the above criteria.
    /// Recipient traverse down the directory from the Top 100 first names, use the <code>search/preview API</code>.
    /// Recipient traverse down from the NextLetters, use this same API, adding a single "next letter"
    /// to the <code>{firstname_starts_with}</code> string.
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="city_fips">City FIPS code</param>
    /// <param name="lastname">Full last name</param>
    /// <param name="firstname_starts_with">Starting characters of first name (may be passed as empty string)</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname}/firstname/{firstname_starts_with}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.GpdNode>))]
    public HttpResponseMessage _GET_geo_states_X_cities_X_lastname_X_firstname_X(int state_fips, int city_fips, string lastname, string firstname_starts_with)
    {
      using (SqlConnection conn = WebServiceConfig.ProfileSqlConnection)
      {
        spGpdNodeGet sp = new spGpdNodeGet(conn);
        Cli.GpdNode retVal = sp.Execute(
          bStateFips: (byte)state_fips,
          intCityFips: city_fips,
          strLastName: lastname,
          strFirstName: firstname_starts_with);
        return HttpStatusOK<Cli.GpdNode>(retVal);
      }
    }

    /// <summary>
    /// Returns the 
    /// </summary>
    /// <param name="state_fips">State FIPS code</param>
    /// <param name="city_fips">City FIPS code</param>
    /// <param name="lastname">Full last name</param>
    /// <returns></returns>
    [GET("states/{state_fips:int}/cities/{city_fips:int}/profiles/{lastname}/{firstname}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<ProfileResponse>))]
    public HttpResponseMessage _GET_geo_states_X_cities_X_lastname_X_firstname(int state_fips, int city_fips, string lastname, string firstname)
    {
      Cli.PersonQuery query = new Cli.PersonQuery
      {
        FirstName = firstname,
        MiddleName = null,
        LastName = lastname,
        City = null,
        State = null
      };

      int intProfileCount;
      int intFullNameHits;
      string strRetVal = ProfileHelper.GetPreviewsXml(query, out intProfileCount, out intFullNameHits);
      return HttpStatusOK<string>(strRetVal);
    }

  }
}
