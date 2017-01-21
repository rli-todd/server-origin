using System.Data;

namespace Aci.X.Database
{
  public class CompositeKeyTable : DataTable
  {
    public CompositeKeyTable()
    {
      Columns.Add("KeyPart1", typeof(int));
      Columns.Add("KeyPart2", typeof(int));
    }

    public CompositeKeyTable(ulong[] ulCompositeKeys)
    {
      Columns.Add("KeyPart1", typeof(int));
      Columns.Add("KeyPart2", typeof(int));
      Add(ulCompositeKeys);
    }

    public void Add(ulong[] ulCompositeKeys)
    {
      if (ulCompositeKeys != null)
      {
        foreach (ulong ulCompositeKey in ulCompositeKeys)
        {
          Add(ulCompositeKey);
        }
      }
    }

    public void Add(ulong ulCompositeKey)
    {
      DataRow row = this.NewRow();
      row["KeyPart1"] = (int)(ulCompositeKey & 0xffffffff);
      row["KeyPart2"] = (int)(ulCompositeKey >> 32);
      Rows.Add(row);

    }
  }
}
