using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aci.X.IwsLib;
using Aci.X.ClientLib;
using Aci.X.ClientLib.ProfileTypes;
using Aci.X.WebAPI.Tests;

namespace Aci.X.IwsLib.Tests
{
  [TestClass]
  public class ProfileTests
  {

    private PersonQuery _testQuery = new PersonQuery
    {
      FirstName = "brian",
      MiddleName = "lee",
      LastName = "smith",
      State = "NC",
      City = "Fort Bragg"
    };

    private PersonQuery _testQuery2 = new PersonQuery
    {
      FirstName = "orenthal",
      MiddleName = "james",
      LastName = "simpson"
    };

    string _strTestProfileID = "08CK12K54VN";

    [TestMethod]
    public void Profile_Preview()
    {
      string strJson;
      ProfileResponse response = ProfileClient.QueryPreviews(out strJson, _testQuery);
      string strXml = response.ToLegacyXML(_testQuery);
    }

    [TestMethod]
    public void Profile_Fetch()
    {
      string strJson;
      ProfileResponse response = ProfileClient.QueryPreviews(out strJson, _testQuery);
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.IsTrue(response.ProfileCount >= 5);
      string[] ProfileIDs = (from p in response.Profiles.Profile select p.ProfileID.ToString()).ToArray();
      Assert.IsTrue(ProfileIDs.Contains(_strTestProfileID));
      response = ProfileClient.QueryProfiles(out strJson, _strTestProfileID);
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.AreEqual(1,response.ProfileCount);
    }


    [TestMethod]
    public void Profile_Fetch_other()
    {
      const string CLIENT_SECRET = "3DF07A4B-9C67-4EC8-9C2F-7760AF23361C";
      var client = new WebServiceClient(CLIENT_SECRET, "127.0.0.1");
      VisitControllerTests.CreateTestVisit(client);
      const string LEO_CALVIN_WALDRON_IN_GA = "06DAYGC7VH3";
      const string TIMOTHY_J_MCVEIGH_IN_LOCKPORT_NY = "06AP4N173FH";
      const string FN = "timothy";
      const string MI = "j";
      const string LN = "mcveigh";
      const string STATE = "NY";
      const string URL = "search/preview?firstname={0}&lMiddleInitial={1}&LastName={2}&state={3}";

      var srp = client.Get<SearchResultsPage>(URL, FN,MI,LN,STATE);

      var response = ProfileHelper.GetProfile(LEO_CALVIN_WALDRON_IN_GA, "all");
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.AreEqual(1, response.ProfileCount);
      var strHtml = response.Profiles.Profile[0].Render();
      var sw = new StreamWriter("C:\\temp\\FULL_profile_LEO_CALVIN_WALDRON_IN_GA.html", append: false);
      sw.Write(strHtml);
      sw.Close();

      response = ProfileHelper.GetProfile(TIMOTHY_J_MCVEIGH_IN_LOCKPORT_NY, "all");
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.AreEqual(1, response.ProfileCount);
      strHtml = response.Profiles.Profile[0].Render();
      sw = new StreamWriter("C:\\temp\\FULL_profile_TIMOTHY_J_MCVEIGH_IN_LOCKPORT_NY.html", append: false);
      sw.Write(strHtml);
      sw.Close();
    }

    string strProfileID_OJ = "0C5FBP4FW19";

    [TestMethod]
    public void Profile_Fetch_OJ()
    {
      
      string CRIMINAL_CHECK_ATTRS = "Name,Addresses,Relatives,CriminalRecords,DOB";
      string PEOPLE_LOOKUP_ATTRS = "Name,Addresses,Relatives,DOB";



      ProfileResponse response = ProfileHelper.GetProfile(strProfileID_OJ, CRIMINAL_CHECK_ATTRS, strState: "NV");
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.AreEqual(1, response.ProfileCount);
      string strHtml = response.Profiles.Profile[0].Render();
      strHtml = response.Profiles.Profile[0].Render();
      var sw = new StreamWriter("C:\\temp\\profile_CRIMINAL_CHECK.html", append: false);
      sw.Write(strHtml);
      sw.Close();

      response = ProfileHelper.GetProfile(strProfileID_OJ, PEOPLE_LOOKUP_ATTRS);
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.AreEqual(1, response.ProfileCount);
      strHtml = response.Profiles.Profile[0].Render();
      strHtml = response.Profiles.Profile[0].Render();
      sw = new StreamWriter("C:\\temp\\profile_PEOPLE_LOOKUP.html", append: false);
      sw.Write(strHtml);
      sw.Close();
      int NOP = 0;

    }

    [TestMethod]
    public void Render_Profile_Background_Check()
    {
      RenderReport(
        strTitle: "Background_Check",
        strProfileID: strProfileID_OJ,
        strAttrs: "Name,Addresses,Relatives,Phones,Emails,BusinessRecords,CivilRecords,CriminalRecords,MarriageDivorceRecords,DOB");
    }

    [TestMethod]
    public void Render_Profile_Nationwide_Criminal_Check()
    {
      RenderReport(
        strTitle: "Nationwide_Criminal_Check",
        strProfileID: strProfileID_OJ,
        strAttrs: "Name,Addresses,Phones,Relatives,CriminalRecords,DOB");
    }

    [TestMethod]
    public void Render_Profile_Statewide_Criminal_Check()
    {
      RenderReport(
        strTitle: "Statewide_Criminal_Check",
        strProfileID: strProfileID_OJ,
        strAttrs: "Name,Addresses,Phones,Relatives,CriminalRecords,DOB",
        strState: "CA");
    }

    [TestMethod]
    public void Render_Profile_People_Lookup()
    {
      RenderReport(
        strTitle: "People_Lookup",
        strProfileID: strProfileID_OJ,
        strAttrs: "Name,Addresses,Relatives,DOB");
    }

    private string RenderReport(string strTitle, string strProfileID, string strAttrs, string strState = null)
    {
      ProfileResponse response = ProfileHelper.GetProfile(strProfileID, strAttrs, strState: strState);
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.AreEqual(1, response.ProfileCount);
      string strHtml = response.Profiles.Profile[0].Render();
      strHtml = response.Profiles.Profile[0].Render();
      string strFilename = String.Format("c:\\Temp\\Profile_{0}_{1}.html", strProfileID, strTitle);
      var sw = new StreamWriter(strFilename, append: false);
      sw.Write(strHtml);
      sw.Close();
      return strHtml;
    }

    [TestMethod]
    public void Profile_PreviewConsumer()
    {
      string strJson;
      ProfileResponse response = ProfileClient.QueryPreviews(out strJson, _testQuery);
      Assert.AreEqual("1000", response.ResponseCode.ToString());
      Assert.IsTrue(response.ProfileCount >= 10);
      foreach (Profile profile in response.Profiles.Profile)
      {
        int intProfileID = Aci.X.IwsLib.DB.ServerProfile.UpdateDB(profile);
        Assert.IsTrue(intProfileID > 0);
      }
    }
  }
}
