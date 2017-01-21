using System.Data;

namespace Aci.X.Database
{
  public class IDTable : DataTable
  {
    public IDTable()
    {
      Columns.Add("ID", typeof(int));
    }

    public IDTable(int[] intIDs)
    {
      Columns.Add("ID", typeof(int));
      Add(intIDs);
    }

    public IDTable(byte[] tIDs)
    {
      Columns.Add("ID", typeof(int));
      Add(tIDs);
    }

    public void Add(int[] intIDs)
    {
      if (intIDs != null)
      {
        foreach (int intID in intIDs)
        {
          Add(intID);
        }
      }
    }

    public void Add(byte[] tIDs)
    {
      if (tIDs != null)
      {
        foreach (byte tID in tIDs)
        {
          Add(tID);
        }
      }
    }

    public void Add(int intID)
    {
      DataRow row = this.NewRow();
      row["ID"] = intID;
      Rows.Add(row);
    }
  }
}
