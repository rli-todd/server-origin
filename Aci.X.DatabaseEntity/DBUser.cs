using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBUser : MySqlResult
  {
    public byte SiteID;
    public int UserID;
    public Guid UserGuid;
    public int FirstVisitID;
    public int LastVisitID;
    public int ExternalID;
    public string FirstName;
    public string MiddleName;
    public string LastName;
    public string EmailAddress;
    public bool HasAcceptedUserAgreement;
    public bool HasValidPaymentMethod;
    public string CardNumberLast4;
    public string CardCVV;
    public byte[] CardHash;
    public string CardholderName;
    public string CardExpiry;
    public string CardAddress;
    public string CardCity;
    public string CardState;
    public string CardCountry;
    public string CardZip;
    public DateTime? CardLastModified;
    public DateTime? DateLastAuthenticated;
    public DateTime DateCreated;
    public bool IsBackofficeReader;
    public bool IsBackofficeWriter;

    public override void Read()
    {
      SiteID= Value<byte>("SiteID");
      UserID= Value<int>("ID");
      UserGuid= Value<Guid>("UserGuid");
      FirstVisitID = Value<int>("FirstVisitID");
      LastVisitID = Value<int>("LastVisitID");
      ExternalID = Value<int>("ExternalID");
      FirstName = Value<String>("FirstName");
      MiddleName = Value<String>("MiddleName");
      LastName = Value<String>("LastName");
      EmailAddress = Value<String>("EmailAddress");
      HasAcceptedUserAgreement = Value<bool>("HasAcceptedUserAgreement");
      HasValidPaymentMethod = Value<bool>("HasValidPaymentMethod");
      CardNumberLast4 = Value<string>("CardLast4");
      CardCVV = Value<string>("CardCVV");
      CardHash = Value<byte[]>("CardHash");
      CardholderName = Value<string>("CardholderName");
      CardExpiry = Value<string>("CardExpiry");
      CardAddress = Value<string>("CardAddress");
      CardCity = Value<string>("CardCity");
      CardState = Value<string>("CardState");
      CardCountry = Value<string>("CardCountry");
      CardZip = Value<String>("CardZip");
      CardLastModified = Value<DateTime?>("CardLastModified");
      DateLastAuthenticated = Value<DateTime?>("DateLastAuthenticated");
      DateCreated = Value<DateTime>("DateCreated");
      IsBackofficeReader = Value<bool>("IsBackofficeReader");
      IsBackofficeWriter = Value<bool>("IsBackofficeWriter");
    }
  }
}
