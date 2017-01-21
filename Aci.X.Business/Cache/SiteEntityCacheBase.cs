using System;
using Aci.X.ServerLib;

namespace Aci.X.Business.Cache
{
  public class SiteEntityCacheBase<ENTITY_TYPE, KEY_TYPE> : EntityCacheBase<ENTITY_TYPE, KEY_TYPE>
    where ENTITY_TYPE : class
    where KEY_TYPE : IComparable<KEY_TYPE> 

  {
    protected byte _tSiteID;

    public SiteEntityCacheBase(byte tSiteID, string strRegion)
      : base("Site_" + tSiteID.ToString()+":" + strRegion)
    {
      _tSiteID = tSiteID;
    }
  }
}
