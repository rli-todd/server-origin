using System.Data;

namespace Aci.X.Database
{
  public class StringTable : DataTable
  {
    public StringTable()
    {
      Columns.Add("String", typeof(string));
    }

    public StringTable(string[] strings)
    {
      Columns.Add("String", typeof(string));
      Add(strings);
    }

    public void Add(string[] strings)
    {
      if (strings != null)
      {
        foreach (string s in strings)
        {
          Add(s);
        }
      }
    }

    public void Add(string s)
    {
      DataRow row = this.NewRow();
      row["String"] = s;
      Rows.Add(row);
    }
  }
}
