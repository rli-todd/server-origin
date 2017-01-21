using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;
using System.Configuration;

namespace Aci.X.IwsLib
{
  public class IwsConfig : MyConfig 
  {
    public static int PreviewConsumerMaxConcurrentOps
    {
      get { return GetIntSetting("MessageQueue.IwsPreviewConsumer.MaxConcurrentOps", 1); }
    }

    public static int PreviewResultsLimit
    {
      get { return GetIntSetting("Iws.Apis.Preview.ResultsLimit", 100); }
    }

    public static int PreviewDelayMsecs
    {
      get { return GetIntSetting("Iws.Apis.Preview.DelayMsecs", 0); }
    }

    public static string AuthProxyApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.AuthProxy.BaseUrl", "https://iauth.intelius.com/authproxy-1.0.13/"); }
    }

    public static string AuthProxyApiVersion
    {
      get { return GetSetting("Iws.Apis.AuthProxy.Version", "4.0"); }
    }

    public static string PaymentProxyApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.PaymentProxy.BaseUrl", "http://integ-jweb11.tuk2.intelius.com:8080/paymentproxy-0.0.2/"); }
    }

    public static string PaymentProxyApiVersion
    {
      get { return GetSetting("Iws.Apis.PaymentProxy.Version", "1.0"); }
    }

    public static string StorefrontClientID
    {
      get { return GetSetting("Iws.Apis.Storefront.ClientID", ""); }
    }

    public static string StorefrontRefererID
    {
      get { return GetSetting("Iws.Apis.Storefront.RefererID", "14927");  }
    }

    public static string StorefrontSharedSecret
    {
      get { return GetSetting("Iws.Apis.Storefront.SharedSecret", ""); }
    }

    public static string PreviewApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Preview.BaseUrl", "https://iws.intelius.com/private/3.0/"); }
    }
    public static string PreviewApiKey
    {
      get { return GetSetting("Iws.Apis.Preview.Key", "c8qhdz8n39s42ebyy4we45rj"); }
    }
    
    public static string FetchApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Fetch.BaseUrl", "https://intelius.api.mashery.com/core/3.0/"); }
    }

    public static string CommerceClientID
    {
      get { return GetSetting("Iws.Apis.Commerce.ClientID", ""); }
    }

    public static string CommerceAuthID
    {
      get { return GetSetting("Iws.Apis.Commerce.AuthID", ""); }
    }

    public static string CommerceAuthKey
    {
      get { return GetSetting("Iws.Apis.Commerce.AuthKey", ""); }
    }

    public static string CatalogApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Catalog.BaseUrl", "https://iiwscommerce.intelius.com/commerce/2.1/catalog.php"); }
    }

    public static string CustomerApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Customer.BaseUrl", "https://iiwscommerce.intelius.com/commerce/2.1/customer.php"); }
    }

    public static string SubscriptionApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Subscription.BaseUrl", "https://iiwscommerce.intelius.com/commerce/2.1/subscription.php"); }
    }

    public static string TransactionApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Transaction.BaseUrl", "https://iiwscommerce.intelius.com/commerce/2.1/transaction.php"); }
    }

    public static string FulfillmentApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Fulfillment.BaseUrl", "https://iiwscommerce.intelius.com/commerce/2.1/fulfill.php"); }
    }



    public static string FetchApiKey
    {
      get { return GetSetting("Iws.Apis.Fetch.Key", "2md8yhx7dwvxqt8d5n74q2dj"); }
    }

    public static string IwsCacheRootFolder
    {
      get { return GetSetting("Iws.Cache.RootFolder", @"D:\Data\IwsCache\"); }
    }

    public static int IwsDatabaseTimeoutSecs
    {
      get { return GetIntSetting("Iws.Database.Timeout.Secs", 30); }
    }

    public static int IwsQueryTimeoutSecs
    {
      get { return GetIntSetting("Iws.Query.Timeout.Secs", 30); }
    }

    public static int IwsQueryProfileTimeoutSecs
    {
      get { return GetIntSetting("Iws.Query.ProfileTimeout.Secs", 300); }
    }

    public static string IwsCacheFilePath(int intQueryID, bool boolCheckFolderExists = true)
    {
      string strFolder = String.Format(@"{0}\{1:x2}\{2:x2}\{3:x2}",
        IwsCacheRootFolder,
        (intQueryID >> 24) & 0xff,
        (intQueryID >> 16) & 0xff,
        (intQueryID >> 8) & 0xff);
      if (boolCheckFolderExists && !Directory.Exists(strFolder))
      {
        Directory.CreateDirectory(strFolder);
      }
      return string.Format(@"{0}\{1:x8}_{1}.json.gzip", strFolder, intQueryID);
    }

    public static bool IwsCacheFileExists(int intQueryID)
    {
      return File.Exists(IwsCacheFilePath(intQueryID, boolCheckFolderExists: false));
    }

    public static string ProfileApiBaseUrl
    {
      get { return GetSetting("Iws.Apis.Profile.BaseUrl", "http://iws.intelius.com/private/3.0/"); }
    }

    public static string ProfileApiKey
    {
      get { return GetSetting("Iws.Apis.Profile.Key", "c8qhdz8n39s42ebyy4we45rj"); }
    }

    public static string ProfileConnectionString
    {
      get { return ConfigurationManager.ConnectionStrings["Profile"].ConnectionString; }
    }

    public static string PublicRecordsConnectionString
    {
      get { return ConfigurationManager.ConnectionStrings["PublicRecords"].ConnectionString; }
    }

    public static string SolishineGeoConnectionString
    {
      get { return ConfigurationManager.ConnectionStrings["SolishineGeo"].ConnectionString; }
    }

  }
}
