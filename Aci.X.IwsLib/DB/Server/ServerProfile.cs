using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.IwsLib.DB.Cache;
using NLog;

namespace Aci.X.IwsLib.DB
{
  public class ServerProfile
  {
    private static GeoLocationCache _geoLocationCache;
    private static CompanyCache _companyCache;
    private static SchoolCache _schoolCache;
    private static FirstNameCache _firstNameCache;
    private static MiddleNameCache _middleNameCache;
    private static LastNameCache _lastNameCache;
    private static PersonCache _personCache;
    private static AliasCache _aliasCache;

    private static Logger _logger = LogManager.GetCurrentClassLogger();

    private static GeoLocationCache MyGeoLocationCache
    {
      get
      {
        if (_geoLocationCache == null)
          _geoLocationCache = new GeoLocationCache();
        return _geoLocationCache;
      }
    }

    private static CompanyCache MyCompanyCache
    {
      get 
      {
        if (_companyCache == null)
          _companyCache = new CompanyCache();
        return _companyCache;
      }
    }

    private static SchoolCache MySchoolCache
    {
      get
      {
        if (_schoolCache == null)
          _schoolCache = new SchoolCache();
        return _schoolCache;
      }
    }

    private static FirstNameCache MyFirstNameCache
    {
      get
      {
        if (_firstNameCache == null)
          _firstNameCache = new FirstNameCache();
        return _firstNameCache;
      }
    }
    
    private static MiddleNameCache MyMiddleNameCache
    {
      get
      {
        if (_middleNameCache == null)
          _middleNameCache = new MiddleNameCache();
        return _middleNameCache;
      }
    }

    private static LastNameCache MyLastNameCache
    {
      get
      {
        if (_lastNameCache == null)
          _lastNameCache = new LastNameCache();
        return _lastNameCache;
      }
    }

    private static PersonCache MyPersonCache
    {
      get
      {
        if (_personCache == null)
          _personCache = new PersonCache();
        return _personCache;
      }
    }

    private static AliasCache MyAliasCache
    {
      get
      {
        if (_aliasCache == null)
          _aliasCache = new AliasCache();
        return _aliasCache;
      }
    }

    public static Profile LoadFromDB(int intProfileID)
    {
      return null;
    }

    public static int UpdateDB(Profile profile)
    {
      List<int> listGeoLocationIDs = new List<int>();
      List<int> listCompanyIDs = new List<int>();
      List<int> listSchoolIDs = new List<int>();
      List<int> listAliasIDs = new List<int>();
      List<string> listRelativeIDs = new List<string>();

      if (profile.Addresses != null && profile.Addresses.Address != null)
      {
        foreach (Address address in profile.Addresses.Address)
        {
          string strKey = Text.NullCheck(address.City) + "|" + Text.NullCheck(address.State) + "|" + address.GetZip();
          listGeoLocationIDs.Add(MyGeoLocationCache.GetID(strKey));
        }
      }

      if (profile.Professional != null && profile.Professional.WorkHistory != null)
      {
        foreach (BusinessRecord company in profile.Professional.WorkHistory)
        {
          listCompanyIDs.Add(MyCompanyCache.GetID(Text.NullCheck(company.CompanyName)));
        }
      }

      if (profile.Education != null && profile.Education.School != null)
      {
        foreach (/*Text*/String school in profile.Education.School)
        {
          listSchoolIDs.Add(MySchoolCache.GetID(Text.NullCheck(school)));
        }
      }

      if (profile.Aliases != null && profile.Aliases.Alias != null)
      {
        foreach (Name alias in profile.Aliases.Alias)
        {
          int intFirstNameID = MyFirstNameCache.GetID(Text.NullCheck(alias.FirstName));
          int intMiddleNameID = MyMiddleNameCache.GetID(Text.NullCheck(alias.MiddleName));
          int intLastNameID = MyLastNameCache.GetID(Text.NullCheck(alias.LastName));
          listAliasIDs.Add(MyAliasCache.GetID(intFirstNameID, intMiddleNameID, intLastNameID));
        }
      }

      if (profile.Relatives != null && profile.Relatives.Relative != null)
      {
        foreach (Relative relative in profile.Relatives.Relative)
        {
          string strRelationship = Text.NullCheck(relative.Relationship);
          int intFirstNameID = MyFirstNameCache.GetID(Text.NullCheck(relative.Name.FirstName));
          int intMiddleNameID = MyMiddleNameCache.GetID(Text.NullCheck(relative.Name.MiddleName));
          int intLastNameID = MyLastNameCache.GetID(Text.NullCheck(relative.Name.LastName));
          int intID = MyPersonCache.GetID(
            strProfileID: Text.NullCheck(relative.ProfileID),
            intFirstNameID: intFirstNameID,
            intMiddleNameID: intMiddleNameID,
            intLastNameID: intLastNameID);
          listRelativeIDs.Add(strRelationship + "|" + intID.ToString());
        }
      }
      return MyPersonCache.GetID(
        strProfileID: Text.NullCheck(profile.ProfileID),
        intFirstNameID: MyFirstNameCache.GetID(Text.NullCheck(profile.Name.FirstName)),
        intMiddleNameID: MyMiddleNameCache.GetID(Text.NullCheck(profile.Name.MiddleName)),
        intLastNameID: MyLastNameCache.GetID(Text.NullCheck(profile.Name.LastName)),
        dtBirth: profile.GetDateOfBirth(),
        intPhoneCount: (profile.Phones != null && profile.Phones.Phone != null && profile.Phones.Phone.Length > 0) ? profile.Phones.Phone.Length : (int?)null,
        intEmailCount: (profile.Emails != null && profile.Emails.Email != null && profile.Emails.Email.Length > 0) ? profile.Emails.Email.Length : (int?)null,
        strListSchoolIDs: listSchoolIDs.Count == 0 ? null : String.Join(",", listSchoolIDs.ToArray()),
        strListRelativeIDs: listRelativeIDs.Count == 0 ? null : String.Join(",", listRelativeIDs.ToArray()),
        strListCompanyIDs: listCompanyIDs.Count == 0 ? null : String.Join(",", listCompanyIDs.ToArray()),
        strListGeoLocationIDs: listGeoLocationIDs.Count == 0 ? null : String.Join(",", listGeoLocationIDs.ToArray()),
        strListAliasIDs: listAliasIDs.Count == 0 ? null : String.Join(",", listAliasIDs.ToArray()));

    }
  }
}
