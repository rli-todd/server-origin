using System.Data.Common;

using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserCreate : MyStoredProc
  {
    public spUserCreate(DbConnection conn)
      : base(strProcName: "spUserCreate", conn: conn)
    {
    }
    public DBUser Execute(
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
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@ExternalID", intExternalID);
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@EmailAddress", strEmailAddress);
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@MiddleName", strMiddleName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@HasAcceptedUserAgreement", boolHasAcceptedUserAgreement);
      Parameters.AddWithValue("@IsBackofficeReader", boolIsBackofficeReader);
      Parameters.AddWithValue("@IwsUserToken", strIwsUserToken);
      Parameters.AddWithValue("@StorefrontUserToken", strStorefrontUserToken);
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBUser[] users = reader.GetResults<DBUser>();
        return users[0];
      }
    }
  }
}



