using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aci.X.WebAPI
{
  [AttributeUsage(AttributeTargets.All)]
  public class ReturnValueAttribute : Attribute
  {
    public Type ReturnType;
    public ReturnValueAttribute(Type t)
    {
      ReturnType = t;
    }
  }

}