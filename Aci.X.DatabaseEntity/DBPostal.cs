using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBPostal : MySqlResult
  {
    public string PostalCode;
    public string City;
    public string StateAbbr;
    public string County;
    public string CountryISO;
    public Single Latitude;
    public Single Longitude;

    public override void Read()
    {
      PostalCode= Value<string>("PostalCode");
      City = Value<string>("City");
      StateAbbr = Value<string>("StateAbbr");
      County = Value<string>("County");
      CountryISO = Value<string>("CountryISO");
      Latitude = Value<Single>("Latitude");
      Longitude = Value<Single>("Longitude");
    }
  }
}
