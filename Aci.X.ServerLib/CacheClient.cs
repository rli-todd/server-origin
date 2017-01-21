using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Aci.X.ClientLib;
using Aci.X.ServerLib;
using StackExchange.Redis;
using NLog;
using JsonObjectSerializer = Solishine.CommonLib.JsonObjectSerializer;

namespace Aci.X.ServerLib
{
  public class CacheClient
  {
    public enum DatabaseNumber
    {
      Default = 0,
      ServerEntity = 1
    };
    protected static Logger _logger = LogManager.GetCurrentClassLogger();
    protected static ConcurrentDictionary<string, string> _dictRegions = new ConcurrentDictionary<string, string>();
    private CacheMonitor _cacheMonitor;
    internal static ConcurrentDictionary<string, ConnectionMultiplexer> DictRedisCacheConnections =
      new ConcurrentDictionary<string, ConnectionMultiplexer>();
    protected TimeSpan _tsTTL;
    protected string _strCacheRegion;
    protected string _strCacheName;
    private string _strConnectionString;
    private ConnectionMultiplexer _redisCache;
    private bool _boolStatisticsEnabled;
    public const char CACHE_KEY_DELIM = ':';

    /// <summary>
    /// Creates a new instance of a cache client associated with a specific Redis
    /// database number and connection string.  This allows us to do a couple of things:
    /// a) connect to multiple databases for the same redis server
    /// b) specific different connection options (for timeouts, etc.)
    /// c) connect to multiple redis servers
    /// 
    /// The binding of connection string + database number is not strictly necessary
    /// and may be eliminated at some future date if necessary.
    /// </summary>
    /// <param name="intTimeToLiveMins"></param>
    /// <param name="strCacheName"></param>
    /// <param name="strRegion"></param>
    /// <param name="strConnectionString"></param>
    /// <param name="intDatabaseNumber"></param>
    public CacheClient(
      int intTimeToLiveMins = 14400,
      string strCacheName = "ACIX",
      string strRegion = "Default",
      string strConnectionString = null,
      bool boolStatisticsEnabled = true)
    {
      _tsTTL = new TimeSpan(hours: 0, minutes: intTimeToLiveMins, seconds: 0);
      _strCacheRegion = strRegion;
      _strCacheName = strCacheName;
      _boolStatisticsEnabled = boolStatisticsEnabled;
      _strConnectionString = strConnectionString ?? DefaultConnectionString;
      if (_boolStatisticsEnabled)
        _cacheMonitor = CacheMonitor.Init(this);
      if (!DictRedisCacheConnections.TryGetValue(_strConnectionString, out _redisCache))
      {
        try
        {
          _redisCache = ConnectionMultiplexer.Connect(_strConnectionString);
          DictRedisCacheConnections[_strConnectionString] = _redisCache;
        }
        catch (Exception ex)
        {
          _logger.Error("Could not connect to cache {0}: {1} at {2}", _strConnectionString, ex.Message, ex.StackTrace);
        }
      }
    }

    internal string ConnectionString
    {
      get { return _strConnectionString; }
    }

    private string DefaultConnectionString
    {
      get
      {
        string strCacheHostname = WebServiceConfig.GetSetting("Redis.Cache.Hostname", "DB3v.pr.local");
        string strCachePort = WebServiceConfig.GetSetting("Redis.Cache.Port", "6379");
        return strCacheHostname + ":" + strCachePort;
      }
    }
    /// <summary>
    /// Gets a single string value stored at the specified key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public RedisValue Get(string strUserCacheKey)
    {
      RedisValue retVal = RedisValue.Null;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .StringGet(KeyPrefix + strUserCacheKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, !String.IsNullOrEmpty(retVal));
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Gets multiple string values for the specified keys
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public RedisValue[] Get(string[] strUserCacheKeys)
    {
      RedisValue[] retVal = new RedisValue[strUserCacheKeys.Length];
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        RedisKey[] keys = (from k in strUserCacheKeys select (RedisKey)KeyPrefix + k).ToArray();
        retVal = _redisCache
          .GetDatabase()
          .StringGet(keys);
        int intNumMisses = (from v in retVal where v.IsNull select v).Count();
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, intNumHits: keys.Length - intNumMisses, intNumMisses: intNumMisses);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Gets a single string value stored at the specified key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public async Task<RedisValue> GetAsync(string strUserCacheKey)
    {
      RedisValue retVal = RedisValue.Null;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = await _redisCache
          .GetDatabase()
          .StringGetAsync(KeyPrefix + strUserCacheKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, !retVal.IsNullOrEmpty);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Gets the value stored at the specified key and deserializes it from JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public T Get<T>(string strUserCacheKey) /* where T : class */
    {
      RedisValue value = Get(strUserCacheKey);
      if (value == RedisValue.Null)
        return default(T);
      return (T)JsonObjectSerializer.Deserialize(typeof(T), value);
    }

    /// <summary>
    /// Gets the values stored at the specified keys and deserializes them from JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strUserCacheKeys"></param>
    /// <returns></returns>
    public T[] Get<T>(string[] strUserCacheKeys) /* where T : class */
    {
      RedisValue[] values = Get(strUserCacheKeys);
      T[] retVal = new T[values.Length];
      for (int idx = 0; idx < values.Length; idx++)
      {
        retVal[idx] = (values[idx] == RedisValue.Null)
          ? default(T)
          : (T)JsonObjectSerializer.Deserialize(typeof(T), values[idx]);
      }
      return retVal;
    }

    /// <summary>
    /// Gets the value stored at the specified key and deserializes it from JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string strUserCacheKey) /* where T : class */
    {
      RedisValue value = await GetAsync(strUserCacheKey);
      if (value == RedisValue.Null)
        return default(T);
      return (T)JsonObjectSerializer.Deserialize(typeof(T), value);
    }

    /// <summary>
    /// Gets a the value stored in a hash at the specified cache key
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public RedisValue GetHash(string strUserCacheKey, string strHashKey)
    {
      RedisValue retVal = RedisValue.Null;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .HashGet(
            KeyPrefix + strUserCacheKey,
            strHashKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, !retVal.IsNullOrEmpty);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Gets all the values stored in a hash at the specified cache key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public HashEntry[] GetHashAll(string strUserCacheKey)
    {
      HashEntry[] retVal = new HashEntry[0];
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .HashGetAll(
            KeyPrefix + strUserCacheKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, retVal != null);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Gets a the value stored in a hash at the specified cache key
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public async Task<RedisValue> GetHashAsync(string strUserCacheKey, string strHashKey)
    {
      RedisValue retVal = RedisValue.Null;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = await _redisCache
          .GetDatabase()
          .HashGetAsync(
            KeyPrefix + strUserCacheKey,
            strHashKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, !retVal.IsNullOrEmpty);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Clears the value stored in a hash at the specified cache key
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public void RemoveHash(string strUserCacheKey, string strHashKey)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        bool boolHit = _redisCache
          .GetDatabase()
          .HashDelete(
            KeyPrefix + strUserCacheKey,
            strHashKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountDelete(dtStart, boolHit);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Clears the value stored in a hash at the specified cache key
    /// </summary>
    /// <param name="intUserID"></param>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public async Task RemoveHashAsync(string strUserCacheKey, string strHashKey)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        bool boolHit = await _redisCache
          .GetDatabase()
          .HashDeleteAsync(
            KeyPrefix + strUserCacheKey,
            strHashKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountDelete(dtStart, boolHit);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Gets a hash value stored at the specified key and deserialized it from JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="intUserID"></param>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public T GetHash<T>(string strUserCacheKey, string strHashkey)
    {
      RedisValue value = GetHash(strUserCacheKey, strHashkey);
      if (value == RedisValue.Null)
        return default(T);
      return (T)JsonObjectSerializer.Deserialize(typeof(T), value);
    }

    /// <summary>
    /// Gets a hash value stored at the specified key and deserialized it from JSON
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="intUserID"></param>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public async Task<T> GetHashAsync<T>(string strUserCacheKey, string strHashkey)
    {
      RedisValue value = await GetHashAsync(strUserCacheKey, strHashkey);
      if (value == RedisValue.Null)
        return default(T);
      return (T)JsonObjectSerializer.Deserialize(typeof(T), value);
    }

    /// <summary>
    /// Gets all key value pairs stored at the specified key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public KeyValuePair<string, string>[] GetAll(string strUserCacheKey)
    {
      KeyValuePair<string, string>[] retVal = null;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        HashEntry[] hashEntries = _redisCache
          .GetDatabase()
          .HashGetAll(KeyPrefix + strUserCacheKey);
        if (hashEntries != null)
          retVal = (from h in hashEntries select new KeyValuePair<string, string>(h.Name, h.Value)).ToArray();
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, hashEntries != null);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Gets all key value pairs stored at the specified key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <returns></returns>
    public async Task<KeyValuePair<string, string>[]> GetAllAsync(string strUserCacheKey)
    {
      KeyValuePair<string, string>[] retVal = null;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        HashEntry[] hashEntries = await _redisCache
          .GetDatabase()
          .HashGetAllAsync(KeyPrefix + strUserCacheKey);
        if (hashEntries != null)
          retVal = (from h in hashEntries select new KeyValuePair<string, string>(h.Name, h.Value)).ToArray();
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, hashEntries != null);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Stores an object with the specified key.  
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="oValue"></param>
    public void Put(string strUserCacheKey, object oValue, bool boolFireAndForget = false)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        _redisCache
          .GetDatabase()
          .StringSet(
            key: KeyPrefix + strUserCacheKey,
            value: JsonObjectSerializer.Serialize(oValue),
            expiry: _tsTTL,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Stores multiple objects.  Note that there is no TTL for this method.
    /// </summary>
    /// <param name="strUserCacheKeys"></param>
    /// <param name="oValues"></param>
    /// <param name="boolFireAndForget"></param>
    public void Put(string[] strUserCacheKeys, object[] oValues, bool boolFireAndForget = false)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        KeyValuePair<RedisKey, RedisValue>[] keyValuePairs = new KeyValuePair<RedisKey, RedisValue>[strUserCacheKeys.Length];
        for (int idx = 0; idx < keyValuePairs.Length; idx++)
        {
          keyValuePairs[idx] = new KeyValuePair<RedisKey, RedisValue>(
            key: KeyPrefix + strUserCacheKeys[idx],
            value: JsonObjectSerializer.Serialize(oValues[idx]));
        }
        _redisCache
          .GetDatabase()
          .StringSet(
            values: keyValuePairs,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Stores an object with the specified key.  
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="oValue"></param>
    public async Task PutAsync(string strUserCacheKey, object oValue, bool boolFireAndForget = false)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        await _redisCache
          .GetDatabase()
          .StringSetAsync(
            key: KeyPrefix + strUserCacheKey,
            value: JsonObjectSerializer.Serialize(oValue),
            expiry: _tsTTL,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Increments counter at the specified key by the specified value
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="oValue"></param>
    public long Increment(string strUserCacheKey, long lValue, bool boolFireAndForget = false)
    {
      long retVal = 0;
      if (_redisCache == null)
        return 0;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .StringIncrement(
            key: KeyPrefix + strUserCacheKey,
            value: lValue,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Increments counter the specified key by the specified value
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="oValue"></param>
    public async Task<long> IncrementAsync(string strUserCacheKey, long lValue, bool boolFireAndForget = false)
    {
      long retVal = 0;
      if (_redisCache == null)
        return 0;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = await _redisCache
          .GetDatabase()
          .StringIncrementAsync(
            key: KeyPrefix + strUserCacheKey,
            value: lValue,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Stores a particular value into a hash stored at the user cache key
    /// Value is stored as string by calling value's ToString() method.  
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="strHashKey"></param>
    /// <param name="oValue"></param>
    public void PutHash(string strUserCacheKey, string strHashKey, object oValue, bool boolFireAndForget = false)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        IDatabase db = _redisCache.GetDatabase();
        string strInternalKey = KeyPrefix + strUserCacheKey;
        CommandFlags flags = boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None;
        db.HashSet(
          key: strInternalKey,
          hashField: strHashKey,
          value: JsonObjectSerializer.Serialize(oValue),
          when: When.Always,
          flags: flags);
        db.KeyExpire(
          key: strInternalKey,
          expiry: _tsTTL,
          flags: flags);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Stores a particular value into a hash stored at the user cache key
    /// Value is stored as string by calling value's ToString() method.  
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="strHashKey"></param>
    /// <param name="oValue"></param>
    public async Task PutHashAsync(string strUserCacheKey, string strHashKey, object oValue, bool boolFireAndForget = false)
    {
      if (_redisCache == null)
        return;
      try
      {
        DateTime dtStart = DateTime.Now;
        IDatabase db = _redisCache.GetDatabase();
        string strInternalKey = KeyPrefix + strUserCacheKey;
        CommandFlags flags = boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None;
        await db.HashSetAsync(
          key: strInternalKey,
          hashField: strHashKey,
          value: JsonObjectSerializer.Serialize(oValue),
          when: When.Always,
          flags: flags);
        await db.KeyExpireAsync(
          key: strInternalKey,
          expiry: _tsTTL,
          flags: flags);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
    }

    /// <summary>
    /// Increments the value in the hash stored at the user cache key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="strHashKey"></param>
    /// <param name="oValue"></param>
    public long IncrementHash(string strUserCacheKey, string strHashKey, long lValue, bool boolFireAndForget = false)
    {
      long retVal = 0;
      if (_redisCache == null)
        return 0;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .HashIncrement(
            key: KeyPrefix + strUserCacheKey,
            hashField: strHashKey,
            value: lValue,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Increments the value in the hash stored at the user cache key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    /// <param name="strHashKey"></param>
    /// <param name="oValue"></param>
    public async Task<long> IncrementHashAsync(string strUserCacheKey, string strHashKey, long lValue, bool boolFireAndForget = false)
    {
      long retVal = 0;
      if (_redisCache == null)
        return 0;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = await _redisCache
          .GetDatabase()
          .HashIncrementAsync(
            key: KeyPrefix + strUserCacheKey,
            hashField: strHashKey,
            value: lValue,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountWrite(dtStart);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Remove an object from the cache
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    public bool Remove(string strUserCacheKey, bool boolFireAndForget = false)
    {
      bool retVal = false;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .KeyDelete(
            key: KeyPrefix + strUserCacheKey,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountDelete(dtStart, retVal);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Remove multiple objects from the cache
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    public long Remove(string[] strUserCacheKeys, bool boolFireAndForget = false)
    {
      RedisKey[] keys = (from k in strUserCacheKeys select (RedisKey)(KeyPrefix + k)).ToArray();
      return Remove(keys, boolFireAndForget);
    }

    /// <summary>
    /// Remove multiple objects from the cache
    /// </summary>
    /// <param name="keys"></param>
    public long Remove(RedisKey[] keys, bool boolFireAndForget = false)
    {
      long retVal = 0;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .KeyDelete(
            keys: keys,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountDelete(dtStart: dtStart, lAttempts: keys.Length, lHits: retVal);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Remove an object from the cache
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    public async Task<bool> RemoveAsync(string strUserCacheKey, bool boolFireAndForget = false)
    {
      bool retVal = false;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = await _redisCache
          .GetDatabase()
          .KeyDeleteAsync(
            key: KeyPrefix + strUserCacheKey,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountDelete(dtStart, retVal);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Removes multiple objects from the cache
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    public async Task<long> RemoveAsync(string[] strUserCacheKeys, bool boolFireAndForget = false)
    {
      long retVal = 0;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        RedisKey[] keys = (from k in strUserCacheKeys select (RedisKey)(KeyPrefix + k)).ToArray();
        retVal = await _redisCache
          .GetDatabase()
          .KeyDeleteAsync(
            keys: keys,
            flags: boolFireAndForget ? CommandFlags.FireAndForget : CommandFlags.None);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountDelete(dtStart: dtStart, lAttempts: strUserCacheKeys.Length, lHits: retVal);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Check for the existence of a key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    public bool Exists(string strUserCacheKey)
    {
      bool retVal = false;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = _redisCache
          .GetDatabase()
          .KeyExists(
            key: KeyPrefix + strUserCacheKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, retVal);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    /// <summary>
    /// Check for the existence of a key
    /// </summary>
    /// <param name="strUserCacheKey"></param>
    public async Task<bool> ExistsAsync(string strUserCacheKey)
    {
      bool retVal = false;
      if (_redisCache == null)
        return retVal;
      try
      {
        DateTime dtStart = DateTime.Now;
        retVal = await _redisCache
          .GetDatabase()
          .KeyExistsAsync(
            key: KeyPrefix + strUserCacheKey);
        if (_boolStatisticsEnabled)
          _cacheMonitor.CountRead(dtStart, retVal);
      }
      catch (Exception ex)
      {
        LogException(ex);
      }
      return retVal;
    }

    public void Flush()
    {
      RedisKey[] keys = _redisCache
        .GetServer(_strConnectionString)
        .Keys(pattern: KeyPrefix + "*")
        .ToArray();
      Remove(keys);
      if (_cacheMonitor != null)
        _cacheMonitor.Reset();
    }

    public string KeyPrefix
    {
      get { return _strCacheName + CACHE_KEY_DELIM + _strCacheRegion + CACHE_KEY_DELIM; }
    }

    public string CacheName
    {
      get { return _strCacheName; }
    }

    public string CacheRegion
    {
      get { return _strCacheRegion; }
    }

    public CacheStats Stats
    {
      get { return _cacheMonitor == null ? null : _cacheMonitor.Stats; }
    }

    public static CacheStats[] AllStats
    {
      get
      {
        var dictCacheStats = new Dictionary<string, CacheStats>();
        foreach (var strConnection in DictRedisCacheConnections.Keys)
        {
          var redisCache = DictRedisCacheConnections[strConnection];
          var keys = redisCache
          .GetServer(strConnection)
          .Keys(pattern: "*" + CACHE_KEY_DELIM+CacheMonitor.STATS_KEY)
          .ToArray();
          foreach (var key in keys)
          {
            if (!dictCacheStats.ContainsKey(key))
            {
              string[] strParts = ((string)key).Split(CACHE_KEY_DELIM);
              dictCacheStats[key] = new CacheClient(
                strCacheName: strParts[0],
                strRegion: strParts[1],
                strConnectionString: strConnection).Stats;
            }
          }
        }
        return dictCacheStats.Values.ToArray();
      }
    }

    protected void LogException(Exception ex)
    {
      while (ex != null)
      {
        string strMessage = String.Format("{0} at {1}", ex.Message, ex.StackTrace);
        _logger.Warn(strMessage);
        ex = ex.InnerException;
      }
    }
  }
}
