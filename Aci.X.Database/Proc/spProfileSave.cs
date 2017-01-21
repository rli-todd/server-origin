using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spProfileSave : MyStoredProc
  {
    public spProfileSave(DbConnection conn)
      : base(strProcName: "spProfileSave", conn: conn)
    {
    }

    public void Execute(string strProfileID, string strProfileAttributes, byte[] tCompressedJson, int intDurationMsecs)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@ProfileID", strProfileID);
      Parameters.AddWithValue("@ProfileAttributes", strProfileAttributes);
      Parameters.AddWithValue("@CompressedJson", tCompressedJson);
      Parameters.AddWithValue("@DurationMsecs", intDurationMsecs);
      ExecuteNonQuery();
    }
  }
}
