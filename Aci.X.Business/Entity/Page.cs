using System;
using NLog;
using System.Collections.Generic;
using Aci.X.DatabaseEntity;
using DB=Aci.X.Database;
using Aci.X.ServerLib;
using Cli = Aci.X.ClientLib;
using Aci.X.ClientLib.Exceptions;

namespace Aci.X.Business
{
  public class Page
  {
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    public static ClientLib.Page[] ToClient(DBPage[] dbPages)
    {
      ClientLib.Page[] retVal = new ClientLib.Page[dbPages.Length];
      for (int idx = 0; idx < dbPages.Length; ++idx)
      {
        retVal[idx] = ToClient(dbPages[idx]);
      }
      return retVal;
    }

    public static ClientLib.Page ToClient(DBPage dbPage)
    {
      return new ClientLib.Page
      {
        PageID = dbPage.PageID,
        PageCode = dbPage.PageCode,
        Description = dbPage.Description
      };
    }

    public static Cli.ContentBlock[] GetContent(
      CallContext context, 
      string strPageCode, 
      string strWhere,
      IEnumerable<KeyValuePair<string, string>> keyValuePairs)
    {
      Cli.ContentBlock[] retVal = null;
      DBSectionTemplate[] sectionTemplates = null;
      string strCacheKey = null;
      CacheClient cache = null;
      string strWhereLower = null;
      if (strWhere != null)
      {
        // Do our best to foil potential SQL injection attack.
        // This adds about 5 msecs to every call to this API.
        //
        // After we have stabilized on a range of valid where clauses
        // for all pages, we may want to eliminate this method in favor 
        // of using specific field names, rather than a whole "where clause".
        strWhereLower = strWhere.ToLower();
        if (
          strWhereLower.Contains("select") ||
          strWhereLower.Contains("insert") ||
          strWhereLower.Contains("update") ||
          strWhereLower.Contains("delete") ||
          strWhereLower.Contains("drop") ||
          strWhereLower.Contains("create") ||
          strWhereLower.Contains("alter") ||
          strWhereLower.Contains("exec") ||
          strWhereLower.Contains("dbcc") ||
          strWhereLower.Contains("truncate"))
        {
          throw new InvalidWhereClauseException();
        }
      }

      if (WebServiceConfig.CacheEnabled)
      {
        strCacheKey = "ST::" + (strPageCode ?? "") + "|" + (strWhereLower ?? "");
        cache = new CacheClient();
        sectionTemplates = cache.Get<DBSectionTemplate[]>(strCacheKey);
      }

      using (var db=new DB.AciXDB())
      {
        db.spVisitSetPage(context.VisitID, strPageCode);
        if (sectionTemplates == null)
        {
          try
          {
            sectionTemplates = db.spPageGetContent(
              strPageCode: strPageCode,
              strWhereClause: strWhereLower);
            if (WebServiceConfig.CacheEnabled)
            {
              cache.Put(strCacheKey, sectionTemplates);
            }
          }
          catch (Exception ex)
          {
            _logger.Error("page_code: {0}, where: {1}, ex: {2}, stack: {3}", strPageCode ?? "", strWhereLower?? "", ex.Message, ex.StackTrace);
            throw;
          }
        }
      }

      


      retVal = ContentBlock.ToClient(sectionTemplates, keyValuePairs);
      return retVal;
    }

  }
}
