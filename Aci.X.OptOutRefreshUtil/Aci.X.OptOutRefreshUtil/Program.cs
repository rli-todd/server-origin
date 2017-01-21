using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.IwsLib;
using Aci.X.ClientLib;

namespace Aci.X.OptOutRefreshUtil
{
  class Program
  {
    static void Main(string[] args)
    {
      using (var db = new ProfileDB())
      {
        foreach (var optout in db.spOptOutGetUnrefreshed())
        {
          var query = new PersonQuery
          {
            FirstName = optout.FirstName,
            LastName = optout.LastName,
            State = optout.State
          };
          ProfileHelper.GetPreviews(query);
          db.spOptOutSetRefreshDate(
            strFirstName: optout.FirstName,
            strLastName: optout.LastName,
            strState: optout.State);
        }
      }
    }
  }
}
