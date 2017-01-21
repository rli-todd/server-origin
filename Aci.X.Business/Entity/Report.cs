using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.ClientLib.Exceptions;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.IwsLib;
using Aci.X.ServerLib;
using Solishine.CommonLib;
using P=Aci.X.ClientLib.ProfileTypes;
using NLog;

namespace Aci.X.Business
{
  public class Report
  {
    public static Logger _logger = LogManager.GetCurrentClassLogger();

    public static ClientLib.Report Render(DBReport dbReport)
    {
      return new ClientLib.Report
      {
        ReportID = dbReport.ReportID,
        ReportDate = dbReport.ReportDate,
        UserID = dbReport.UserID,
        OrderID = dbReport.OrderID,
        OrderExternalID = dbReport.OrderExternalID,
        QueryID = dbReport.QueryID,
        ProfileID = dbReport.ProfileID,
        Title = String.Format("{0} for {1} {2}{3}", 
          dbReport.Title, dbReport.FirstName, dbReport.LastName,
          String.IsNullOrWhiteSpace(dbReport.State)?"":" in "+dbReport.State),
        ReportTypeCode = dbReport.ReportTypeCode,
        FirstName = dbReport.FirstName,
        MiddleInitial = dbReport.MiddleInitial,
        LastName = dbReport.LastName,
        State = dbReport.State,
        ReportJson = CompressionHelper.UncompressString(dbReport.CompressedJson),
        ReportHtml = CompressionHelper.UncompressString(dbReport.CompressedHtml)
      };
    }

    public static ClientLib.Report[] Search(
      CallContext context,
      string strFirstName = null,
      string strLastName = null,
      string strState = null,
      string strProfileID = null,
      int? intOrderID = null)
    {
      using (var db = new AciXDB())
      {
        var reportIDs = db.spReportSearch(
          intSiteID: context.SiteID,
          intUserID: context.AuthorizedUserID,
          strFirstName: strFirstName,
          strLastName: strLastName,
          strState: strState,
          strProfileID: strProfileID,
          intOrderID: intOrderID);
        return Get(context, reportIDs);
      }
    }

    /*
     * This should not be called without business rules enforcing access
     */
    public static ClientLib.Report[] Get(CallContext context, int[] keys)
    {
      var dbReports = SiteReportCache.ForSite(context.SiteID).Get(keys);
      return (from o in dbReports select Render(o)).ToArray();
    }

    /*
     * This should not be called without business rules enforcing access
     */
    private static ClientLib.Report Get(CallContext context, int intReportID)
    {
      var reports = Get(context, new int[] { intReportID });
      return reports.Length > 0 ? reports[0] : null;
    }

    /*
     * Create a report
     */
    public static ClientLib.Report Create(
      CallContext context,
      string strTypeCode,
      int intOrderID,
      int intQueryID,
      string strProfileID,
      bool boolIsRetry=false)
    {
      var dbOrder = SiteOrderCache.ForSite(context.SiteID).Get(intOrderID);
      if (dbOrder == null || dbOrder.UserID != context.AuthorizedUserID)
      {
        throw new OrderNotFoundException();
      }
      var query = QueryCache.Singleton.Get(intQueryID, boolForceRefresh: true);
      var reports = Search(context, intOrderID: intOrderID);
      if (reports != null)
      {
        var existingReport = (from r 
                                in reports 
                              where r.ReportTypeCode==strTypeCode 
                                && r.ProfileID==strProfileID 
                              select r).FirstOrDefault();
        if (existingReport != null)
          throw new OrderAlreadyFulfilledException();
      }

      var orderItem = (
        from oi in dbOrder.Items
        where oi.ReportTypeCode != null
        select oi).FirstOrDefault();

      var expectedReportTypeCode = orderItem == null ? null : orderItem.ReportTypeCode;

      if (strTypeCode == null && expectedReportTypeCode == null)
      {
        throw new ReportTypeCodeRequiredException();
      }

      if (strTypeCode != null && expectedReportTypeCode != null && !strTypeCode.Equals(expectedReportTypeCode))
      {
        //throw new ReportTypeCodeUnexpectedException(expectedReportTypeCode);
        _logger.Warn("Expected report type of {0} from OrderID {1}, got {1}.  Using a credit?",
          expectedReportTypeCode, intOrderID, strTypeCode);
      }

      var reportType = ReportTypeCache.ForSite(context.SiteID).Get(strTypeCode ?? expectedReportTypeCode, boolForceRefresh: true);
      _logger.Info("Fetching profile for report: strTypeCode={0}, expectedReportTypeCode={1}, reportType={2}, query={3}, queryID={4}",
        strTypeCode ?? "null", expectedReportTypeCode ?? "null",
        reportType == null ? "null" : "not null",
        query == null ? "null" : "not null", intQueryID);

      DateTime dtStart = DateTime.Now;
      var profileResponse = ProfileHelper.GetProfile(
        strProfileID: strProfileID,
        strProfileAttributes: reportType.ProfileAttributes,
        strState: query.State);

      if (profileResponse == null || profileResponse.ProfileCount == 0)
      {
        if (boolIsRetry)
        {
          throw new ProfileNotFoundException();
        }
        else
        {
          ProfileHelper.DeleteCachedProfile(strProfileID);
          return Create(context: context, strTypeCode: strTypeCode, intOrderID: intOrderID, intQueryID: intQueryID,
            strProfileID: strProfileID, boolIsRetry: true);
        }
      }

      var profile = profileResponse.Profiles.Profile[0];
      /*
       * Connect phone numbers to addresses, if possible
       */
      var phones = profile.Phones == null ? new P.Phone[0] : profile.Phones.Phone;
      var addresses = profile.Addresses == null ? new P.Address[0] : profile.Addresses.Address;
      if (phones.Length > 0 && addresses.Length > 0)
      {
        for (int idxAddress = 0; idxAddress < addresses.Length; ++idxAddress)
        {
          List<string> list = new List<string>();
          for (int idxPhone = 0; idxPhone < phones.Length; ++ idxPhone)
          {
            if (phones[idxPhone].AddressIDs != null && phones[idxPhone].AddressIDs.AddressID != null)
            {
              foreach (string strAddressID in phones[idxPhone].AddressIDs.AddressID)
              {
                if (addresses[idxAddress].AddressID.Equals(strAddressID))
                  list.Add(phones[idxPhone].PhoneNumber);
              }
            }
          }
          if (list.Count > 0)
            addresses[idxAddress].Phones = String.Join(", ", list.ToArray());
        }
      }

      /*
       * Some hacky hard-coding for Statewide Criminal Check
       */
      if (strTypeCode.ToUpper().Equals("SCC"))
      {
        var CR = profile.CriminalRecords;
        if (CR != null && CR.CriminalRecord != null && CR.CriminalRecord.Length > 0)
        {
          CR.CriminalRecord = (
            from r in CR.CriminalRecord
            where (r.StateCode ?? "").Equals(query.State, StringComparison.CurrentCultureIgnoreCase)
            select r).ToArray();
        }
        if (CR == null || CR.CriminalRecord == null || CR.CriminalRecord.Length == 0)
        {
          profile.CriminalRecords = null;
          profile.NoRecordsFound = String.Format(
            "No criminal records were found for {0} in {1}.</br>" +
            "To check for criminal records in all states, try our Nationwide Criminal Check.",
            profile.Name.FullName, GeneralPurposeCache.Singleton.StateByAbbr(query.State).StateName);
        }
      }
      else if (strTypeCode.ToUpper().Equals("NCC"))
      {
        var CR = profile.CriminalRecords;
        if (CR == null || CR.CriminalRecord == null || CR.CriminalRecord.Length == 0)
        {
          profile.CriminalRecords = null;
          profile.NoRecordsFound = String.Format(
            "No criminal records were found for {0}.",
            profile.Name.FullName);
        }
      }
      var json = JsonObjectSerializer.Serialize(profile);
      var html = profile.Render();
      var compressedJson = CompressionHelper.CompressString(json);
      var compressedHtml = CompressionHelper.CompressString(html);
      var reportCreationMsecs = (int)DateTime.Now.Subtract(dtStart).TotalMilliseconds;
      int intReportID = 0;

      using (var db = new AciXDB())
      {
        intReportID = db.spReportCreate(
          intSiteID: context.SiteID,
          intUserID: context.AuthorizedUserID,
          intOrderID: intOrderID,
          intQueryID: intQueryID,
          strState: query.State,
          strProfileID: strProfileID,
          strReportTypeCode: reportType.TypeCode,
          intJsonLen: json.Length,
          tCompressedJson: compressedJson,
          intHtmlLen: html.Length,
          tCompressedHtml: compressedHtml,
          intReportCreationMsecs: reportCreationMsecs);

        /*
         * This is a little hacky.  This should be done in the 
         * Order entity in the Checkout() method.  Unfortunately, 
         * at present, the process of "purchasing" a report using
         * a credit bypasses the Checkout() process and goes straight
         * to creating a report.  Hence, we update the cart with the 
         * OrderID here instead.
         */
        db.spCartUpdate(
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          intOrderID: intOrderID);
      }

      var order = Order.Get(context, intOrderID);

      var fulfillmentCli = new IwsFulfillmentClient(context);
      fulfillmentCli.DebitOrder(
        order: order,
        orderItem: order.Items[0],
        strProfileID: strProfileID,
        strFirstName: query.FirstName,
        strMiddleInitial: query.MiddleInitial,
        strLastName: query.LastName,
        strState: query.State);

      return Get(context, intReportID);
    }

    public static int[] ValidateReportAccess(CallContext context, int[] intReportIDs)
    {
      using (var db = new AciXDB())
      {
        return db.spReportValidateAccess(
          intAuthorizedUserID: context.AuthorizedUserID,
          intSiteID: context.SiteID,
          intKeys: intReportIDs);
      }
    }

  }
}
