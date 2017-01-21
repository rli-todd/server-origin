using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class Duration
  {
    public string unit; // HOUR, DAY, MONTH, YEAR
    public decimal amount; // The amount of unit(s), i.e. *1* month
    public decimal price; // The price associated with this duration;
  }
}
