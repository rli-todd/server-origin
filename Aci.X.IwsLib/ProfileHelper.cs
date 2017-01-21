using System;
using System.IO;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Data.SqlClient;
using Newtonsoft.Json;
using NLog;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.ServerLib;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.IwsLib
{
  /// <summary>
  /// Exists primarily to query profile previews from the local data store,
  /// and if they do not exist, query from IWS and cache them for subsequent queries. 
  /// In a transition period, this class may be called directly from the web client.
  /// Eventually, this will be accessed only by the web service.
  /// </summary>
  public class ProfileHelper
  {
    private static CacheClient Cache;
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    public static ConcurrentDictionary<string, int> _dictFirstNameID = new ConcurrentDictionary<string, int>();
    public static ConcurrentDictionary<string, int> _dictLastNameID = new ConcurrentDictionary<string, int>();

    static ProfileHelper()
    {
        Cache = new CacheClient(strRegion: "Preview", intTimeToLiveMins: WebServiceConfig.PreviewCacheTtlMins);
    }

    private static DBSearchResults[] ExecuteQuery(ProfileDB db, int intQueryID)
    {
      return db.spSearchResultsGet(
        shSearchType: null,
        strFirstName: null,
        strLastName: null,
        intQueryID: intQueryID);
    }
    private static DBSearchResults[] ExecuteQuery(ProfileDB db, PersonQuery query)
    {
      return db.spSearchResultsGet(
        shSearchType: (short)query.SearchType,
        strFirstName: query.FirstName,
        strMiddleName: query.MiddleName,
        strLastName: query.LastName,
        strState: query.State,
        intVisitID: query.VisitID,
        intFirstNameID: query.FirstNameID,
        intLastNameID: query.LastNameID,
        intTimeoutSecs: IwsConfig.IwsDatabaseTimeoutSecs);
    }
    
    private static DBSearchResults[] FetchSearchResults(PersonQuery query)
    {
      DBSearchResults[] dbResults = null;
      var connectionString = WebServiceConfig.SecondaryToPrimaryProfileConnectionString;
      var conn = new SqlConnection(connectionString);
      try
      {
        conn.Open();
      }
      catch (Exception ex)
      {
        _logger.Error("Using primary connection string: {0} at {1}", ex.Message, ex.StackTrace);
        connectionString = WebServiceConfig.ProfileConnectionString;
        conn = WebServiceConfig.ProfileSqlConnection ;
      }
      using (var db = new ProfileDB(conn))
      {
        dbResults = ExecuteQuery(db, query);
      }

      if (connectionString != WebServiceConfig.ProfileConnectionString && (dbResults == null || dbResults.Length==0))
      {
        using (var db = new ProfileDB(WebServiceConfig.ProfileSqlConnection))
        {
          dbResults = ExecuteQuery(db,query);
        }
      }
      return dbResults;
    }

    public static ProfileResponse GetPreviews(PersonQuery query, int intRequeryAfterMonths = 2)
    {
      string strJson = null;
      int intFullNameHits = 0;
      ProfileResponse response = null;
      byte[] tCompressedResults = null;
      int intQueryID;

      DateTime dtStart;
      bool boolResultsAreEmpty = false;

      query.Normalize(); // convert names to mixed case, and state to upper case

      /*
       * Normalize the case for firstname, lastname, and state
       */

      int intValue;
      if (_dictFirstNameID.TryGetValue(query.FirstName, out intValue))
      {
        query.FirstNameID = intValue;
      }

      if (_dictLastNameID.TryGetValue(query.LastName, out intValue))
      {
        query.LastNameID = intValue;
      }

      ProductIdEnum product = SearchType.ToProductID(query.SearchType);

      DBSearchResults[] dbResults = FetchSearchResults(query);

      if (dbResults.Length > 0 && (dbResults[0].RobotID != 0 || dbResults[0].DateCached > DateTime.Now.AddMonths(-intRequeryAfterMonths)))
      {
        intFullNameHits = dbResults[0].FullNameHits;
        if (query.FirstNameID==null)
        {
          _dictFirstNameID[query.FirstName] = dbResults[0].FirstNameID;
        }
        if (query.LastNameID == null)
        {
          _dictLastNameID[query.LastName] = dbResults[0].LastNameID;
        }
        intQueryID = dbResults[0].QueryID;
        tCompressedResults = dbResults[0].CompressedResults;
        if (tCompressedResults != null)
        {
          strJson = CompressionHelper.UncompressString(tCompressedResults);
          /*
           * This next step should eventually be unnecessary.   We are 
           * "cleaning" up previously stored JSON data.
           */
          string strConvertedJson = ProfileHelper.ConvertTextJson(strJson);
          response = JsonConvert.DeserializeObject<ProfileResponse>(strConvertedJson);
          // Eliminate "default" values, and other stuff we don't want (Upsell Links)
          strConvertedJson = JsonConvert.SerializeObject(response);
          try
          {
            if (strConvertedJson.Length != strJson.Length)
            {
              strJson = strConvertedJson;
              response.FullNameHits = dbResults[0].FullNameHits;
              response.QueryID = dbResults[0].QueryID;
              intQueryID = SaveJsonResults(null, query, product, "iws3.0+", boolResultsAreEmpty, response, strJson, out intFullNameHits);
              if (intQueryID == 0)
              {
                // Name was rejected in the DB (i.e. aaaa,bbbb,cccc, etc)
                strJson = null;
                response = null;
              }
            }
            else
            {
              response.FullNameHits = intFullNameHits;
              response.QueryID = dbResults[0].QueryID;
            }
          }
          catch (Exception)
          {
            response = null;
          }
        }
      }
      if (response == null)
      {
        LimitQueryRate(query);
        dtStart = DateTime.Now;
        response = ProfileClient.QueryPreviews(out strJson, query);
        // Eliminate "default" values, and other stuff we don't want (Upsell Links)
        strJson = JsonConvert.SerializeObject(response);
        boolResultsAreEmpty = response == null || response.ProfileCount == 0;
        intQueryID = SaveJsonResults(dtStart, query, product, "iws3.0+", boolResultsAreEmpty, response, strJson, out intFullNameHits);
        if (response != null)
        {
          response.FullNameHits = intFullNameHits;
          response.QueryID = intQueryID;
        }
        if (intQueryID == 0)
        {
          // Name was "rejected" in the DB (i.e. aaaa bbbb asdf, etc.)
          response = null;
        }
      }
      return response;
    }

    public static bool ProfilesExist(PersonQuery query)
    {
      using (var db = new ProfileDB())
      {
        return db.spSearchResultsExists(  
          shSearchType: (short)query.SearchType,
          strFirstName: query.FirstName,
          strMiddleName: query.MiddleName,
          strLastName: query.LastName,
          strState: query.State);
      }
    }

    public static string GetPreviewsXml(PersonQuery query, out int intProfileCount, out int intFullNameHits)
    {
      intProfileCount = 0;
      intFullNameHits = 0;
      ProfileResponse response = GetPreviews(query);
      if (response == null)
      {
        return null;
      }
      intFullNameHits = response.FullNameHits;
      intProfileCount = response.ProfileCount;
      return response.ToLegacyXML(query);
    }

    public static ProfileResponse GetProfile(string strProfileID, string strProfileAttributes, string strState=null)
    {
      ProfileResponse profileRet = null;
      string strJson = null;

      using (var db = new ProfileDB())
      {
        var dbProfile = db.spProfileGet(strProfileID, strProfileAttributes);
        if (dbProfile != null)
        {
          strJson = CompressionHelper.UncompressString(dbProfile.CompressedJson);
        }
      }
      if (strJson == null)
      {
        DateTime dtStart = DateTime.Now;
        ProfileClient.QueryProfiles(out strJson, strProfileID, strProfileAttributes);
        if (strJson != null)
        {
          var intDurationMsecs = (int) DateTime.Now.Subtract(dtStart).TotalMilliseconds;
          var compressedJson = CompressionHelper.CompressString(strJson);
          using (var db = new ProfileDB())
          {
            db.spProfileSave(
              strProfileID: strProfileID, 
              strProfileAttributes: strProfileAttributes, 
              tCompressedJson: compressedJson,
              intDurationMsecs: intDurationMsecs);
          }
        }
      }
      if (strJson != null)
      {
        profileRet = Aci.X.ClientLib.JsonObjectSerializer.Deserialize<ProfileResponse>(strJson);
      }
      return profileRet;
    }


    private static int SaveJsonResults(
      DateTime? dtStart,
      PersonQuery query,
      ProductIdEnum product,
      string strApiSource,
      bool boolResultsAreEmpty,
      ProfileResponse response,
      string strJson,
      out int intFullNameHits)
    {
      int? intIwsQueryDurationMsecs = null;
      intFullNameHits = 0;
      int intQueryID=0;
      if (dtStart != null)
        intIwsQueryDurationMsecs = (int)DateTime.Now.Subtract(dtStart.Value).TotalMilliseconds;
      using (var db = new ProfileDB())
      {
        if (boolResultsAreEmpty)
        {
          intQueryID = db.spSearchResultsSave(
            shSearchType: ((short) query.SearchType),
            strFirstName: query.FirstName,
            strMiddleName: query.MiddleName,
            strLastName: query.LastName,
            strState: query.State,
            intVisitID: query.VisitID,
            intNumResults: 0,
            intQueryDurationMsecs: intIwsQueryDurationMsecs,
            strApiSource: strApiSource,
            boolResultsAreEmpty: true,
            tCompressedResults: null,
            intFileSize: 0,
            boolMinimized: true,
            intFullNameHits: out intFullNameHits);
        }
        else
        {
          if (response != null && response.ProfileCount > 0)
          {
            byte[] tCompressedJson = CompressionHelper.CompressString(strJson);
            intQueryID = db.spSearchResultsSave(
              shSearchType: ((short) query.SearchType),
              strFirstName: query.FirstName,
              strMiddleName: query.MiddleName,
              strLastName: query.LastName,
              strState: query.State,
              intVisitID: query.VisitID,
              intNumResults: response.ProfileCount,
              intQueryDurationMsecs: intIwsQueryDurationMsecs,
              strApiSource: strApiSource,
              boolResultsAreEmpty: false,
              tCompressedResults: tCompressedJson,
              intFileSize: tCompressedJson.Length,
              boolMinimized: true,
              intFullNameHits: out intFullNameHits);

#if WRITE_TO_FS
          WriteToFileSystem(intQueryID, tCompressedJson);
          new spSearchResultsSetFileSize(conn).Execute(intQueryID, tCompressedJson.Length);
#endif

          }
        }
      }
      if (!boolResultsAreEmpty && intQueryID != 0 && WebServiceConfig.QueueToPreviewConsumer)
      {
        Aci.X.IwsLib.IwsPreviewConsumerQueue.Singleton.Queue(intQueryID);
      }

      return intQueryID;
    }

    public static void WriteToFileSystem( int intQueryID, byte[] tCompressedData )
    {
      using (FileStream fs = new FileStream(IwsConfig.IwsCacheFilePath(intQueryID), FileMode.Create, FileAccess.Write))
      {
        fs.Write(tCompressedData, 0, tCompressedData.Length);
        fs.Close();
      }
    }

    public static byte[] ReadFromFileSystem(int intQueryID)
    {
      string strPath = IwsConfig.IwsCacheFilePath(intQueryID);
      if (!File.Exists(strPath))
      {
        return null;
      }
      using (FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read))
      {
        using (MemoryStream ms = new MemoryStream())
        {
          fs.CopyTo(ms);
          return ms.GetBuffer();
        }
      }
    }

    private static ConcurrentDictionary<string, DateTime?> _dictLastQueries = new ConcurrentDictionary<string, DateTime?>();
    private static void LimitQueryRate( PersonQuery query)
    {
      DateTime? dtLastQuery = null;
      int intMsecsBetweenQueries = WebServiceConfig.MinMsecsBetweenQueries;
      if (query.ClientIP != null)
      {
        string strCacheKey = query.ClientIP;
        if (query.RobotID != 0)
        {
          strCacheKey = query.RobotID.ToString();
          intMsecsBetweenQueries = WebServiceConfig.MinMsecsBetweenRobotQueries(query.RobotID);
        }
        dtLastQuery = Cache.Get<DateTime?>(strCacheKey);
        //_dictLastQueries.TryGetValue(strCacheKey, out dtLastQuery);
        double dMsecsSinceLastQuery = DateTime.Now.Subtract(dtLastQuery ?? DateTime.MinValue).TotalMilliseconds;
        Cache.Put(strCacheKey, DateTime.Now);
        //_dictLastQueries[strCacheKey] = DateTime.Now;

        if (dMsecsSinceLastQuery < intMsecsBetweenQueries)
        {
          if (WebServiceConfig.LimitQueryRateWith503)
          {
            throw new IwsException(System.Net.HttpStatusCode.ServiceUnavailable, "Query rate exceeded; server too busy");
          }
          int intDelayMsecs = intMsecsBetweenQueries - (int)dMsecsSinceLastQuery;
          _logger.Info("Limiting IP={0}, RobotID={1} by {2} msecs", strCacheKey, query.RobotID, intDelayMsecs);
          System.Threading.Thread.Sleep(intDelayMsecs);
        }
      }
    }

    public static string ConvertTextJson(string strInput)
    {
      // Get rid of the verbose _t objects
      //const string REGEX = @"{\s*""_t""\s*:\s*([^}]*)}";
      const string REGEX = @"{\s*""_t""\s*:\s*((""[^""]*"")|([^}]*))\s*}";
      return System.Text.RegularExpressions.Regex.Replace(strInput, REGEX, "$1");

    }


    public static void DeleteCachedProfile(string strProfileID)
    {
      using (var db = new ProfileDB())
      {
        db.spProfileDelete(strProfileID);
      }
    }
  }
}
