using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cli=Aci.X.ClientLib;

namespace Aci.X.DirectoryWalker
{
  class Program
  {
    const string CLIENT_SECRET = "3DF07A4B-9C67-4EC8-9C2F-7760AF23361C";
    const int TIMER_INTERVAL_MSECS = 10000;
    const int MAX_PARALLEL_CALLS = 200;
    private int _intCallsCompleted = 0;
    private int _intCallsStarted = 0;
    private long _lGetNodeMsecs = 0;
    private long _lLastCheckpointMsecs = 0;
    private Timer _timer;
    private DateTime _dtStart = DateTime.Now;


    static void Main(string[] args)
    {
      try
      {
        Program prog = new Program();
        prog.WalkUSA();
      }
      catch (Exception ex)
      {
        while (ex != null)
        {
          Console.Error.WriteLine("{0} at {1}", ex.Message, ex.StackTrace);
          ex = ex.InnerException;
        }
      }
    }

    public Program()
    {
      _timer = new Timer(new TimerCallback(OnTimer), null, TIMER_INTERVAL_MSECS, TIMER_INTERVAL_MSECS);
    }

    private void OnTimer(object o)
    {
      long lMsecs = _lGetNodeMsecs;
      int intAvg = _intCallsCompleted == 0 ? 0 : (int)lMsecs / _intCallsCompleted;
      Console.WriteLine("Elapsed Secs: {0}, Calls: {1}, Active: {2}, PerSec: {3:#.#}, AvgMsecs: {4}, ThreadMultiple: {5:#.#}",
        (int)DateTime.Now.Subtract(_dtStart).TotalSeconds,
        _intCallsCompleted,
        _intCallsStarted - _intCallsCompleted,
        _intCallsCompleted/DateTime.Now.Subtract(_dtStart).TotalSeconds,
        intAvg,
        ((Double)(lMsecs - _lLastCheckpointMsecs)) / TIMER_INTERVAL_MSECS);
      _lLastCheckpointMsecs = lMsecs;
      //
    }

    private void WalkUSA()
    {
      Cli.WebServiceClient client = new Cli.WebServiceClient(CLIENT_SECRET,"127.0.0.1");
      string strUrl = "geo/states";
      Cli.GeoState[] states = client.Get<Cli.GeoState[]>(strUrl);
      client.AssertHttpOK();
      List<Thread> listThreads = new List<Thread>();
      foreach (Cli.GeoState state in states)
      {
        ParameterizedThreadStart pts = new ParameterizedThreadStart(DoWalkState);
        Thread t = new Thread(pts);
        listThreads.Add(t);
        t.Start(state);
      }
      //await Task.WhenAll(listTasks);
      foreach (Thread t in listThreads)
      {
        t.Join();
      }
    }

    private void DoWalkState(Object o)
    {
      WalkState((Cli.GeoState)o).Wait();
    }

    private async Task WalkState(Cli.GeoState state)
    {
      Cli.WebServiceClient client = new Cli.WebServiceClient(CLIENT_SECRET, "127.0.0.1");
      string strUrl = String.Format("geo/states/{0}/cities", state.StateFips);
      Cli.GeoCity[] cities = await client.GetAsync<Cli.GeoCity[]>(strUrl);
      client.AssertHttpOK();
      foreach (Cli.GeoCity city in cities)
      {
        Console.WriteLine("{0,20}, {1,-20} started", state.StateName, city.CityName);
        await WalkCity(city);
        Console.WriteLine("{0,20}, {1,-20} done", state.StateName, city.CityName);
      }
    }

    private async Task WalkCity(Cli.GeoCity city, string strLastName = "", string strFirstName = null)
    {
      // Query the current node
      Cli.GpdNode nodeCity = await GetNode(
        intStateFips: city.StateFips,
        intCityFips: city.CityFips,
        strLastName: strLastName,
        strFirstName: strFirstName);

      if (strFirstName == null)
      {
        // We haven't zeroed in on a last name yet.
        // Walk all the available next letters
        for (int idxNextLetter = 0; idxNextLetter < nodeCity.NextLetters.Length; ++idxNextLetter)
        {
          await WalkCity(
            city: city,
            strLastName: strLastName + nodeCity.NextLetters.Substring(idxNextLetter, 1),
            strFirstName: null); // Null firstname indicates that we're still trying to home in on the last name
        }

        // walk the top 100 last names in this city starting with strLastName
        foreach (string strTop100Name in nodeCity.Top100)
        {
          // A non-null firstname indicates that the last name is a complete last name, not a "startswith"
          await WalkCity(city: city, strLastName: strTop100Name, strFirstName: "");
        }
      }
      else
      {
        // We have a last name.  Now we're trying to zero in on the first name
        // The Top 100 at this level are all "full names", and so they would be going off 
        // to the SRP.   For our purposes here, nothing to do.  We're done.
        // All we need to do is walk the next letters.
        for (int idxNextLetter = 0; idxNextLetter < nodeCity.NextLetters.Length; ++idxNextLetter)
        {
          await WalkCity(
            city: city,
            strLastName: strLastName,
            strFirstName: strFirstName + nodeCity.NextLetters.Substring(idxNextLetter, 1));
        }
      }
    }

    private async Task<Cli.GpdNode> GetNode(int intStateFips, int intCityFips, string strLastName = "", string strFirstName = null)
    {
      //while (_intCallsStarted - _intCallsCompleted >= MAX_PARALLEL_CALLS)
      //  await Task.Delay(100);
      Interlocked.Increment(ref _intCallsStarted);
      Stopwatch sw = new Stopwatch();
      sw.Start();
      Cli.WebServiceClient client = new Cli.WebServiceClient(CLIENT_SECRET, "127.0.0.1");
      string strUrl = String.Format("geo/states/{0}/cities/{1}/lastname/{2}",
        intStateFips, intCityFips, strLastName);
      if (strFirstName != null)
      {
        strUrl += String.Format("/firstname/{0}", strFirstName);
      }
      Cli.GpdNode node = await client.GetAsync<Cli.GpdNode>(strUrl);
      client.AssertHttpOK();
      sw.Stop();
      Interlocked.Add(ref _lGetNodeMsecs, sw.ElapsedMilliseconds);
      Interlocked.Increment(ref _intCallsCompleted);
      return node;
      
    }

  }
}
