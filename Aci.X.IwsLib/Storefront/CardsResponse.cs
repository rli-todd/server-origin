using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class CardsResponse
  {
    public int responseCode;
    public ResponseDetail responseDetail;
    public List<Card> cards;
  }
}
