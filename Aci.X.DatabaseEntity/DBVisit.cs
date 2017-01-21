using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  [DataContract(Namespace = "")]
  public class DBVisit : MySqlResult
  {
    [DataMember(EmitDefaultValue = false)] public int VisitID;
    [DataMember(EmitDefaultValue = false)] public Guid VisitGuid;
    [DataMember(EmitDefaultValue = false)] public string ClientIP;
    [DataMember(EmitDefaultValue = false)] public int UtcOffsetMins;
    [DataMember(EmitDefaultValue = false)] public string AcceptLanguage;
    [DataMember(EmitDefaultValue = false)] public int UserID;
    [DataMember(EmitDefaultValue = false)] public Guid UserGuid;
    [DataMember(EmitDefaultValue = false)] public bool IsBlocked;
    [DataMember(EmitDefaultValue = false)] public int SiteID;
    [DataMember(EmitDefaultValue = false)] public string ReferCode;
    [DataMember(EmitDefaultValue = false)] public string CountryName;
    [DataMember(EmitDefaultValue = false)] public string RegionName;
    [DataMember(EmitDefaultValue = false)] public string CityName;
    [DataMember(EmitDefaultValue = false)] public int StateFips;
    [DataMember(EmitDefaultValue = false)] public int CityFips;
    [DataMember(EmitDefaultValue = false)] public int GeoLocationID;
    [DataMember(EmitDefaultValue = false)] public Single Longitude;
    [DataMember(EmitDefaultValue = false)] public Single Latitude;
    [DataMember(EmitDefaultValue = false)] public string IwsUserToken;
    [DataMember(EmitDefaultValue = false)] public DateTime? IwsUserTokenExpiry;
    [DataMember(EmitDefaultValue = false)] public int? IwsUserID;
    [DataMember(EmitDefaultValue = false)] public string StorefrontUserToken;
    [DataMember(EmitDefaultValue = false)] public int RobotID;
    
    // Not persisted in DB.
    [DataMember(EmitDefaultValue = false)] public ClientLib.Cart Cart;
    [DataMember(EmitDefaultValue = false)] public int CurrentQueryID;
    [DataMember(EmitDefaultValue = false)] public string CurrentQueryState;


    public override void Read()
    {
      VisitID = Value<int>("VisitID");
      ClientIP = Value<string>("ClientIP");
      UtcOffsetMins = Value<int>("UtcOffsetMins");
      AcceptLanguage = Value<string>("AcceptLanguage");
      VisitGuid = Value<Guid>("VisitGuid");
      UserID = Value<int>("UserID");
      UserGuid = Value<Guid?>("UserGuid") ?? Guid.Empty;
      IsBlocked = Value<bool>("IsBlocked");
      SiteID = Value<byte>("SiteID");
      ReferCode = Value<string>("IwsReferCode");
      CountryName = Value<string>("CountryName");
      RegionName = Value<string>("RegionName");
      CityName = Value<string>("CityName");
      StateFips = Value<int>("StateFips");
      CityFips = Value<int>("CityFips");
      GeoLocationID = Value<int>("GeoLocationID");
      Longitude = Value<Single>("Longitude");
      Latitude = Value<Single>("Latitude");
      IwsUserToken = Value<string>("IwsUserToken");
      IwsUserTokenExpiry = Value<DateTime?>("IwsUserTokenExpiry");
      IwsUserID = Value<int?>("IwsUserID");
      StorefrontUserToken = Value<string>("StorefrontUserToken");
      RobotID = Value<int>("RobotID");
    }
  }
}
