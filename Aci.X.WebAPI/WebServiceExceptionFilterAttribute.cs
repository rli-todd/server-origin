using System;
using System.Net;
using System.Diagnostics;
using System.Web.Http.Filters;
using NLog;

namespace Aci.X.WebAPI
{
  public class WebServiceExceptionFilterAttribute : ExceptionFilterAttribute
  {
    public override void OnException(HttpActionExecutedContext context)
    {
      Exception ex = context.Exception;
      HttpStatusCode statusCode = GetStatusCode(context.Exception);
      // Set the default response.
      // Try to set a customized response (yielding a serialized ServerResponse) if appropriate.
      context.Response = this.SetResponseMessage(context.Request, statusCode, context.Exception);
      if (statusCode == HttpStatusCode.InternalServerError)
      {
        while (ex != null)
        {
          StackFrame stackFrame = new StackFrame(1);
          LogManager
            .GetLogger(context.ActionContext.ControllerContext.Controller.GetType().Name)
            .Error("{0}: {1}({2}): {3}", context.ActionContext.ActionDescriptor.ActionName, ex.GetType().Name, ex.Message, ex.StackTrace);
          Aci.X.ServerLib.CallContext.Current.Exception = ex;
          ex = ex.InnerException;
        }
      }
    }

    /// <summary>
    /// Try to set the HTTP status code apppropriate to the exception that was thrown.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    private HttpStatusCode GetStatusCode(Exception ex)
    {
      HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
      if (ex.Message.Contains("BadRequest"))
      {
        statusCode = HttpStatusCode.BadRequest;
      }
      else if (ex.Message.Contains("Forbidden"))
      {
        statusCode = HttpStatusCode.Forbidden;
      }
      else if (ex is System.IO.FileNotFoundException || ex.Message.Contains("NotFound"))
      {
        statusCode = HttpStatusCode.NotFound;
      }
      else if (ex is System.UnauthorizedAccessException || ex.Message.Contains("Unauthorized"))
      {
        statusCode = HttpStatusCode.Unauthorized;
      }
      else if (ex.Message.Contains("AlreadyExists") || ex.Message.Contains("Conflict"))
      {
        statusCode = HttpStatusCode.Conflict;
      }
      return statusCode;
    }

  }
}