using System;
using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsSetFileSize : MyStoredProc
  {
    public spSearchResultsSetFileSize (DbConnection conn)
      : base(strProcName: "spSearchResultsSetFileSize", conn: conn)
    {
      Parameters.Add("@QueryID", System.Data.SqlDbType.Int);
      Parameters.Add("@CompressedSize", System.Data.SqlDbType.Int);
      Parameters.Add("@DateCached", System.Data.SqlDbType.DateTime);
      Parameters.Add("@DeleteOnly", System.Data.SqlDbType.Bit);
      this.CommandTimeoutSecs = 1;
    }
    public void Execute(int intQueryID, int intCompressedSize, DateTime? dtCached=null, bool boolDeleteOnly=false )
    {
      Parameters["@QueryID"].Value = intQueryID;
      Parameters["@CompressedSize"].Value = intCompressedSize;
      Parameters["@DateCached"].Value = dtCached;
      Parameters["@DeleteOnly"].Value = boolDeleteOnly;

      base.ExecuteNonQuery();
    }
  }
}
