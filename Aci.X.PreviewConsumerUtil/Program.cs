using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Aci.X.IwsLib;
using Aci.X.IwsLib.DB;
using Aci.X.IwsLib.DB.Cache;

namespace Aci.X.PreviewConsumerUtil
{
  class Program
  {
    static void Main(string[] args)
    {
      int intNumPreviews = 5;
      int intTotalPreviews = 0;
      int intReportInterval = 10;
      int intMaxConcurrentOps = 200;
      if (args.Length > 0)
      {
        Int32.TryParse(args[0], out intNumPreviews);
      }
      if (args.Length > 1)
      {
        Int32.TryParse(args[1], out intReportInterval);
      }
      if (args.Length > 2)
      {
        Int32.TryParse(args[2], out intMaxConcurrentOps);
      }
      DateTime dtStart = DateTime.Now;
      DateTime dtLastReport = dtStart;
      DBID[] ids;
      while (true)
      {
        Log("");
        Log("***********************************************************");
        Log("Starting block of {0} queries", intNumPreviews);
        try
        {
          var allCaches = CacheBase.AllCaches;
          using (SqlConnection conn = new SqlConnection(IwsConfig.ProfileConnectionString))
          {
            spSearchResultsGetUnconsumed sp = new spSearchResultsGetUnconsumed(conn);
            ids = sp.Execute(intNumRows: intNumPreviews);
            for (int idx = 0; idx < ids.Length; ++idx)
            {
              IwsPreviewConsumerQueue.Singleton.Queue(ids[idx].ID);
            }
          }
          while (IwsPreviewConsumerQueue.Singleton.OpsPending > 0)
          {
            Report(intTotalPreviews + ids.Length - (int)IwsPreviewConsumerQueue.Singleton.OpsPending, DateTime.Now.Subtract(dtStart).TotalMilliseconds);
            System.Threading.Thread.Sleep(intReportInterval * 1000);
          }
          Report(intTotalPreviews + ids.Length, DateTime.Now.Subtract(dtStart).TotalMilliseconds);
          intTotalPreviews += ids.Length;
        }
        catch (Exception ex)
        {
          var s = ex.Message;
        }
      }
    }

    private static void Report(int intConsumed, double dMsecs)
    {
      Log("");
      Log("_______________________________________________________");
      Log("{0} consumed in {1:#0.0} secs ({2:#0.0}/second)", intConsumed, dMsecs/1000, (1000 * intConsumed) / (dMsecs==0?1:dMsecs));
      var caches = (from c in CacheBase.AllCaches where c.Hits > 0 select c).OrderByDescending(n => n.HitRatio);
      foreach (var cache in caches)
      {
        Log("{0,50} {1,8} of {2,-8}: hit ratio {3:#0.0}", cache.Name, cache.Hits, cache.Hits+cache.Misses, 100 * cache.HitRatio);
      }
    }

    static void Log(string strTemplate, params object[] oParams)
    {
      Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss}: {1}", DateTime.Now, String.Format(strTemplate, oParams));
    }

  }
}
