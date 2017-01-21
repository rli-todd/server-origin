using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aci.X.IwsLib;
using Solishine.X.ServerLib;
using Aci.X.Database;
using System.Data.SqlClient;

namespace IwsCacheMigrator2
{
  class Program
  {
    int _intNumQueried;
    int _intNumWritten;
    int _intNextQueryID;
    int _intNumSkipped;
    int _intNumThreads;
    string _strConnection = "Data Source=.; Database=PR; Application Name=PreviewConsumerUtil; Integrated Security=SSPI;";
    DateTime _dtStart = DateTime.Now;

    static void Main(string[] args)
    {
      Program program = new Program();
      int intReportingIntervalMins = 5;
      int intQueryID = 1;
      int intMaxThreads = 10;

      if (args.Length>0)
        Int32.TryParse(args[0], out intReportingIntervalMins );
      if (args.Length>1)
        Int32.TryParse(args[1], out intQueryID );
      if (args.Length>2)
        Int32.TryParse(args[2], out intMaxThreads);

      program.Start( intReportingIntervalMins, intQueryID, intMaxThreads );
    }

    private void Start( int intReportingIntervalMins, int intQueryID, int intMaxThreads )
    {
      _intNextQueryID = intQueryID;
      int intLastReportedMinute=-1;
      
      Console.WriteLine("Reporting interval: {0}", intReportingIntervalMins);
      while (true)
      {
        while (_intNumThreads >= intMaxThreads)
          Thread.Sleep(10);
        Interlocked.Increment(ref _intNumThreads);
        ThreadPool.QueueUserWorkItem( new WaitCallback( ThreadProc ), _intNextQueryID++ );
        DateTime dtNow = DateTime.Now;
        if (dtNow.Minute % intReportingIntervalMins == 0 && dtNow.Minute != intLastReportedMinute)
        {
          intLastReportedMinute = dtNow.Minute;
          Report();
        }
      }
    }

    public void ThreadProc(object oState)
    {
      int intQueryID = (int)oState;
      Interlocked.Increment(ref _intNumQueried );
      if (!IwsConfig.IwsCacheFileExists(intQueryID))
      {
        using (SqlConnection conn = new SqlConnection(_strConnection))
        {
          spIwsCacheGet spGet = new spIwsCacheGet(conn);
          IwsCacheInfo info = spGet.Execute(intQueryID);
          if (info == null || info.CompressedResults == null)
          {
            Interlocked.Increment(ref _intNumSkipped);
          }
          else
          {
            ProfileHelper.WriteToFileSystem(info.QueryID, info.CompressedResults);
            Interlocked.Increment(ref _intNumWritten);

          }
        }
      }
      Interlocked.Decrement(ref _intNumThreads);
    }

    private void Report()
    {
      DateTime dtNow = DateTime.Now;
      using (SqlConnection conn = new SqlConnection(_strConnection))
      {
        Double dNumRemaining = (Double)(new spIwsCacheCountUnmigrated(conn).Execute(_intNextQueryID));
        Double dRowsPerSec = ((Double)_intNumQueried) / DateTime.Now.Subtract(_dtStart).TotalSeconds;
        Double dRowsPerHour = dRowsPerSec * 3600;
        DateTime dtProjected = dtNow.AddSeconds(dNumRemaining / (dRowsPerSec));
        Console.WriteLine("\n{0:HH:mm}: {1}w/{2}q/{3}s: {4:0.000}M @ {5:0,0}/s--> {6:MMM dd HH:mm}, QID={7}",
          dtNow,
          _intNumWritten,
          _intNumQueried,
          _intNumSkipped,
          dNumRemaining / 1000000,
          dRowsPerSec,
          dtProjected,
          _intNextQueryID);
      }
    }
  }
}
