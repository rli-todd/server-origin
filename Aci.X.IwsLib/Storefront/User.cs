using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aci.X.IwsLib.Storefront
{
  public class User
  {
    public int userId;
    public string userName;
    public Name name;
    public bool active;
    public bool admin;
    public string dateCreated;
    public Wallet wallet;
    public bool userAgreementAccepted;
    public string company;
    public List<Preference> preferences;
    public List<string> emails;
    public List<Phone> phones;
    public List<Address> addresses;
  }
}
