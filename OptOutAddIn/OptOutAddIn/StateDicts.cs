namespace OptOutAddIn
{
  using System;
  using System.Collections.Generic;
  using System.Collections;

  public class MyDict : Dictionary<string, string>
  {

    public new void Add(string strKey,string strValue)
    {
      base.Add(strKey.ToLower(), strValue.ToLower());
    }

    public new bool ContainsKey(string strKey)
    {
      return base.ContainsKey(strKey.ToLower());
    }
  }

  public class StateDicts
  {
    public static MyDict StateAbbrByName = new MyDict();
    public static MyDict StateNameByAbbr = new MyDict();

    static StateDicts()
    {
      StateNameByAbbr.Add("AK", "Alaska");
      StateNameByAbbr.Add("AL", "Alabama");
      StateNameByAbbr.Add("AR", "Arkansas");
      StateNameByAbbr.Add("AZ", "Arizona");
      StateNameByAbbr.Add("CA", "California");
      StateNameByAbbr.Add("CO", "Colorado");
      StateNameByAbbr.Add("CT", "Connecticut");
      StateNameByAbbr.Add("DC", "Washington DC");
      StateNameByAbbr.Add("DE", "Delaware");
      StateNameByAbbr.Add("FL", "Florida");
      StateNameByAbbr.Add("GA", "Georgia");
      StateNameByAbbr.Add("HI", "Hawaii");
      StateNameByAbbr.Add("IA", "Iowa");
      StateNameByAbbr.Add("ID", "Idaho");
      StateNameByAbbr.Add("IL", "Illinois");
      StateNameByAbbr.Add("IN", "Indiana");
      StateNameByAbbr.Add("KS", "Kansas");
      StateNameByAbbr.Add("KY", "Kentucky");
      StateNameByAbbr.Add("LA", "Louisiana");
      StateNameByAbbr.Add("MA", "Massachusetts");
      StateNameByAbbr.Add("MD", "Maryland");
      StateNameByAbbr.Add("ME", "Maine");
      StateNameByAbbr.Add("MI", "Michigan");
      StateNameByAbbr.Add("MN", "Minnesota");
      StateNameByAbbr.Add("MO", "Missouri");
      StateNameByAbbr.Add("MS", "Mississippi");
      StateNameByAbbr.Add("MT", "Montana");
      StateNameByAbbr.Add("NC", "North Carolina");
      StateNameByAbbr.Add("ND", "North Dakota");
      StateNameByAbbr.Add("NE", "Nebraska");
      StateNameByAbbr.Add("NH", "New Hampshire");
      StateNameByAbbr.Add("NJ", "New Jersey");
      StateNameByAbbr.Add("NM", "New Mexico");
      StateNameByAbbr.Add("NV", "Nevada");
      StateNameByAbbr.Add("NY", "New York");
      StateNameByAbbr.Add("OH", "Ohio");
      StateNameByAbbr.Add("OK", "Oklahoma");
      StateNameByAbbr.Add("OR", "Oregon");
      StateNameByAbbr.Add("PA", "Pennsylvania");
      StateNameByAbbr.Add("RI", "Rhode Island");
      StateNameByAbbr.Add("SC", "South Carolina");
      StateNameByAbbr.Add("SD", "South Dakota");
      StateNameByAbbr.Add("TN", "Tennessee");
      StateNameByAbbr.Add("TX", "Texas");
      StateNameByAbbr.Add("UT", "Utah");
      StateNameByAbbr.Add("VA", "Virginia");
      StateNameByAbbr.Add("VI", "Virginia");
      StateNameByAbbr.Add("VT", "Vermont");
      StateNameByAbbr.Add("WA", "Washington");
      StateNameByAbbr.Add("WI", "Wisconsin");
      StateNameByAbbr.Add("WV", "West Virginia");
      StateNameByAbbr.Add("WY", "Wyoming");
      StateAbbrByName.Add("Alaska", "AK");
      StateAbbrByName.Add("Alabama", "AL");
      StateAbbrByName.Add("Arkansas", "AR");
      StateAbbrByName.Add("Arizona", "AZ");
      StateAbbrByName.Add("California", "CA");
      StateAbbrByName.Add("Colorado", "CO");
      StateAbbrByName.Add("Connecticut", "CT");
      StateAbbrByName.Add("Washington DC", "DC");
      StateAbbrByName.Add("Delaware", "DE");
      StateAbbrByName.Add("Florida", "FL");
      StateAbbrByName.Add("Georgia", "GA");
      StateAbbrByName.Add("Hawaii", "HI");
      StateAbbrByName.Add("Iowa", "IA");
      StateAbbrByName.Add("Idaho", "ID");
      StateAbbrByName.Add("Illinois", "IL");
      StateAbbrByName.Add("Indiana", "IN");
      StateAbbrByName.Add("Kansas", "KS");
      StateAbbrByName.Add("Kentucky", "KY");
      StateAbbrByName.Add("Louisiana", "LA");
      StateAbbrByName.Add("Massachusetts", "MA");
      StateAbbrByName.Add("Maryland", "MD");
      StateAbbrByName.Add("Maine", "ME");
      StateAbbrByName.Add("Michigan", "MI");
      StateAbbrByName.Add("Minnesota", "MN");
      StateAbbrByName.Add("Missouri", "MO");
      StateAbbrByName.Add("Mississippi", "MS");
      StateAbbrByName.Add("Montana", "MT");
      StateAbbrByName.Add("North Carolina", "NC");
      StateAbbrByName.Add("North Dakota", "ND");
      StateAbbrByName.Add("Nebraska", "NE");
      StateAbbrByName.Add("New Hampshire", "NH");
      StateAbbrByName.Add("New Jersey", "NJ");
      StateAbbrByName.Add("New Mexico", "NM");
      StateAbbrByName.Add("Nevada", "NV");
      StateAbbrByName.Add("New York", "NY");
      StateAbbrByName.Add("Ohio", "OH");
      StateAbbrByName.Add("Oklahoma", "OK");
      StateAbbrByName.Add("Oregon", "OR");
      StateAbbrByName.Add("Pennsylvania", "PA");
      StateAbbrByName.Add("Rhode Island", "RI");
      StateAbbrByName.Add("South Carolina", "SC");
      StateAbbrByName.Add("South Dakota", "SD");
      StateAbbrByName.Add("Tennessee", "TN");
      StateAbbrByName.Add("Texas", "TX");
      StateAbbrByName.Add("Utah", "UT");
      StateAbbrByName.Add("Virginia", "VA");
      StateAbbrByName.Add("Vermont", "VT");
      StateAbbrByName.Add("Washington", "WA");
      StateAbbrByName.Add("Wisconsin", "WI");
      StateAbbrByName.Add("West Virginia", "WV");
      StateAbbrByName.Add("Wyoming", "WY");
    }
  }
}

