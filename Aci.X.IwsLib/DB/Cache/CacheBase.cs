using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Concurrent;
using Solishine.CommonLib;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.DB.Cache
{
  public class CacheBase
  {
    protected ConcurrentDictionary<string,int> _dictCache = new ConcurrentDictionary<string,int>();
    protected string _strConnectionString;
    protected int _intCacheMisses;
    protected int _intCacheHits;

    public static ConcurrentBag<CacheBase> AllCaches = new ConcurrentBag<CacheBase>();

    public CacheBase( string strConnectionString)
    {
      _strConnectionString = strConnectionString;
      AllCaches.Add(this);
    }

    public bool Enabled
    {
      get { return true; }
    }

    public virtual string Name
    {
      get
      {
        return this.GetType().Name;
      }
    }

    public int Hits
    {
      get { 
        return _intCacheHits;
      }
    }

    public int Misses
    {
      get
      {
        return _intCacheMisses;
      }
    }

    public Single HitRatio
    {
      get
      {
        int intTotal = (_intCacheHits + _intCacheMisses);
        return intTotal == 0 ? 0 : ((Single)_intCacheHits) / intTotal;
      }
    }

    public int GetID(string strKey)
    {
      int intRetVal = 0;
      if (Enabled && _dictCache.ContainsKey(strKey))
      {
        _intCacheHits++;
        intRetVal = _dictCache[strKey];
      }
      else
      {
        _intCacheMisses++;
        using (SqlConnection conn = new SqlConnection(_strConnectionString))
        {
          intRetVal = QueryDB(conn, strKey??"");
          _dictCache[strKey] = intRetVal;
        }
      }
      return intRetVal;
    }

    public virtual int QueryDB(SqlConnection conn, string strKey)
    {
      return 0;
    }
  }

  public class GeoLocationCache : CacheBase
  {
    public GeoLocationCache()
      : base(IwsConfig.SolishineGeoConnectionString)
    {
    }

    public override int QueryDB(SqlConnection conn, string strKey)
    {
      spGeoLocationGetByCityStateZip sp = new spGeoLocationGetByCityStateZip(conn);
      string[] strParts = strKey.Split('|');
      return sp.Execute(strCityName: strParts[0], strStateCode: strParts[1], strZip: strParts[2]);
    }
  }

  public class CompanyCache : CacheBase
  {
    public CompanyCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public override int QueryDB(SqlConnection conn, string strKey)
    {
      spCompanyGetID sp = new spCompanyGetID(conn);
      return sp.Execute(strCompanyName: strKey);
    }
  }

  public class SchoolCache : CacheBase
  {
    public SchoolCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public override int QueryDB(SqlConnection conn, string strKey)
    {
      spSchoolGetID sp = new spSchoolGetID(conn);
      return sp.Execute(strSchoolName: strKey);
    }
  }

  public class FirstNameCache : CacheBase
  {
    public FirstNameCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public override int QueryDB(SqlConnection conn, string strKey)
    {
      spFirstNameGetID sp = new spFirstNameGetID(conn);
      return sp.Execute(strFirstName: strKey);
    }
  }

  public class MiddleNameCache : CacheBase
  {
    public MiddleNameCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public override int QueryDB(SqlConnection conn, string strKey)
    {
      spMiddleNameGetID sp = new spMiddleNameGetID(conn);
      return sp.Execute(strMiddleName: strKey);
    }
  }

  public class LastNameCache : CacheBase
  {
    public LastNameCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public override int QueryDB(SqlConnection conn, string strKey)
    {
      spLastNameGetID sp = new spLastNameGetID(conn);
      return sp.Execute(strLastName: strKey);
    }
  }

  public class PersonCache: CacheBase
  {
    public PersonCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public int GetID(
      string strProfileID, 
      int? intFirstNameID,
      int? intMiddleNameID,
      int? intLastNameID,
      DateTime? dtBirth=null,
      int? intPhoneCount=null,
      int? intEmailCount=null,
      string strListSchoolIDs=null,
      string strListRelativeIDs=null,
      string strListCompanyIDs=null,
      string strListGeoLocationIDs=null,
      string strListAliasIDs=null)
    {
      int intRetVal = 0;
      if (Enabled && _dictCache.ContainsKey(strProfileID))
      {
        _intCacheHits++;
        intRetVal = _dictCache[strProfileID];
      }
      else
      {
        _intCacheMisses++;
        using (SqlConnection conn = new SqlConnection(_strConnectionString))
        {
          spPersonGetID sp = new spPersonGetID(conn);
          intRetVal = sp.Execute(
            strProfileID: strProfileID,
            intFirstNameID: intFirstNameID,
            intMiddleNameID: intMiddleNameID,
            intLastNameID: intLastNameID,
            dtBirth: dtBirth,
            intPhoneCount: intPhoneCount,
            intEmailCount: intEmailCount,
            strListSchoolIDs: strListSchoolIDs,
            strListRelativeIDs: strListRelativeIDs,
            strListCompanyIDs: strListCompanyIDs,
            strListGeoLocationIDs: strListGeoLocationIDs,
            strListAliasIDs: strListAliasIDs);
          _dictCache[strProfileID] = intRetVal;
        }
      }
      return intRetVal;
    }
  }

  public class AliasCache : CacheBase
  {
    public AliasCache()
      : base(IwsConfig.ProfileConnectionString)
    {
    }

    public int GetID(
      int? intFirstNameID,
      int? intMiddleNameID,
      int? intLastNameID)
    {
      int intRetVal = 0;
      string strKey = (intFirstNameID ?? 0) + "|" + (intMiddleNameID ?? 0) + "|" + (intLastNameID ?? 0);
      if (Enabled && _dictCache.ContainsKey(strKey))
      {
        _intCacheHits++;
        intRetVal = _dictCache[strKey];
      }
      else
      {
        _intCacheMisses++;
        using (SqlConnection conn = new SqlConnection(_strConnectionString))
        {
          spAliasGetID sp = new spAliasGetID(conn);
          intRetVal = sp.Execute(
            intFirstNameID: intFirstNameID,
            intMiddleNameID: intMiddleNameID,
            intLastNameID: intLastNameID);
          _dictCache[strKey] = intRetVal;
        }
      }
      return intRetVal;
    }
  }

}
