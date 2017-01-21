using System;

using System.Data;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserUpdate : MyStoredProc
  {
    public spUserUpdate(DbConnection conn)
      : base(strProcName: "spUserUpdate", conn: conn)
    {
    }

    public DBUser Execute(
      int intVisitID,
      int? intSiteID = null,
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
      string strCardExpiry = null,
      string strCardholderName = null,
      string strCardAddress = null,
      string strCardCity = null,
      string strCardState = null,
      string strCardCountry = null,
      string strCardZip = null,
      DateTime? dtCardLastModified = null, 
      bool? boolUpdateDateLastAuthenticated = null)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@ExternalUserID", intExternalUserID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@IwsUserToken", strIwsUserToken);
      Parameters.AddWithValue("@StorefrontUserToken", strStorefrontUserToken);
      Parameters.AddWithValue("@EmailAddress", strEmailAddress);
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@MiddleName", strMiddleName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@HasAcceptedUserAgreement", boolHasAcceptedUserAgreement);
      Parameters.AddWithValue("@HasValidPaymentMethod", boolHasValidPaymentMethod);
      Parameters.AddWithValue("@CardCVV", strCardCVV);
      Parameters.AddWithValue("@CardHash", tCardHash);
      Parameters.AddWithValue("@CardLast4", strCardLast4);
      Parameters.AddWithValue("@CardExpiry", strCardExpiry);
      Parameters.AddWithValue("@CardholderName", strCardholderName);
      Parameters.AddWithValue("@CardAddress", strCardAddress);
      Parameters.AddWithValue("@CardCity", strCardCity);
      Parameters.AddWithValue("@CardState", strCardState);
      Parameters.AddWithValue("@CardCountry", strCardCountry);
      Parameters.AddWithValue("@CardZip", strCardZip);
      Parameters.AddWithValue("@CardLastModified", dtCardLastModified);
      Parameters.AddWithValue("@UpdateDateLastAuthenticated", boolUpdateDateLastAuthenticated);
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBUser[] users = reader.GetResults<DBUser>();
        return users.Length > 0 ? users[0] : null;
      }
    }
  }
}



