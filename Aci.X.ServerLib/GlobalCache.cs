namespace Aci.X.ServerLib
{
  public class GlobalCache 
  {
    //private static GlobalCache _cache = new GlobalCache();

    //public static ACIDB.DBProductDictionary GetProductItemDictionary(int intSiteID)
    //{
    //  string strKey = GetProductItemKey(intSiteID);
    //  ACIDB.DBProductDictionary dictRet = _cache.Get<ACIDB.DBProductDictionary>(strKey);
    //  if (dictRet==null)
    //  {
    //    using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
    //    {
    //      ACIDB.spProductItemGet sp = new ACIDB.spProductItemGet(conn);
    //      dictRet = new ACIDB.DBProductDictionary(sp.Execute(intSiteID: intSiteID));
    //      _cache.Put(strKey, dictRet);
    //    }
    //  }
    //  return dictRet;
    //}

    //public static ACIDB.DBSkuProductDictionary GetProductSkuDictionary(int intSiteID)
    //{
    //  string strKey = GetProductSkuKey(intSiteID);
    //  ACIDB.DBSkuProductDictionary dictRet = _cache.Get<ACIDB.DBSkuProductDictionary>(strKey);
    //  if (dictRet == null)
    //  {
    //    dictRet = new ACIDB.DBSkuProductDictionary(GetProductItemDictionary(intSiteID).Values.ToArray());
    //    _cache.Put(strKey, dictRet);
    //  }
    //  return dictRet;
    //}


    //private static string GetProductItemKey(int intSiteID)
    //{
    //  return "ProductItems_" + intSiteID.ToString();
    //}

    //private static string GetProductSkuKey(int intSiteID)
    //{
    //  return "ProductSkus_" + intSiteID.ToString();
    //}

    //public static void ClearProductDictionaries(int intSiteID)
    //{
    //  _cache.Put(GetProductItemKey(intSiteID), null);
    //  _cache.Put(GetProductSkuKey(intSiteID), null);
    //}

  }
}
