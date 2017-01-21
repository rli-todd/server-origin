using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Aci.X.ClientLib;
using StackExchange.Redis;

namespace Aci.X.ServerLib
{
  public class CacheMonitor
  {
    private static ConcurrentDictionary<string, CacheMonitor> _dictMonitorsByCachePrefix;
    private static string[] _statKeys;
    private static Timer _timer;


    public const string STATS_KEY = "Stats";
    private const int TIMER_INTERVAL_MSECS = 5000;

    private const string READ_HITS = "ReadHits";
    private const string READ_MISSES = "ReadMisses";
    private const string READ_HIT_RATIO = "ReadHitRatio";
    private const string READ_MSECS = "ReadMsecsTotal";
    private const string READ_MSECS_AVG = "ReadMsecsAvg";

    private const string DELETE_HITS = "DeleteHits";
    private const string DELETE_MISSES = "DeleteMisses";
    private const string DELETE_HIT_RATIO = "DeleteHitRatio";
    private const string DELETE_MSECS = "DeleteMsecsTotal";
    private const string DELETE_MSECS_AVG = "DeleteMsecsAvg";

    private const string WRITES = "Writes";
    private const string WRITE_MSECS = "WriteMsecsTotal";
    private const string WRITE_MSECS_AVG = "WriteMsecsAvg";

    private CacheClient _cacheClient;
    private ConcurrentDictionary<string, long> _dictStats;

    static CacheMonitor()
    {
      _dictMonitorsByCachePrefix = new ConcurrentDictionary<string, CacheMonitor>();
      _statKeys = new string[]
      {
        READ_HITS,
        READ_MISSES,
        READ_HIT_RATIO,
        READ_MSECS,
        READ_MSECS_AVG,

        DELETE_HITS,
        DELETE_MISSES,
        DELETE_HIT_RATIO,
        DELETE_MSECS,
        DELETE_MSECS_AVG,

        WRITES,
        WRITE_MSECS,
        WRITE_MSECS_AVG
      };
      _timer = new Timer(new TimerCallback(OnTimer), null, TIMER_INTERVAL_MSECS, TIMER_INTERVAL_MSECS);
    }

    public static CacheMonitor Init(CacheClient cacheClient)
    {
      CacheMonitor retVal = null;
      if (!_dictMonitorsByCachePrefix.TryGetValue(cacheClient.KeyPrefix, out retVal))
      {
        retVal = new CacheMonitor(cacheClient);
        _dictMonitorsByCachePrefix[cacheClient.KeyPrefix] = retVal;
      }
      return retVal;
    }

    public void Reset()
    {
      foreach (string strKey in _statKeys)
      {
        _dictStats[strKey] = 0;
        _cacheClient.RemoveHash(STATS_KEY, strKey);
      }
    }

    private CacheMonitor(CacheClient cacheClient)
    {
      _cacheClient = cacheClient;
      _dictStats = new ConcurrentDictionary<string, long>();
      foreach (string strKey in _statKeys)
      {
        _dictStats[strKey] = 0;
      }
    }

    public void CountRead(DateTime dtStart, int intNumHits, int intNumMisses)
    {
      if (intNumHits > 0)
      {
        _dictStats[READ_HITS] += intNumHits;
      }
      if (intNumMisses > 0)
      {
        _dictStats[READ_MISSES] += intNumMisses;
      }
      _dictStats[READ_MSECS] += (long)DateTime.Now.Subtract(dtStart).TotalMilliseconds;
    }

    public void CountRead(DateTime dtStart, bool isHit)
    {
      _dictStats[(isHit ? READ_HITS : READ_MISSES)]++;
      _dictStats[READ_MSECS] += (long)DateTime.Now.Subtract(dtStart).TotalMilliseconds;
    }

    public void CountWrite(DateTime dtStart)
    {
      _dictStats[WRITES]++;
      _dictStats[WRITE_MSECS] += (long)DateTime.Now.Subtract(dtStart).TotalMilliseconds;
    }

    public void CountDelete(DateTime dtStart, bool isHit)
    {
      _dictStats[(isHit ? DELETE_HITS : DELETE_MISSES)]++;
      _dictStats[DELETE_MSECS] += (long)DateTime.Now.Subtract(dtStart).TotalMilliseconds;
    }

    public void CountDelete(DateTime dtStart, long lAttempts, long lHits)
    {
      _dictStats[DELETE_HITS] += lHits;
      _dictStats[DELETE_MISSES] += (lAttempts - lHits);
      _dictStats[DELETE_MSECS] += (long)DateTime.Now.Subtract(dtStart).TotalMilliseconds;
    }

    public CacheStats Stats
    {
      get
      {
        var dictHashEntries = _cacheClient.GetHashAll(STATS_KEY)
          .ToDictionary(n => n.Name);
        var retVal = new CacheStats
        {
          CacheName = _cacheClient.CacheName,
          CacheRegion = _cacheClient.CacheRegion,
          Key = _cacheClient.CacheName.GetHashCode(),
          KeyPart2 = _cacheClient.CacheRegion.GetHashCode()
        };
        HashEntry hashEntry;
        if (dictHashEntries.TryGetValue(READ_HITS, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.ReadHits);
        }
        if (dictHashEntries.TryGetValue(READ_MISSES, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.ReadMisses);
        }
        if (dictHashEntries.TryGetValue(READ_MSECS, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.ReadMsecs);
        }
        if (dictHashEntries.TryGetValue(READ_MSECS_AVG, out hashEntry))
        {
          Double.TryParse(hashEntry.Value.ToString(), out retVal.ReadMsecsAvg);
        }
        if (dictHashEntries.TryGetValue(READ_HIT_RATIO, out hashEntry))
        {
          Double.TryParse(hashEntry.Value.ToString(), out retVal.ReadHitRatio);
        }
        if (dictHashEntries.TryGetValue(DELETE_HITS, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.DeleteHits);
        }
        if (dictHashEntries.TryGetValue(DELETE_MISSES, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.DeleteMisses);
        }
        if (dictHashEntries.TryGetValue(DELETE_MSECS, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.DeleteMsecs);
        }
        if (dictHashEntries.TryGetValue(DELETE_MSECS_AVG, out hashEntry))
        {
          Double.TryParse(hashEntry.Value.ToString(), out retVal.DeleteMsecsAvg);
        }
        if (dictHashEntries.TryGetValue(DELETE_HIT_RATIO, out hashEntry))
        {
          Double.TryParse(hashEntry.Value.ToString(), out retVal.DeleteHitRatio);
        }
        if (dictHashEntries.TryGetValue(WRITES, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.Writes);
        }
        if (dictHashEntries.TryGetValue(WRITE_MSECS, out hashEntry))
        {
          Int64.TryParse(hashEntry.Value.ToString(), out retVal.WriteMsecs);
        }
        if (dictHashEntries.TryGetValue(WRITE_MSECS_AVG, out hashEntry))
        {
          Double.TryParse(hashEntry.Value.ToString(), out retVal.WriteMsecsAvg);
        }
        return retVal;
      }
    }

    private static void OnTimer(object o)
    {
      foreach (CacheMonitor monitor in _dictMonitorsByCachePrefix.Values)
      {
        try
        {
          monitor.UpdateRegionStats();
        }
        catch (TimeoutException)
        {
        }
      }
    }

    private void UpdateRegionStats()
    {
      long lWrites, lReadHits, lReadMisses, lDeleteHits, lDeleteMisses, lWriteMsecs, lReadMsecs, lDeleteMsecs;
      lock (_dictStats)
      {
        lWrites = _dictStats[WRITES];
        lWriteMsecs = _dictStats[WRITE_MSECS];
        lReadHits = _dictStats[READ_HITS];
        lReadMisses = _dictStats[READ_MISSES];
        lReadMsecs = _dictStats[READ_MSECS];
        lDeleteHits = _dictStats[DELETE_HITS];
        lDeleteMisses = _dictStats[DELETE_MISSES];
        lDeleteMsecs = _dictStats[DELETE_MSECS];

        _dictStats[WRITES] = 0;
        _dictStats[WRITE_MSECS] = 0;
        _dictStats[READ_HITS] = 0;
        _dictStats[READ_MISSES] = 0;
        _dictStats[READ_MSECS] = 0;
        _dictStats[DELETE_HITS] = 0;
        _dictStats[DELETE_MISSES] = 0;
        _dictStats[DELETE_MSECS] = 0;
      }
      if (lWrites != 0)
      {
        lWrites = _cacheClient.IncrementHash(STATS_KEY, WRITES, lWrites);
        lWriteMsecs = _cacheClient.IncrementHash(STATS_KEY, WRITE_MSECS, lWriteMsecs);
        Double dWriteMsecsAvg = Convert.ToDouble(lWriteMsecs) / lWrites;
        _cacheClient.PutHash(STATS_KEY, WRITE_MSECS_AVG, dWriteMsecsAvg, boolFireAndForget: true);
      }
      if (lReadHits != 0 || lReadMisses != 0)
      {
        lReadHits = _cacheClient.IncrementHash(STATS_KEY, READ_HITS, lReadHits);
        lReadMisses = _cacheClient.IncrementHash(STATS_KEY, READ_MISSES, lReadMisses);
        lReadMsecs = _cacheClient.IncrementHash(STATS_KEY, READ_MSECS, lReadMsecs);
        long lReads = lReadHits + lReadMisses;
        Double dReadHitRatio = lReads == 0 ? 0 : Convert.ToDouble(lReadHits) / lReads;
        Double dReadMsecsAvg = Convert.ToDouble(lReadMsecs) / lReads;
        _cacheClient.PutHash(STATS_KEY, READ_HIT_RATIO, dReadHitRatio, boolFireAndForget: true);
        _cacheClient.PutHash(STATS_KEY, READ_MSECS_AVG, dReadMsecsAvg, boolFireAndForget: true);
      }
      if (lDeleteHits != 0 || lDeleteMisses != 0)
      {
        lDeleteHits = _cacheClient.IncrementHash(STATS_KEY, DELETE_HITS, lDeleteHits);
        lDeleteMisses = _cacheClient.IncrementHash(STATS_KEY, DELETE_MISSES, lDeleteMisses);
        lDeleteMsecs = _cacheClient.IncrementHash(STATS_KEY, DELETE_MSECS, lDeleteMsecs);
        long lDeletes = lDeleteHits + lDeleteMisses;
        Double dDeleteHitRatio = lDeletes == 0 ? 0 : Convert.ToDouble(lDeleteHits) / lDeletes;
        Double dDeleteMsecsAvg = Convert.ToDouble(lDeleteMsecs) / lDeletes;
        _cacheClient.PutHash(STATS_KEY, DELETE_HIT_RATIO, dDeleteHitRatio, boolFireAndForget: true);
        _cacheClient.PutHash(STATS_KEY, DELETE_MSECS_AVG, dDeleteMsecsAvg, boolFireAndForget: true);
      }
    }

  }
}
