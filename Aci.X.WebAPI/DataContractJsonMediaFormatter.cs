using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Aci.X.WebAPI
{
  public class DataContractJsonMediaFormatter : MediaTypeFormatter
  {
    public DataContractJsonMediaFormatter()
    {
      SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
    }

    public override bool CanWriteType(Type type)
    {
      return type != typeof(Stream);
    }

    public override bool CanReadType(Type type)
    {
      return type != typeof(Stream);
    }


    public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
    {
      var task = Task<object>.Factory.StartNew(() =>
      {
        var serializer = new DataContractJsonSerializer(type);
        try
        {
          return serializer.ReadObject(readStream);
        }
        catch (System.Runtime.Serialization.SerializationException sex)
        {
          return null;
        }
      });
      return task;
    }

    public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext)
    {
      var task = Task.Factory.StartNew(() =>
      {
        if (value != null)
        {
          var serializer = new DataContractJsonSerializer(type);
          serializer.WriteObject(writeStream, value);
          writeStream.Flush();
        }
      });
      return task;
    }
  }
}