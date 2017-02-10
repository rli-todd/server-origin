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

namespace Aci.X.WebAPI
{
  [RoutePrefix("search")]
  public class SearchController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    [GET("preview?{firstname}&{lastname}&{state?}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<SearchResultsPage>))]
    public HttpResponseMessage _GET_search_preview(string firstname, string lastname, string state = null)
    {
      Cli.PersonQuery query = new Cli.PersonQuery
      {
        FirstName = firstname,
        MiddleName = String.Empty,
        LastName = lastname,
        State = state
      };

      ProfileResponse response = ProfileHelper.GetPreviews(query, 12);
      SearchResultsPage retVal = new SearchResultsPage() { QueryID = response.QueryID };

      if (response != null && response.Profiles != null && response.Profiles.Profile != null)
      {
        Business.Visit.SetStateAndQueryID(CallContext, state, response.QueryID);
        // Sort profiles by age
        response.Profiles.Profile =
          (
            from p in response.Profiles.Profile
            orderby (p.DateOfBirth ?? new DateOfBirth { Age = "999" }).Age
            select p
          ).ToArray();
        // Reduce multiple addresses in same city for a given profile to a single one
        foreach (Profile profile in response.Profiles.Profile)
        {
          if (profile.Addresses != null && profile.Addresses.Address != null)
          {
            List<Address> listCities = new List<Address>();
            Dictionary<string, string> dictCities = new Dictionary<string, string>();

            foreach (Address address in profile.Addresses.Address)
            {
              string strKey = address.City + "," + address.State;
              if (!dictCities.ContainsKey(strKey))
              {
                dictCities[strKey] = strKey;
                listCities.Add(new Address { State = address.State, City = address.City });
              }
            }
            profile.Addresses.Address = listCities.ToArray();
          }
        }
        retVal.Profiles = response.Profiles;
      }
      return HttpStatusOK<SearchResultsPage>(retVal);
    }

    /// <summary>
    /// Gets an individual profile id.
    /// </summary>
    /// <param name="profileid"></param>
    /// <param name="firstname"></param>
    /// <param name="lastname"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [GET("profile/{profileid}?{firstname}&{lastname}&{state?}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Profile>))]
    public HttpResponseMessage _GET_search_profile(string profileid, string firstname, string lastname, string state = null)
    {
      Cli.PersonQuery query = new Cli.PersonQuery
      {
        FirstName = firstname,
        MiddleName = String.Empty,
        LastName = lastname,
        State = state
      };

      ProfileResponse response = ProfileHelper.GetPreviews(query);

      Profile profileRet = null;
      if (response != null && response.Profiles != null)
      {
        profileRet = response.Profiles.Find(profileid);
      }
      return HttpStatusOK<Profile>(profileRet);
    }
  }
}
