using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Reflection;
using Aci.X.ServerLib;
using Solishine.CommonLib;
using Aci.X.ClientLib;

namespace Aci.X.WebAPI
{
  public static class FilterAttributeExtensions
  {

    /// <summary>
    /// If the client is expecting a "WebServiceResponse" type in the response
    /// payload, then we will construct one and set the Response appropriately.
    /// </summary>
    /// <param name="context"></param>
    public static HttpResponseMessage SetResponseMessage(this FilterAttribute attribute, HttpRequestMessage request, HttpStatusCode statusCode, Exception ex = null)
    {
      HttpResponseMessage retVal = new HttpResponseMessage { StatusCode = statusCode, ReasonPhrase = ex == null ? null : ex.Message.Replace('\n',' ').Replace('\r',' ') };
      //var callContext = HttpContext.Current.Items["CallContext"] as CallContext;
      var callContext = CallContext.Current;
      if (callContext != null && callContext.ActionMethod != null)
      {
        ReturnValueAttribute attr = callContext.ActionMethod.GetCustomAttribute<ReturnValueAttribute>();
        if (attr != null && attr.ReturnType != null && typeof(WebServiceResponse).IsAssignableFrom(attr.ReturnType))
        {
          /*
           * Get the default constructor for the "WebServiceResponse" type (could be a generic)
           * and create an instance with the error message and type.
           */
          ConstructorInfo ctor = attr.ReturnType.GetConstructor(new Type[0]);
          object oWebServiceResponse = ctor.Invoke(new object[0]);
          WebServiceResponse sr = (WebServiceResponse)oWebServiceResponse;
          if (ex != null)
          {
            sr.ErrorMessage = ex.Message;
            sr.ErrorType = ex.GetType().Name;
          }
          // Short-circuit all the reflection and complexity if this is a non-generic WebServiceResponse
          if (attr.ReturnType == typeof(WebServiceResponse))
            retVal = (HttpResponseMessage)request.CreateResponse<WebServiceResponse>(statusCode, sr);
          else
          {
            /*
             * So we need to basically call HttpRequestMessage.CreateResponse<T> where 
             * T is the WebServiceResponse type we just created above.
             * HttpRequestMessage.CreateResponse<T> is an extension method defined in
             * System.Net.Http.HttpRequestMessageExtensions.   
             * The problem is that this class is defined in two different assemblies
             * (what's up with that, Microsoft?)  The one we want is in System.Web.Http.dll,
             * so in order to create an unambigous instance of that Type, we need to load
             * that particular assembly.
             */
            AssemblyName[] ans = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            AssemblyName an = (
              from a in ans
              where a.Name.StartsWith("System.Web.Http")
              select a).FirstOrDefault();

            Assembly aSystemWebHttp = Assembly.Load(an);

            Type extensionType = aSystemWebHttp.GetType("System.Net.Http.HttpRequestMessageExtensions");
            try
            {
              /*
               * Now for some more complexity.  There are a number of generic versions
               * of the CreateResponse<T> method on that class, and it is not really possible
               * to find the right method using Type.GetMethod, so I have utilized a handy
               * Type extension class written by Ken Beckett and posted here:
               * http://stackoverflow.com/questions/4035719/getmethod-for-generic-method
               */
              MethodInfo miCreateResponse = extensionType.GetMethodExt("CreateResponse", new Type[] 
            { 
              typeof(System.Net.Http.HttpRequestMessage),
              typeof(System.Net.HttpStatusCode),
              typeof(TypeExtensions.T)
            });
              /*
               * Now that we have identified the method, create a generic version of
               * it using the particular WebServiceResponse<T> type that we need to return.
               */
              var CreateResponse = miCreateResponse.MakeGenericMethod(attr.ReturnType);
              /*
               * And (finally!) call the method to create the HttpRespnoseMessage to return.
               * Phew. That was alot of work.
               */
              retVal = (HttpResponseMessage)CreateResponse.Invoke(null, new object[] { request, statusCode, oWebServiceResponse });
            }
            catch (Exception)
            {
              // If we can't deliver the WebServiceResponse, then... we can't deliver it.
              // Just swallow this exception, and return the original exception.
            }
          }
        }
      }
      return retVal;
    }
  }
}

