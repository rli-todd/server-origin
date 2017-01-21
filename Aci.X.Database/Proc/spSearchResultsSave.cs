using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsSave : MyStoredProc
  {
    public spSearchResultsSave(DbConnection conn)
      : base(strProcName: "spSearchResultsSave", conn: conn)
    {
    }

    public int Execute(
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
      Parameters.AddWithValue("@SearchType", shSearchType);
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@MiddleName", strMiddleName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@State", strState);
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@NumResults", intNumResults);
      Parameters.AddWithValue("@QueryDurationMsecs", intQueryDurationMsecs);
      Parameters.AddWithValue("@ApiSource", strApiSource);
      Parameters.AddWithValue("@ResultsAreEmpty", boolResultsAreEmpty);
      Parameters.AddWithValue("@CompressedResults", tCompressedResults);
      Parameters.AddWithValue("@Minimized", boolMinimized);
      Parameters.AddWithValue("@FileSize", intFileSize);
      Parameters.Add(new SqlParameter()
      {
        ParameterName = "@QueryID",
        SqlDbType = System.Data.SqlDbType.Int,
        Direction = System.Data.ParameterDirection.ReturnValue
      });
      Parameters.Add(new SqlParameter()
      {
        ParameterName = "@FullNameHits",
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.InputOutput
      });

      ExecuteNonQuery();
      object oFullNameHits = Parameters["@FullNameHits"].Value;
      intFullNameHits = oFullNameHits is DBNull ? 0 : (int)oFullNameHits;
      return (int)Parameters["@QueryID"].Value;
    }
  }
}
