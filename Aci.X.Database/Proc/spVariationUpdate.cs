using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Solishine.CommonLib;
using Aci.X.ClientLib;

namespace Aci.X.Database
{
  public class spVariationUpdate : MyStoredProc
  {
    public spVariationUpdate(DbConnection conn)
      : base(strProcName: "spVariationUpdate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@VariationID", System.Data.SqlDbType.Int);
      Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
      Parameters.Add("@MultirowPrefix", System.Data.SqlDbType.VarChar);
      Parameters.Add("@MultirowSuffix", System.Data.SqlDbType.VarChar);
      Parameters.Add("@MultirowDelimiter", System.Data.SqlDbType.VarChar);
      Parameters.Add("@HeaderTemplate", System.Data.SqlDbType.VarChar);
      Parameters.Add("@BodyTemplate", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ViewName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ViewFieldNames", System.Data.SqlDbType.VarChar);
      Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit);
    }

    public void Execute(
      int intAuthorizedUserID,
      int intVariationID,
      string strDescription,
      string strMultirowPrefix,
      string strMultirowSuffix,
      string strMultirowDelimiter,
      string strHeaderTemplate,
      string strBodyTemplate,
      string strViewName,
      string strViewFieldNames,
      bool isEnabled)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@VariationID"].Value = intVariationID;
      Parameters["@Description"].Value = strDescription;
      Parameters["@MultirowPrefix"].Value = strMultirowPrefix;
      Parameters["@MultirowSuffix"].Value = strMultirowSuffix;
      Parameters["@MultirowDelimiter"].Value = strMultirowDelimiter;
      Parameters["@HeaderTemplate"].Value = strHeaderTemplate;
      Parameters["@BodyTemplate"].Value = strBodyTemplate;
      Parameters["@ViewName"].Value = strViewName;
      Parameters["@ViewFieldNames"].Value = strViewFieldNames;
      Parameters["@IsEnabled"].Value = isEnabled;
      base.ExecuteNonQuery();
    }
  }
}



