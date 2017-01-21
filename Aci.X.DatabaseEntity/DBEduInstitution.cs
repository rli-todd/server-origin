using System;
using System.Collections.Generic;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBEduInstitution : MySqlResult
  {
    public int InstitutionID;
    public string InstitutionName;
    public string Synonym;
    public string Address;
    public string City;
    public string State;
    public string Zip;
    public string Phone;
    public string OPEID;
    public int IPEDS_UnitID;
    public string WebAddress;
    public string SearchTerm;
    public List<DBEduCampus> Campuses;

    public override void Read()
    {
      InstitutionID = Value<int?>("Institution_ID") ?? 0;
      InstitutionName = Value<string>("Institution_Name");
      Synonym = Value<string>("Synonym");
      Address = Value<string>("Institution_Address");
      City = Value<string>("Institution_City");
      State = Value<string>("Institution_State");
      Zip = Value<string>("Institution_Zip");
      Phone = Value<string>("Institution_Phone");
      OPEID = Value<string>("Institution_OPEID");
      IPEDS_UnitID = Value<int>("Institution_IPEDS_UnitID");
      WebAddress = Value<string>("Institution_Web_Address");
      SearchTerm = Value<string>("SearchTerm");
    }
  }

  public class DBEduCampus : MySqlResult
  {
    public int InstitutionID;
    public int CampusID;
    public string CampusName;
    public string Address;
    public string City;
    public string State;
    public string Zip;
    public List<DBEduAccreditation> Accreditations;

    public override void Read()
    {
      InstitutionID = Value<int>("Institution_ID");
      CampusID = Value<short>("Campus_ID");
      CampusName = Value<string>("Campus_Name");
      Address = Value<string>("Campus_Address");
      City = Value<string>("Campus_City");
      State = Value<string>("Campus_State");
      Zip = Value<string>("Campus_Zip");
    }
  }

  public class DBEduAccreditation : MySqlResult
  {
    public int InstitutionID;
    public int CampusID;
    public string Narrative;

    public override void Read()
    {
      InstitutionID = Value<int>("Institution_ID");
      CampusID = Value<short>("Campus_ID");
      Narrative = Value<string>("Narrative");

    }
  }
}
