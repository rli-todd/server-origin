using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsGet : MyStoredProc
  {
    public spSearchResultsGet(DbConnection conn)
      : base(strProcName: "spSearchResultsGet", conn: conn)
    {
    }

    public DBSearchResults[] Execute(
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
      if (intTimeoutSecs < 0)
      {
        throw new TimeoutException("Timeout < 0");
      }
      CommandTimeoutSecs = intTimeoutSecs;
      Parameters.Clear();
      Parameters.AddWithValue("@SearchType", shSearchType);
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@MiddleName", strMiddleName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@State", strState);
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@FirstNameID", intFirstNameID);
      Parameters.AddWithValue("@LastNameID", intLastNameID);
      Parameters.AddWithValue("@QueryID", intQueryID);
      Parameters.AddWithValue("@IgnoreDateCachedBefore1111", boolIgnoreDateCachedBefore1111);

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBSearchResults>();
      }
    }
  }
}
