using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSectionValues : MySqlResult
  {
    public int SectionID;
    public int VariationID;
    public object[] Values;

    public override void Read()
    {
      SectionID = Value<int>("SectionID");
      VariationID = Value<int>("VariationID");
      // All results sets should have SectionID and VariationID as the first two columns.
      // We will skip these in getting the remaining columns
      Values = new object[Reader.ColumnNames.Length-2];
      for (int idxCol=2; idxCol<Reader.ColumnNames.Length; ++idxCol)
      {
        object o = Reader.ValueAtIndex(idxCol);
        Values[idxCol - 2] = o is DBNull ? null : o;
      }
    }
  }
}
