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
  public class spVariationCreate : MyStoredProc
  {
    public spVariationCreate(DbConnection conn)
      : base(strProcName: "spVariationCreate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionID", System.Data.SqlDbType.Int);
      Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
      Parameters.Add("@MultirowPrefix", System.Data.SqlDbType.VarChar);
      Parameters.Add("@MultirowSuffix", System.Data.SqlDbType.VarChar);
      Parameters.Add("@MultirowDelimiter", System.Data.SqlDbType.VarChar);
      Parameters.Add("@HeaderTemplate", System.Data.SqlDbType.VarChar);
      Parameters.Add("@BodyTemplate", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ViewName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ViewFieldNames", System.Data.SqlDbType.VarChar);
      Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit);
      Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
      Parameters["@ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;
    }

    public int Execute(
      int intAuthorizedUserID,
      int intSectionID, 
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
      Parameters["@SectionID"].Value = intSectionID;
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
      return (int)Parameters["@ReturnValue"].Value;
    }
  }
}



