//using System;
//using System.Data.SqlClient;
//using Aci.X.ClientLib;
//using Aci.X.ServerLib;
//using ACIDB=Aci.X.Database;

//namespace Aci.X.Business.Cache
//{
//  public class VisitCacheOld : CacheClient
//  {
//    private Guid _visitGuid;

//    public VisitCacheOld(string strVisitGuid)
//      : base(intTimeToLiveMins: WebServiceConfig.VisitCacheTimeoutMins)
//    {
//      Guid.TryParse(strVisitGuid, out _visitGuid);
//    }

//    public VisitCacheOld(Guid visitGuid)
//      : base(intTimeToLiveMins: WebServiceConfig.VisitCacheTimeoutMins)
//    {
//      _visitGuid = visitGuid;
//    }

//    public string GetCacheKey(string strCacheKey)
//    {
//      return _visitGuid + "_" + strCacheKey;
//    }

//    const string CART_KEY = "Cart";
//    public Cart Cart
//    {
//      get
//      {
//        Cart cartRet = Get<Cart>(GetCacheKey(CART_KEY));
//        if (cartRet == null)
//        {
//          cartRet = new Cart();
//          Cart = cartRet;
//        }
//        return cartRet;
//      }
//      set
//      {
//        Put(GetCacheKey(CART_KEY), value);
//      }
//    }

//    const string USER_KEY = "User";
//    public ACIDB.DBUser User
//    {
//      get
//      {
//        ACIDB.DBUser userRet = Get<ACIDB.DBUser>(GetCacheKey(USER_KEY));
//        if (userRet == null)
//        {
//          userRet = new ACIDB.DBUser();
//          User = userRet;
//        }
//        return userRet;
//      }
//      set
//      {
//        Put(GetCacheKey(USER_KEY), value);
//      }
//    }

//    const string VISIT_KEY = "Visit";
//    public ACIDB.DBVisit Visit
//    {
//      get
//      {
//        ACIDB.DBVisit visitRet = Get<ACIDB.DBVisit>(GetCacheKey(VISIT_KEY));
//        if (visitRet==null)
//        {
//          using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
//          {
//            visitRet = new ACIDB.spVisitGetByToken(conn).Execute(_visitGuid);
//          }
//        }
//        return visitRet;
//      }
//      set
//      {
//        Put(GetCacheKey(VISIT_KEY), value);
//      }
//    }

//  }
//}
