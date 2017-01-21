using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aci.X.IwsLib;
using Solishine.X.ServerLib;
using System.Data.SqlClient;

namespace IwsCacheMigrator
{
  class Program
  {
    static void Main(string[] args)
    {
      int intReportingIntervalMins = args.Length == 0 ? 1 : Convert.ToInt32(args[0]);
      int? intModArgument = null;
      int? intModResult = null;
      if (args.Length >= 3)
      {
        intModArgument = Convert.ToInt32(args[1]);
        intModResult = Convert.ToInt32(args[2]);
      }
      string strConnection = IwsConfig.PublicRecordsConnectionString;
      if (args.Length > 4)
      {
        strConnection = "Data Source=.; Database=PR; Application Name=PreviewConsumerUtil; Integrated Security=SSPI;";
      }
      Console.WriteLine("Reporting interval: {0}", intReportingIntervalMins);
      using (SqlConnection conn = new SqlConnection(strConnection))
      {
        Aci.X.Database.spSearchResultsSetFileSize spSetFileSize = new Aci.X.Database.spSearchResultsSetFileSize(conn);
        spIwsCacheGetNext spGetNext = new spIwsCacheGetNext(conn);
#if V1
        int? intPreviousQueryID = null;
#else
        int intPreviousQueryID = Convert.ToInt32(args[3]);
#endif
        IwsCacheInfo info = spGetNext.Execute(intModArgument: intModArgument, intModResult: intModResult, intPreviousQueryID: intPreviousQueryID);
        DateTime dtStart = DateTime.Now;
        int intNumWritten = 0;
        int intNumQueried = 0;
        int intLastReportedMinute = -1;
        while (info != null)
        {
          intNumQueried++;
          string strPath = IwsConfig.IwsCacheFilePath(info.QueryID);
          FileInfo fileInfo = new FileInfo(strPath);
          if (!fileInfo.Exists && info.CompressedResults != null)
          {
            ProfileHelper.WriteToFileSystem(info.QueryID, info.CompressedResults);
            intNumWritten++;
          }
#if V1
          try
          {
            spSetFileSize.Execute(
              intQueryID: info.QueryID,
              intCompressedSize: info.CompressedResults == null ? 0 : info.CompressedResults.Length,
              dtCached: info.DateCached,
              boolDeleteOnly: info.FileSize > 0);
            intNumProcessed++;
          }
          catch (Exception ex)
          {
            if (ex.Message.Contains("timeout"))
            {
              Console.Write("x");
            }
            else
              throw;
          }
#else
          intPreviousQueryID = info.QueryID;
#endif
          DateTime dtNow = DateTime.Now;
          if (dtNow.Minute % intReportingIntervalMins == 0 && dtNow.Minute != intLastReportedMinute)
          {
            intLastReportedMinute = dtNow.Minute;
            Double dNumRemaining = (Double) (new spIwsCacheCountUnmigrated(conn).Execute(intPreviousQueryID));
            Double dRowsPerSec = ((Double)intNumQueried) / DateTime.Now.Subtract(dtStart).TotalSeconds;
            Double dRowsPerHour = dRowsPerSec * 3600;
            DateTime dtProjected = dtNow.AddSeconds(dNumRemaining / (dRowsPerSec * intModArgument??1));
            Console.WriteLine("\n{0:HH:mm}: {1}/{2}: {3:0.000}M @ {4:0,0}/h --> {5:MMM dd HH:mm}, QID={6}",
              dtNow,
              intNumWritten,
              intNumQueried,
              dNumRemaining/1000000,
              dRowsPerHour,
              dtProjected,
              info.QueryID);
          }
          // Get the next one
          info = spGetNext.Execute(intModArgument: intModArgument, intModResult: intModResult, intPreviousQueryID: intPreviousQueryID);
        }


      }
    }
  }
}
