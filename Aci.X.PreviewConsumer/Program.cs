using System;
using System.ServiceProcess;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Aci.X.IwsLib;
using Aci.X.IwsLib.DB;
using Aci.X.IwsLib.DB.Cache;
using Aci.X.ClientLib.ProfileTypes;

using Newtonsoft.Json;


namespace Aci.X.PreviewConsumer
{
  class Program
  {
    static void Main(string[] args)
    {
      var service = new PreviewConsumerService();

      if (Environment.UserInteractive)
      {
        Type serviceType = service.GetType();
        MethodInfo onStartMethod = serviceType.GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo onStopMethod = serviceType.GetMethod("OnStop", BindingFlags.NonPublic | BindingFlags.Instance);

        onStartMethod.Invoke(service, new object[] { args });
        Console.WriteLine("Press any key to stop program");
        Console.Read();
        onStopMethod.Invoke(service, null);
      }
      else
      {
        ServiceBase.Run(service);
      }
    }

    private static void Report(int intAttempted, int intSucceeded, int intMsecs )
    {
      Log("");
      Log("_______________________________________________________");
      Log("{0}/{1} suceeded in {2} msecs ({3}/second)", intSucceeded, intAttempted, intMsecs, (1000 * intSucceeded) / intMsecs);
      foreach (CacheBase cache in CacheBase.AllCaches)
      {
        Log("{0,20} hit ratio: {1:#.#}", cache.Name, 100 * cache.HitRatio);
      }
    }

    static void Log(string strTemplate, params object[] oParams)
    {
      Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss}: {1}", DateTime.Now, String.Format(strTemplate, oParams));
    }

  }
}
