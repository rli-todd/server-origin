#define MULTI_ITEM_GET
using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Solishine.CommonLib;
using StackExchange.Redis;

namespace Aci.X.ServerLib
{
  public class EntityCacheBase<ENTITY_TYPE, KEY_TYPE>
    where ENTITY_TYPE : class
    where KEY_TYPE : IComparable<KEY_TYPE>
  {
    private const string VALUE_NOT_IN_DB = "\"\""; // JSON serialized empty string
    private CacheClient _cache = null;
    private byte _tSiteID;
    private int _intMaxKeysPerCacheGet = 50;

    private readonly static Logger _logger = LogManager.GetCurrentClassLogger();

    public EntityCacheBase(string strRegion)
    {
      //if (SubtextServiceConfig.EntityCacheEnabled)
      _cache = new CacheClient(
        intTimeToLiveMins: 1440,
        strRegion: strRegion);
    }

    /// <summary>
    ///  Gets from the cache user single or two-part (64-bit) keys
    /// </summary>
    /// <param name="context"></param>
    /// <param name="keys"></param>
    /// <param name="boolForceRefresh"></param>
    /// <returns></returns>
    public virtual ENTITY_TYPE[] Get(KEY_TYPE[] keys, bool boolForceRefresh = false)
    {
      if (keys.Length == 0)
      {
        return new ENTITY_TYPE[0];
      }
      else if (keys.Length == 1)
      {
        // Use a more efficient path if we're only trying to get one entity.
        ENTITY_TYPE entity = Get(keys[0], boolForceRefresh);
        return entity == default(ENTITY_TYPE)
          ? new ENTITY_TYPE[0]
          : new ENTITY_TYPE[1] { entity };
      }
      /*
       * boolForceRefresh would formerly just force us to read this entity
       * from the database, replacing any potential entry in the cache.
       * Now, we do an explicit cache delete on the keys instead.  This has
       * the effect of not only forcing a refresh of this entity, but it 
       * executes the logic in Remove() that also deletes entries from the
       * UserSpecificEntity cache corresponding to the entities specified here.
       */
      if (boolForceRefresh)
      {
        Remove(keys);
      }
      /*
       * We are creating dictionaries from the results of this call in many
       * places, so ensure that we have uniqueness.
       */
      keys = keys.Distinct().ToArray();

      Dictionary<KEY_TYPE, ENTITY_TYPE> dictEntities = new Dictionary<KEY_TYPE, ENTITY_TYPE>();
      List<KEY_TYPE> listKeysNeeded = new List<KEY_TYPE>();
      // First, try to get as many as we can from the cache
      if (_cache == null || boolForceRefresh)
      {
        listKeysNeeded.AddRange(keys);
      }
      else
      {
#if MULTI_ITEM_GET
        var redisKeys = (from k in keys select k.ToString()).ToArray();
        int intOffset = 0;
        while (intOffset < redisKeys.Length)
        {
          RedisValue[] values = _cache.Get(redisKeys.Skip(intOffset).Take(_intMaxKeysPerCacheGet).ToArray());
          for (int idx = 0; idx < values.Length; idx++)
          {
            if (values[idx] == RedisValue.Null)
            {
              listKeysNeeded.Add(keys[intOffset + idx]);
            }
            else if (VALUE_NOT_IN_DB != values[idx]) // Empty string indicates we looked this up in the DB and did not find it.
            {
              dictEntities[keys[intOffset + idx]] = JsonObjectSerializer.Deserialize<ENTITY_TYPE>(values[idx]);
            }
          }
          intOffset += _intMaxKeysPerCacheGet;
        }
#else
        foreach (KEY_TYPE key in keys)
        {
          ENTITY_TYPE entity = _cache.Search<ENTITY_TYPE>(key.ToString());
          if (entity == default(ENTITY_TYPE))
          {
            listKeysNeeded.Add(key);
          }
          else
          {
            dictEntities[key] = entity;
          }
        }
#endif
      }
      // Now, go retrieve any missing assignments directly from the database.

      if (listKeysNeeded.Count > 0)
      {
        try
        {
          ENTITY_TYPE[] entities = GetFromDB(listKeysNeeded.ToArray(), 100); // TODO: allow this max to be set on entity-type basis
#if MULTI_ITEM_PUT
          _cache.Put(
            strUserCacheKeys: (from k in listKeysNeeded select k.ToString()).ToArray(),
            oValues: entities,
            boolFireAndForget: false);
#else
          foreach (var entity in entities)
          {
            _cache.Put(
              strUserCacheKey: GetKey(entity).ToString(),
              oValue: entity);
          }
#endif
          foreach (ENTITY_TYPE entity in entities)
          {
            KEY_TYPE key = GetKey(entity);
            {
              dictEntities[key] = entity;
            }
          }
          /*
           * Finally deal with the case where we asked for something from the DB
           * but got nothing back.   In this case, we will store a "null" in the cache,
           * and when we try to retrieve this value, when we see this "null" value, we
           * will not try to retrieve it from the DB again.
           */
          if (entities.Length != listKeysNeeded.Count)
          {
            foreach (var key in listKeysNeeded)
            {
              if (!dictEntities.ContainsKey(key))
              {
                _cache.Put(strUserCacheKey: key.ToString(), oValue: String.Empty);
              }
            }
          }
        }
        catch (Exception ex)
        {
          _logger.Error("Error setting {0}: {1} at {2}", typeof(ENTITY_TYPE).Name, ex.Message, ex.StackTrace);
        }
      }
      /*
       * Finally, we need to return the entities in the exact order
       * in which the keys were specified.  Note that the array of 
       * returned values may not necessarily be the same size of the
       * array of requested keys.  One example would be where the 
       * requested key does not exist.
       */
      List<ENTITY_TYPE> retVal = new List<ENTITY_TYPE>();
      for (int idxRequested = 0; idxRequested < keys.Length; idxRequested++)
      {
        if (dictEntities.ContainsKey(keys[idxRequested]))
          retVal.Add(dictEntities[keys[idxRequested]]);
      }
      return retVal.ToArray();
    }

    /// <summary>
    /// Retrieves a single entity from the cache
    /// </summary>
    /// <param name="context"></param>
    /// <param name="key"></param>
    /// <param name="boolForceRefresh"></param>
    /// <returns></returns>
    public ENTITY_TYPE Get(KEY_TYPE key, bool boolForceRefresh = false)
    {
      ENTITY_TYPE entity = default(ENTITY_TYPE);
      RedisValue value = RedisValue.Null;
      if (boolForceRefresh)
      {
        Remove(key);
      }
      else
      {
        value = _cache.Get(key.ToString());
      }
      if (value == RedisValue.Null)
      {
        ENTITY_TYPE[] entities = GetFromDB(new KEY_TYPE[] { key });
        if (entities.Length == 0)
        {
          // Not found.  We'll put an store an empty string so we don't keep looking for this
          _cache.Put(strUserCacheKey: key.ToString(), oValue: String.Empty);
        }
        else
        {
          entity = entities[0];
#if TRACE_CACHE_PUTS
        _logger.Trace(context, "Put(1) key={0}: {1}", key.ToString(), JsonObjectSerializer.Serialize(entity));
#endif
          _cache.Put(strUserCacheKey: key.ToString(), oValue: entity, boolFireAndForget: false);
        }
      }
      else if (VALUE_NOT_IN_DB != (string)value) // Empty string indicates we looked this up in the DB and did not find it
      {
        entity = JsonObjectSerializer.Deserialize<ENTITY_TYPE>(value);
      }
      return entity;
    }

    public void Set(ENTITY_TYPE entity)
    {
#if TRACE_CACHE_PUTS
      _logger.Trace(CallContextType.NullContext, "Put(2) key={0}: {1}", GetCompositeKey(entity).ToString(), JsonObjectSerializer.Serialize(entity));
#endif
      _cache.Put(strUserCacheKey: GetKey(entity).ToString(), oValue: entity);
    }

    public void Remove(KEY_TYPE[] keys)
    {
      if (keys != null && keys.Length > 0)
      {
        if (_cache != null)
        {
          _cache.Remove((from k in keys select k.ToString()).ToArray());
        }
      }
    }
    public virtual void Remove(KEY_TYPE key)
    {
      if (_cache != null)
      {
        _cache.Remove(key.ToString());
      }
    }

    public void Flush()
    {
      if (_cache != null)
      {
        _cache.Flush();
      }
    }

    private ENTITY_TYPE[] GetFromDB(KEY_TYPE[] keys, int intMaxEntities)
    {
      if (keys.Length <= intMaxEntities)
      {
        return GetFromDB(keys);
      }
      else
      {
        _logger.Info("Large request for {0} {1} entities", keys.Length, typeof(ENTITY_TYPE).Name);
        // Get in blocks
        var entities = new List<ENTITY_TYPE>();
        int intOffset = 0;
        DateTime dtStart = DateTime.Now;
        while (intOffset < keys.Length)
        {
          int intCount = Math.Min(keys.Length - intOffset, intMaxEntities);
          entities.AddRange(GetFromDB(keys.Skip(intOffset).Take(intCount).ToArray()));
          intOffset += intCount;
        }
        _logger.Info("Large request for {0} {1} entities completed in {2} msecs",
          keys.Length, typeof(ENTITY_TYPE).Name, (int)DateTime.Now.Subtract(dtStart).TotalMilliseconds);
        return entities.ToArray();
      }
    }

    protected virtual ENTITY_TYPE[] GetFromDB(KEY_TYPE[] keys)
    {
      return new ENTITY_TYPE[0];
    }

    protected virtual KEY_TYPE GetKey(ENTITY_TYPE t)
    {
      return default(KEY_TYPE);
    }
  }
}
