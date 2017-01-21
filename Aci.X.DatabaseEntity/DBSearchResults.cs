using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSearchResults : MySqlResult
  {
    public int QueryID;
    public short SearchType;
    public int FirstNameID;
    public int LastNameID;
    public string MiddleInitial;
    public string State;
    private string DirectoryType;
    public int DirectoryID;
    public int VisitID;
    public int NumResults;
    public int QueryDurationMsecs;
    public string ApiSource;
    public DateTime DateCreated;
    public DateTime DateCached;
    public byte[] CompressedResults;
    public bool ResultsAreEmpty;
    public DateTime DateConsumed;
    public int FileSize;
    public int RobotID;
    public int FullNameHits;
    public bool Minimized;

    public override void Read()
    {
      QueryID = Value<int>("QueryID");
      SearchType = Value<short?>("SearchType") ?? 0;
      FirstNameID = Value<int?>("FirstNameID") ?? 0;
      LastNameID = Value<int?>("LastNameID") ?? 0;
      MiddleInitial = Value<string>("MiddleInitial");
      State = Value<string>("State");
      DirectoryType = Value<string>("DirectoryType");
      DirectoryID = Value<int?>("DirectoryID") ?? 0;
      VisitID = Value<int?>("VisitID") ?? 0;
      NumResults = Value<short?>("NumResults") ?? 0;
      QueryDurationMsecs = Value<int?>("QueryDurationMsecs") ?? 0;
      ApiSource = Value<string>("ApiSource");
      FileSize = Value<int?>("FileSize") ?? 0;
      DateCreated = Value<DateTime?>("DateCreated") ?? DateTime.MinValue;
      DateCached = Value<DateTime?>("DateCached") ?? DateTime.MinValue;
      DateConsumed = Value<DateTime?>("DateConsumed") ?? DateTime.MinValue;
      CompressedResults = Value<byte[]>("CompressedResults");
      ResultsAreEmpty = Value<bool?>("ResultsAreEmpty") ?? false;
      RobotID = Value<int?>("RobotID") ?? 0;
      FullNameHits = Value<int?>("FullNameHits") ?? 0;
      Minimized = Value<bool?>("Minimized") ?? false;
    }
  }
}
