using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;

using Solishine.CommonLib;

namespace Aci.X.ServerLib
{
  public class WebServiceConfig : MyConfig
  {
    public static bool LogApiCalls
    {
      get { return GetBoolSetting("LogApiCalls", false); }
    }

    /*
     * This setting is to enable the Intelius Payment Proxy to work correctly
     * in a development environment where the client browser and the web server app
     * are behind the same firewall, and thus, the REMOTE-ADDR presented by the client
     * to the server is a local, non-routable address.
     * Setting this value to true uses the client's public facing IP address rather  
     * than its local address, so when the browser connects to the payment proxy, 
     * the IP addresses will match.
     */
    public static bool UseServerPublicIpAddressForUserIp
    {
      get
      {
        return GetBoolSetting("Debug.UseServerPublicIpAddressForUserIp", false);
      }
    }

    public static string GeoConnectionString
    {
      get
      {
        return ConfigurationManager.ConnectionStrings["Geo"].ConnectionString;
      }
    }

    public static string AnalyticsConnectionString
    {
      get
      {
        return ConfigurationManager.ConnectionStrings["Analytics"].ConnectionString;
      }
    }

    public static string ReadOnlyProfileConnectionString
    {
      get
      {
        string strRetVal = null;
        var connectionString = ConfigurationManager.ConnectionStrings["ReadOnlyProfile"];
        if (connectionString != null)
        {
          strRetVal = connectionString.ConnectionString;
        }
        return strRetVal ?? ProfileConnectionString;
      }
    }

    public static string ProfileConnectionString
    {
      get
      {
        return ConfigurationManager.ConnectionStrings["Profile"].ConnectionString;
      }
    }

    public static string WebServiceConnectionString
    {
      get 
      {
        return ConfigurationManager.ConnectionStrings["WebServiceDB"].ConnectionString;
      }
    }

    public static SqlConnection GeoSqlConnection
    {
      get
      {
        SqlConnection conn = new SqlConnection(GeoConnectionString);
        conn.Open();
        return conn;
      }
    }

    public static SqlConnection AnalyticsSqlConnection
    {
      get
      {
        SqlConnection conn = new SqlConnection(AnalyticsConnectionString);
        conn.Open();
        return conn;
      }
    }

    public static SqlConnection ReadOnlyProfileSqlConnection
    {
      get
      {
        SqlConnection conn = new SqlConnection(ReadOnlyProfileConnectionString);
        conn.Open();
        return conn;
      }
    }

    public static string SecondaryToPrimaryProfileConnectionString
    {
      get
      {
        return (DateTime.Now.Minute >= 15 && DateTime.Now.Minute < 59)
          ? ReadOnlyProfileConnectionString
          : ProfileConnectionString;
      }
    }

    public static SqlConnection ProfileSqlConnection
    {
      get
      {
        SqlConnection conn = new SqlConnection(ProfileConnectionString);
        conn.Open();
        return conn;
      }
    }

    public static SqlConnection WebServiceSqlConnection
    {
			get
			{
				SqlConnection conn = new SqlConnection(WebServiceConnectionString);
				conn.Open();
				return conn;
			}
		}

    public static bool CacheEnabled
    {
      get
      {
        return GetBoolSetting("CacheEnabled", false);
      }
    }

    public static string MandrillApiKey
    {
      get { return GetSetting("Mandrill.ApiKey", "wgsP7SfR1G_Lsix86c6S9g"); }
    }

    public static string MaxMindGeoIpDatabasePath
    {
      get { return GetSetting("MaxMind.GeoIp.DatabasePath", ""); }
    }

    public static string MaxMindGeoIpPermittedCountryIsoCodes
    {
      get { return GetSetting("MaxMind.GeoIp.PermittedCountryIsoCodes", "US"); }
    }

    public static int MaxMindGeoRejectedLoggingIntervalSecs
    {
      get { return GetIntSetting("MaxMind.GeoIp.RejectedLoggingIntervalSecs", 60); }
    }

    public static bool UseAppFabric
    {
      get
      {
        return GetBoolSetting("UseAppFabric", false);
      }
    }

    public static bool PreviewCacheEnabled
    {
      get { return GetBoolSetting("PreviewCache.Enabled", true); }
    }

    public static int PreviewCacheTtlMins
    {
      get { return GetIntSetting("PreviewCache.TTL.Mins", 1440); }
    }
    #region Redis

    public static string RedisCacheHostname
    {
      get
      {
        return GetSetting("RedisCacheHostname", "localhost");
      }
    }

    public static int RedisCachePort
    {
      get
      {
        return GetIntSetting("RedisCachePort", 6379);
      }
    }

    #endregion // Redis
    
    public static bool QueueToPreviewConsumer
    {
      get
      {
        return GetBoolSetting("QueueToPreviewConsumer", true);
      }
    }

    public static int MinMsecsBetweenQueries
    {
      get { return MyConfig.GetIntSetting("IwsMinMsecsBetweenQueries", 2000); }
    }

    public static int MinMsecsBetweenRobotQueries(int intRobotID)
    {
      {
        return MyConfig.GetIntSetting("IwsMinMsecsBetweenQueries.RobotID." + intRobotID.ToString(), 2000);
      }
    }

    public static bool LimitQueryRateWith503
    {
      get  
      {
          return MyConfig.GetBoolSetting("IwsLimitQueryRateWith503", false);
      }
    }

    public static bool LogApiResponse
    {
      get { return MyConfig.GetBoolSetting("LogApiResponse", false); }
    }

    public static bool LogApiResponseSize
    {
      get { return MyConfig.GetBoolSetting("LogApiResponseSize", false); }
    }

    public static int VisitCacheTimeoutMins
    {
      get { return MyConfig.GetIntSetting("VisitCache.TimeoutMins", 30); }
    }
  }
}
