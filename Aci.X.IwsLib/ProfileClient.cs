using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Aci.X.ClientLib;


using Aci.X.ClientLib.ProfileTypes;
using NLog;

namespace Aci.X.IwsLib
{
  public partial class ProfileClient
  {
    public static ProfileResponse QueryPreviews(out string strJson, PersonQuery query, int intRetryCount = 0)
    {
      strJson = null;
      if (IwsConfig.PreviewDelayMsecs > 0)
      {
        System.Threading.Thread.Sleep(IwsConfig.PreviewDelayMsecs);
      }

      String strPath = String.Format(
        "premiumpreviews?api_key={0}&firstname={1}&middlename={2}&lastname={3}&state={4}&city={5}&limit={6}",
        IwsConfig.PreviewApiKey,
        UrlEncode(query.FirstName),
        UrlEncode(query.MiddleName),
        UrlEncode(query.LastName),
        UrlEncode(query.State),
        UrlEncode(query.City),
        IwsConfig.PreviewResultsLimit);

      ProfileResponse retVal = null;
      try
      {
        retVal = Query<ProfileResponse>(out strJson, IwsConfig.PreviewApiBaseUrl, strPath);
      }
      catch (Exception ex)
      {
        if (ex.InnerException != null && ex.InnerException is TaskCanceledException)
        {
          throw;
        }
        if (ex.Message.StartsWith("Forbidden") && intRetryCount < 5)
        {
          ++intRetryCount;
          Thread.Sleep(1000 * intRetryCount);
          retVal = QueryPreviews(out strJson, query, intRetryCount);
        }
      }
      return retVal;
    }

    public static ProfileResponse QueryProfiles(out string strJson, string[] astrProfiles, string strDataFilterElements = "All")
    {
      return QueryProfiles(out strJson, String.Join(",", astrProfiles), strDataFilterElements);
    }

    public static ProfileResponse QueryProfiles(
      out string strJson, 
      string strProfiles, 
      string strDataFilterElements = "All")
    {
      string strPath = String.Format(
        "premiumprofiles?api_key={0}&profileids={1}&datafilter_type=include&datafilter_dataelements={2}",
        IwsConfig.ProfileApiKey,
        strProfiles,
        strDataFilterElements);

      return Query<ProfileResponse>(out strJson, IwsConfig.ProfileApiBaseUrl, strPath, IwsConfig.IwsQueryProfileTimeoutSecs);
    }

    public static ProfileResponse QueryFetch(out string strJson, string[] astrProfiles)
    {
      return QueryFetch(out strJson, String.Join(",", astrProfiles));
    }

    public static ProfileResponse QueryFetch(out string strJson, string strProfiles, int intTimeoutSecs=0)
    {
      string strPath = String.Format(
        "fetch?format=json&api_key={0}&profileids={1}",
        IwsConfig.ProfileApiKey,
        strProfiles);

      return Query<ProfileResponse>(out strJson, IwsConfig.FetchApiBaseUrl, strPath, intTimeoutSecs);
    }

    private static TYPE Query<TYPE>(out string strJson, string strBaseUrl, string strPath, int intTimeoutSecs=0)
    {
      strJson = Query(strBaseUrl, strPath, intTimeoutSecs);
      //return JsonConvert.DeserializeObject<TYPE>(strJson);
      return (TYPE)JsonObjectSerializer.Deserialize(typeof(TYPE), strJson);
    }

    private static string Query(string strBaseUrl, string strPath, int intTimeoutSecs = 0)
    {
      if (intTimeoutSecs == 0)
      {
        intTimeoutSecs = IwsConfig.IwsQueryTimeoutSecs;
      }
      //_logger.LogEvent(LogLevel.Info, "Calling {0}", strPath);
      HttpClient cli = new HttpClient
      {
        BaseAddress = new Uri(strBaseUrl),
        Timeout = new TimeSpan(hours: 0, minutes: 0, seconds: intTimeoutSecs)
      };

      cli.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

      HttpResponseMessage resp = cli.GetAsync(strPath).Result;

      if (resp.IsSuccessStatusCode)
      {
        string strResp = UTF8Encoding.UTF8.GetString(resp.Content.ReadAsByteArrayAsync().Result);
        return ProfileHelper.ConvertTextJson(strResp);
      }
      else
      {
        throw new IwsException(resp.StatusCode, resp.ReasonPhrase);
      }
    }

    private static string UrlEncode(string strInput)
    {
      return strInput == null
        ? ""
        : System.Net.WebUtility.UrlEncode(strInput);
    }



  }
}
