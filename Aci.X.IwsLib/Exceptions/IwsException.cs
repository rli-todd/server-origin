using System;

namespace Aci.X.IwsLib
{
  public class IwsException : Exception
  {
    public System.Net.HttpStatusCode StatusCode;
    public string Reason;

    public IwsException(System.Net.HttpStatusCode statusCode, string strReason)
      : base(statusCode.ToString() + ": " + strReason)
    {
      StatusCode = statusCode;
      Reason = strReason;
    }
  }

  public class StorefrontException : Exception
  {
    public StorefrontException(int intResponseCode, string strMessage)
      : base(String.Format("Unexpected error: {0},{1}", intResponseCode, strMessage))
    { }
  }

  public class StorefrontBadRequestException : Exception
  {
    public StorefrontBadRequestException(int intResponseCode, string strMessage)
      : base(String.Format("BadRequest: {0},{1}", intResponseCode, strMessage))
    { }
  }

}
