using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Aci.X.ClientLib.ProfileTypes;
using NLog;
using Aci.X.IwsLib.Commerce.v2_1.Customer;


namespace Aci.X.IwsLib
{
  public class CommerceClientBase
  {
    public string ClientID
    {
      get
      {
        return IwsConfig.CommerceClientID;
      }
    }
    public string AuthID
    {
      get
      {
        return IwsConfig.CommerceAuthID;
      }
    }
    public string AuthKey
    {
      get
      {
        return IwsConfig.CommerceAuthKey;
      }
    }

    protected void ValidateResponse(object response, NLogger logger, string strMessage, params object[] oParams)
    {
      var completionResponse = response.GetType().GetField("CompletionResponse").GetValue(response);
      var compRespType = completionResponse.GetType();
      var completionCode = (string)compRespType.GetProperty("CompletionCode").GetValue(completionResponse, null);
      if (completionCode != "1000")
      {
        var responseMessage = (string)compRespType.GetProperty("ResponseMessage").GetValue(completionResponse, null);
        var responseDetail = (string)compRespType.GetProperty("ResponseDetail").GetValue(completionResponse, null);
        logger.LogEvent(LogLevel.Warn,
          String.Format(strMessage, oParams) + " CompletionCode={0}, Message={1}, Detail={2}",
          completionCode,
          responseMessage,
          responseDetail);
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, responseDetail);

      }
    }
  }
}
