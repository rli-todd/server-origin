/**********************************************************************************
**
** The classes in this source file were generated by Aci.X.DalGen 
** on 2016-12-31 at 12:55:12 and should not be modified directly.
**
***********************************************************************************/
using System.Data.SqlClient;
using System;
using Aci.X.DatabaseEntity;
using System.Collections.Generic;
using Aci.X.ClientLib;
namespace Aci.X.Database
{
  /// <summary
  /// This class auto-generated by: Aci.X.DalGen
  /// </summary
  public partial class AciXDB : DalSqlDb
  {
    public AciXDB(SqlConnection conn=null, bool boolUseTransaction=false, int intCommandTimeoutSecs=0) : base(conn, boolUseTransaction, intCommandTimeoutSecs)
    { }
    /// <summary>
    /// AciXDB.spApiLog
    /// </summary>
    public void spApiLog(
      int intSiteID,
      int intVisitID,
      int intUserID,
      int intClientIP,
      int intUserIP,
      string strRequestMethod,
      string strRequestBody,
      string strResponseJson,
      short shHttpStatusCode,
      int intDurationMsecs,
      string strServerHostname,
      string strPathAndQuery,
      string strRequestTemplate,
      string strUserAgent,
      string strErrorType,
      string strErrorMessage,
      ref short shServerID,
      ref int intApiLogPathID,
      ref short shApiLogTemplateID,
      ref int intUserAgentID)
    {
      new spApiLog(_conn)
        .Execute(
          intSiteID: intSiteID,
          intVisitID: intVisitID,
          intUserID: intUserID,
          intClientIP: intClientIP,
          intUserIP: intUserIP,
          strRequestMethod: strRequestMethod,
          strRequestBody: strRequestBody,
          strResponseJson: strResponseJson,
          shHttpStatusCode: shHttpStatusCode,
          intDurationMsecs: intDurationMsecs,
          strServerHostname: strServerHostname,
          strPathAndQuery: strPathAndQuery,
          strRequestTemplate: strRequestTemplate,
          strUserAgent: strUserAgent,
          strErrorType: strErrorType,
          strErrorMessage: strErrorMessage,
          shServerID: ref shServerID,
          intApiLogPathID: ref intApiLogPathID,
          shApiLogTemplateID: ref shApiLogTemplateID,
          intUserAgentID: ref intUserAgentID);
    }

    /// <summary>
    /// AciXDB.spCartUpdate
    /// </summary>
    public void spCartUpdate(
      int intSiteID,
      int intVisitID,
      int? intOrderID=null,
      int? intProductID=null,
      int? intQueryID=null,
      string strProfileID=null,
      string strStateAbbr=null)
    {
      new spCartUpdate(_conn)
        .Execute(
          intSiteID: intSiteID,
          intVisitID: intVisitID,
          intOrderID: intOrderID,
          intProductID: intProductID,
          intQueryID: intQueryID,
          strProfileID: strProfileID,
          strStateAbbr: strStateAbbr);
    }

    /// <summary>
    /// AciXDB.spOrderCreate
    /// </summary>
    public int spOrderCreate(
      int intSiteID,
      string strOrderXml,
      int? intQueryID=null,
      string strProfileID=null)
    {
      return new spOrderCreate(_conn)
        .Execute(
          intSiteID: intSiteID,
          strOrderXml: strOrderXml,
          intQueryID: intQueryID,
          strProfileID: strProfileID);
    }

    /// <summary>
    /// AciXDB.spOrderGet
    /// </summary>
    public DBOrder[] spOrderGet(
      int intSiteID,
      int[] intKeys)
    {
      return new spOrderGet(_conn)
        .Execute(
          intSiteID: intSiteID,
          intKeys: intKeys);
    }

    /// <summary>
    /// AciXDB.spOrderSearch
    /// </summary>
    public int[] spOrderSearch(
      int intSiteID,
      int intUserID=0,
      int? intSelectedUserID=null,
      string strFirstName=null,
      string strLastName=null,
      string strState=null,
      string strProfileID=null,
      int[] intExternalOrderIDs=null)
    {
      return new spOrderSearch(_conn)
        .Execute(
          intSiteID: intSiteID,
          intUserID: intUserID,
          intSelectedUserID: intSelectedUserID,
          strFirstName: strFirstName,
          strLastName: strLastName,
          strState: strState,
          strProfileID: strProfileID,
          intExternalOrderIDs: intExternalOrderIDs);
    }

    /// <summary>
    /// AciXDB.spOrderValidateAccess
    /// </summary>
    public int[] spOrderValidateAccess(
      int intAuthorizedUserID,
      int intSiteID,
      int[] intKeys)
    {
      return new spOrderValidateAccess(_conn)
        .Execute(
          intAuthorizedUserID: intAuthorizedUserID,
          intSiteID: intSiteID,
          intKeys: intKeys);
    }

    /// <summary>
    /// AciXDB.spPageGetContent
    /// </summary>
    public DBSectionTemplate[] spPageGetContent(
      string strPageCode,
      string strWhereClause)
    {
      return new spPageGetContent(_conn)
        .Execute(
          strPageCode: strPageCode,
          strWhereClause: strWhereClause);
    }

    /// <summary>
    /// AciXDB.spCatalogGet
    /// </summary>
    public DBCatalog spCatalogGet(int intSiteID)
    {
      return new spCatalogGet(_conn)
        .Execute(intSiteID: intSiteID);
    }

    /// <summary>
    /// AciXDB.spProductInitAll
    /// </summary>
    public void spProductInitAll()
    {
      new spProductInitAll(_conn)
        .Execute();
    }

    /// <summary>
    /// AciXDB.spProductRefresh
    /// </summary>
    public void spProductRefresh(
      int intSiteID,
      string strExternalProductsXml)
    {
      new spProductRefresh(_conn)
        .Execute(
          intSiteID: intSiteID,
          strExternalProductsXml: strExternalProductsXml);
    }

    /// <summary>
    /// AciXDB.spQueryGet
    /// </summary>
    public DBQuery[] spQueryGet(int intQueryID)
    {
      return new spQueryGet(_conn)
        .Execute(intQueryID: intQueryID);
    }

    /// <summary>
    /// AciXDB.spReportValidateAccess
    /// </summary>
    public int[] spReportValidateAccess(
      int intAuthorizedUserID,
      int intSiteID,
      int[] intKeys)
    {
      return new spReportValidateAccess(_conn)
        .Execute(
          intAuthorizedUserID: intAuthorizedUserID,
          intSiteID: intSiteID,
          intKeys: intKeys);
    }

    /// <summary>
    /// AciXDB.spSiteGet
    /// </summary>
    public DBSite[] spSiteGet(byte[] tSiteIDs)
    {
      return new spSiteGet(_conn)
        .Execute(tSiteIDs: tSiteIDs);
    }

    /// <summary>
    /// AciXDB.spReportCreate
    /// </summary>
    public int spReportCreate(
      int intSiteID,
      int intUserID,
      int intOrderID,
      int? intQueryID,
      string strProfileID,
      string strState,
      string strReportTypeCode,
      int intJsonLen,
      byte[] tCompressedJson,
      int intHtmlLen,
      byte[] tCompressedHtml,
      int intReportCreationMsecs)
    {
      return new spReportCreate(_conn)
        .Execute(
          intSiteID: intSiteID,
          intUserID: intUserID,
          intOrderID: intOrderID,
          intQueryID: intQueryID,
          strProfileID: strProfileID,
          strState: strState,
          strReportTypeCode: strReportTypeCode,
          intJsonLen: intJsonLen,
          tCompressedJson: tCompressedJson,
          intHtmlLen: intHtmlLen,
          tCompressedHtml: tCompressedHtml,
          intReportCreationMsecs: intReportCreationMsecs);
    }

    /// <summary>
    /// AciXDB.spReportGet
    /// </summary>
    public DBReport[] spReportGet(
      int intSiteID,
      int[] intKeys)
    {
      return new spReportGet(_conn)
        .Execute(
          intSiteID: intSiteID,
          intKeys: intKeys);
    }

    /// <summary>
    /// AciXDB.spReportSearch
    /// </summary>
    public int[] spReportSearch(
      int intSiteID,
      int intUserID,
      string strFirstName=null,
      string strLastName=null,
      string strState=null,
      string strProfileID=null,
      int? intOrderID=null)
    {
      return new spReportSearch(_conn)
        .Execute(
          intSiteID: intSiteID,
          intUserID: intUserID,
          strFirstName: strFirstName,
          strLastName: strLastName,
          strState: strState,
          strProfileID: strProfileID,
          intOrderID: intOrderID);
    }

    /// <summary>
    /// AciXDB.spReportTypeGet
    /// </summary>
    public DBReportType[] spReportTypeGet(
      int intSiteID,
      string[] intKeys)
    {
      return new spReportTypeGet(_conn)
        .Execute(
          intSiteID: intSiteID,
          intKeys: intKeys);
    }

    /// <summary>
    /// AciXDB.spReportTypeSearch
    /// </summary>
    public string[] spReportTypeSearch(int intSiteID)
    {
      return new spReportTypeSearch(_conn)
        .Execute(intSiteID: intSiteID);
    }

    /// <summary>
    /// AciXDB.spSkuGet
    /// </summary>
    public DBSku[] spSkuGet(
      byte tSiteID,
      int[] intSkuIDs)
    {
      return new spSkuGet(_conn)
        .Execute(
          tSiteID: tSiteID,
          intSkuIDs: intSkuIDs);
    }

    /// <summary>
    /// AciXDB.spSkuSearch
    /// </summary>
    public int[] spSkuSearch(
      int intSiteID,
      int[] intExternalSkuIDs)
    {
      return new spSkuSearch(_conn)
        .Execute(
          intSiteID: intSiteID,
          intExternalSkuIDs: intExternalSkuIDs);
    }

    /// <summary>
    /// AciXDB.spUserCreate
    /// </summary>
    public DBUser spUserCreate(
      int intSiteID,
      int intExternalID,
      int intVisitID,
      string strEmailAddress,
      string strFirstName,
      string strMiddleName,
      string strLastName,
      bool boolHasAcceptedUserAgreement,
      bool boolIsBackofficeReader,
      string strIwsUserToken,
      string strStorefrontUserToken)
    {
      return new spUserCreate(_conn)
        .Execute(
          intSiteID: intSiteID,
          intExternalID: intExternalID,
          intVisitID: intVisitID,
          strEmailAddress: strEmailAddress,
          strFirstName: strFirstName,
          strMiddleName: strMiddleName,
          strLastName: strLastName,
          boolHasAcceptedUserAgreement: boolHasAcceptedUserAgreement,
          boolIsBackofficeReader: boolIsBackofficeReader,
          strIwsUserToken: strIwsUserToken,
          strStorefrontUserToken: strStorefrontUserToken);
    }

    /// <summary>
    /// AciXDB.spUserGet
    /// </summary>
    public DBUser[] spUserGet(
      byte tSiteID,
      int[] intUserIDs)
    {
      return new spUserGet(_conn)
        .Execute(
          tSiteID: tSiteID,
          intUserIDs: intUserIDs);
    }

    /// <summary>
    /// AciXDB.spUserSearch
    /// </summary>
    public int spUserSearch(
      int intSiteID,
      string strEmailAddress)
    {
      return new spUserSearch(_conn)
        .Execute(
          intSiteID: intSiteID,
          strEmailAddress: strEmailAddress);
    }

    /// <summary>
    /// AciXDB.spUserUpdate
    /// </summary>
    public DBUser spUserUpdate(
      int intVisitID,
      int? intSiteID=null,
      int? intExternalUserID=null,
      int? intUserID=null,
      string strIwsUserToken=null,
      string strStorefrontUserToken=null,
      string strEmailAddress=null,
      string strFirstName=null,
      string strMiddleName=null,
      string strLastName=null,
      bool? boolHasAcceptedUserAgreement=null,
      bool? boolHasValidPaymentMethod=null,
      string strCardCVV=null,
      byte[] tCardHash=null,
      string strCardLast4=null,
      string strCardExpiry=null,
      string strCardholderName=null,
      string strCardAddress=null,
      string strCardCity=null,
      string strCardState=null,
      string strCardCountry=null,
      string strCardZip=null,
      DateTime? dtCardLastModified=null,
      bool? boolUpdateDateLastAuthenticated=null)
    {
      return new spUserUpdate(_conn)
        .Execute(
          intVisitID: intVisitID,
          intSiteID: intSiteID,
          intExternalUserID: intExternalUserID,
          intUserID: intUserID,
          strIwsUserToken: strIwsUserToken,
          strStorefrontUserToken: strStorefrontUserToken,
          strEmailAddress: strEmailAddress,
          strFirstName: strFirstName,
          strMiddleName: strMiddleName,
          strLastName: strLastName,
          boolHasAcceptedUserAgreement: boolHasAcceptedUserAgreement,
          boolHasValidPaymentMethod: boolHasValidPaymentMethod,
          strCardCVV: strCardCVV,
          tCardHash: tCardHash,
          strCardLast4: strCardLast4,
          strCardExpiry: strCardExpiry,
          strCardholderName: strCardholderName,
          strCardAddress: strCardAddress,
          strCardCity: strCardCity,
          strCardState: strCardState,
          strCardCountry: strCardCountry,
          strCardZip: strCardZip,
          dtCardLastModified: dtCardLastModified,
          boolUpdateDateLastAuthenticated: boolUpdateDateLastAuthenticated);
    }

    /// <summary>
    /// AciXDB.spVisitCreate
    /// </summary>
    public DBVisit spVisitCreate(
      string strUserAgent,
      string strIpAddress,
      string strAcceptLanguage,
      string strRefererUrl,
      string strLandingUrl,
      string strWebServerName,
      string strApiServerName,
      Guid? userGuid=null,
      bool isReadOnly=false)
    {
      return new spVisitCreate(_conn)
        .Execute(
          strUserAgent: strUserAgent,
          strIpAddress: strIpAddress,
          strAcceptLanguage: strAcceptLanguage,
          strRefererUrl: strRefererUrl,
          strLandingUrl: strLandingUrl,
          strWebServerName: strWebServerName,
          strApiServerName: strApiServerName,
          userGuid: userGuid,
          isReadOnly: isReadOnly);
    }

    /// <summary>
    /// AciXDB.spVisitGetByToken
    /// </summary>
    public DBVisit spVisitGetByToken(string strVisitGuid)
    {
      return new spVisitGetByToken(_conn)
        .Execute(strVisitGuid: strVisitGuid);
    }

    /// <summary>
    /// AciXDB.spVisitGetV2
    /// </summary>
    public DBVisit[] spVisitGetV2(
      byte tSiteID,
      int[] intUserIDs)
    {
      return new spVisitGetV2(_conn)
        .Execute(
          tSiteID: tSiteID,
          intUserIDs: intUserIDs);
    }

    /// <summary>
    /// AciXDB.spVisitSetPage
    /// </summary>
    public void spVisitSetPage(
      int intVisitID,
      string strPageCode)
    {
      new spVisitSetPage(_conn)
        .Execute(
          intVisitID: intVisitID,
          strPageCode: strPageCode);
    }

    /// <summary>
    /// AciXDB.spVisitUpdateIwsUserToken
    /// </summary>
    public DBVisit spVisitUpdateIwsUserToken(
      int intSiteID,
      int intVisitID,
      int intUserID,
      string strIwsUserToken,
      int intIwsUserID,
      string strStorefrontUserToken)
    {
      return new spVisitUpdateIwsUserToken(_conn)
        .Execute(
          intSiteID: intSiteID,
          intVisitID: intVisitID,
          intUserID: intUserID,
          strIwsUserToken: strIwsUserToken,
          intIwsUserID: intIwsUserID,
          strStorefrontUserToken: strStorefrontUserToken);
    }

  }

  /// <summary
  /// This class auto-generated by: Aci.X.DalGen
  /// </summary
  public partial class GeoDB : DalSqlDb
  {
    public GeoDB(SqlConnection conn=null, bool boolUseTransaction=false, int intCommandTimeoutSecs=0) : base(conn, boolUseTransaction, intCommandTimeoutSecs)
    { }
    /// <summary>
    /// GeoDB.spEduInstitutionAssociate
    /// </summary>
    public int spEduInstitutionAssociate(
      int intInstutitionID,
      string strSearchTerm)
    {
      return new spEduInstitutionAssociate(_conn)
        .Execute(
          intInstutitionID: intInstutitionID,
          strSearchTerm: strSearchTerm);
    }

    /// <summary>
    /// GeoDB.spEduInstitutionGet
    /// </summary>
    public DBEduInstitution spEduInstitutionGet(int intInstutitionID)
    {
      return new spEduInstitutionGet(_conn)
        .Execute(intInstutitionID: intInstutitionID);
    }

    /// <summary>
    /// GeoDB.spEduInstitutionSearch
    /// </summary>
    public Dictionary<string,List<DBEduInstitution>> spEduInstitutionSearch(
      string strSearchTerm,
      int intMaxResultsPerSchool=1)
    {
      return new spEduInstitutionSearch(_conn)
        .Execute(
          strSearchTerm: strSearchTerm,
          intMaxResultsPerSchool: intMaxResultsPerSchool);
    }

    /// <summary>
    /// GeoDB.spGeoStateGet
    /// </summary>
    public GeoState[] spGeoStateGet()
    {
      return new spGeoStateGet(_conn)
        .Execute();
    }

    /// <summary>
    /// GeoDB.spPostalLookup
    /// </summary>
    public DBPostal spPostalLookup(string strPostalCode)
    {
      return new spPostalLookup(_conn)
        .Execute(strPostalCode: strPostalCode);
    }

  }

  /// <summary
  /// This class auto-generated by: Aci.X.DalGen
  /// </summary
  public partial class ProfileDB : DalSqlDb
  {
    public ProfileDB(SqlConnection conn=null, bool boolUseTransaction=false, int intCommandTimeoutSecs=0) : base(conn, boolUseTransaction, intCommandTimeoutSecs)
    { }
    /// <summary>
    /// ProfileDB.spOptOutGetUnrefreshed
    /// </summary>
    public DBOptOut[] spOptOutGetUnrefreshed()
    {
      return new spOptOutGetUnrefreshed(_conn)
        .Execute();
    }

    /// <summary>
    /// ProfileDB.spOptOutSetRefreshDate
    /// </summary>
    public void spOptOutSetRefreshDate(
      string strFirstName,
      string strLastName,
      string strState)
    {
      new spOptOutSetRefreshDate(_conn)
        .Execute(
          strFirstName: strFirstName,
          strLastName: strLastName,
          strState: strState);
    }

    /// <summary>
    /// ProfileDB.spProfileDelete
    /// </summary>
    public void spProfileDelete(string strProfileID)
    {
      new spProfileDelete(_conn)
        .Execute(strProfileID: strProfileID);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsDelete
    /// </summary>
    public void spSearchResultsDelete(int intQueryID)
    {
      new spSearchResultsDelete(_conn)
        .Execute(intQueryID: intQueryID);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsSaveMinimized
    /// </summary>
    public void spSearchResultsSaveMinimized(
      int intQueryID,
      byte[] tCompressedResults)
    {
      new spSearchResultsSaveMinimized(_conn)
        .Execute(
          intQueryID: intQueryID,
          tCompressedResults: tCompressedResults);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsSetEducationIsNull
    /// </summary>
    public void spSearchResultsSetEducationIsNull(
      int intQueryID,
      bool boolEducationIsNull)
    {
      new spSearchResultsSetEducationIsNull(_conn)
        .Execute(
          intQueryID: intQueryID,
          boolEducationIsNull: boolEducationIsNull);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsSetPersonIDs
    /// </summary>
    public void spSearchResultsSetPersonIDs(
      int intQueryID,
      string strListPersonIDs)
    {
      new spSearchResultsSetPersonIDs(_conn)
        .Execute(
          intQueryID: intQueryID,
          strListPersonIDs: strListPersonIDs);
    }

    /// <summary>
    /// ProfileDB.spProfileSave
    /// </summary>
    public void spProfileSave(
      string strProfileID,
      string strProfileAttributes,
      byte[] tCompressedJson,
      int intDurationMsecs)
    {
      new spProfileSave(_conn)
        .Execute(
          strProfileID: strProfileID,
          strProfileAttributes: strProfileAttributes,
          tCompressedJson: tCompressedJson,
          intDurationMsecs: intDurationMsecs);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsExists
    /// </summary>
    public bool spSearchResultsExists(
      short shSearchType,
      string strFirstName,
      string strMiddleName,
      string strLastName,
      string strState)
    {
      return new spSearchResultsExists(_conn)
        .Execute(
          shSearchType: shSearchType,
          strFirstName: strFirstName,
          strMiddleName: strMiddleName,
          strLastName: strLastName,
          strState: strState);
    }

    /// <summary>
    /// ProfileDB.spProfileGet
    /// </summary>
    public DBProfile spProfileGet(
      string strProfileID,
      string strProfileAttributes)
    {
      return new spProfileGet(_conn)
        .Execute(
          strProfileID: strProfileID,
          strProfileAttributes: strProfileAttributes);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsSetFileSize
    /// </summary>
    public void spSearchResultsSetFileSize(
      int intQueryID,
      int intCompressedSize,
      DateTime? dtCached=null,
      bool boolDeleteOnly=false)
    {
      new spSearchResultsSetFileSize(_conn)
        .Execute(
          intQueryID: intQueryID,
          intCompressedSize: intCompressedSize,
          dtCached: dtCached,
          boolDeleteOnly: boolDeleteOnly);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsGet
    /// </summary>
    public DBSearchResults[] spSearchResultsGet(
      short? shSearchType=null,
      string strFirstName=null,
      string strMiddleName=null,
      string strLastName=null,
      string strState="",
      int? intVisitID=null,
      int intTimeoutSecs=30,
      int? intFirstNameID=null,
      int? intLastNameID=null,
      int? intQueryID=null,
      bool boolIgnoreDateCachedBefore1111=true)
    {
      return new spSearchResultsGet(_conn)
        .Execute(
          shSearchType: shSearchType,
          strFirstName: strFirstName,
          strMiddleName: strMiddleName,
          strLastName: strLastName,
          strState: strState,
          intVisitID: intVisitID,
          intTimeoutSecs: intTimeoutSecs,
          intFirstNameID: intFirstNameID,
          intLastNameID: intLastNameID,
          intQueryID: intQueryID,
          boolIgnoreDateCachedBefore1111: boolIgnoreDateCachedBefore1111);
    }

    /// <summary>
    /// ProfileDB.spSearchResultsSave
    /// </summary>
    public int spSearchResultsSave(
      short shSearchType,
      string strFirstName,
      string strMiddleName,
      string strLastName,
      string strState,
      int? intVisitID,
      int intNumResults,
      int? intQueryDurationMsecs,
      string strApiSource,
      bool boolResultsAreEmpty,
      byte[] tCompressedResults,
      int intFileSize,
      bool boolMinimized,
      out int intFullNameHits)
    {
      return new spSearchResultsSave(_conn)
        .Execute(
          shSearchType: shSearchType,
          strFirstName: strFirstName,
          strMiddleName: strMiddleName,
          strLastName: strLastName,
          strState: strState,
          intVisitID: intVisitID,
          intNumResults: intNumResults,
          intQueryDurationMsecs: intQueryDurationMsecs,
          strApiSource: strApiSource,
          boolResultsAreEmpty: boolResultsAreEmpty,
          tCompressedResults: tCompressedResults,
          intFileSize: intFileSize,
          boolMinimized: boolMinimized,
          intFullNameHits: out intFullNameHits);
    }

    /// <summary>
    /// ProfileDB.spSearchStatsGetAll2
    /// </summary>
    public DBSearchStats[] spSearchStatsGetAll2(
      string strFirstName,
      string strLastName,
      string strState,
      ref int? intFirstNameID,
      ref int? intLastNameID,
      int intTimeoutSecs=30)
    {
      return new spSearchStatsGetAll2(_conn)
        .Execute(
          strFirstName: strFirstName,
          strLastName: strLastName,
          strState: strState,
          intFirstNameID: ref intFirstNameID,
          intLastNameID: ref intLastNameID,
          intTimeoutSecs: intTimeoutSecs);
    }

  }

}
