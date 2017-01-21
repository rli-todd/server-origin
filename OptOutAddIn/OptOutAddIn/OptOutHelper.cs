using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OptOutAddIn
{
  public class OptOutHelper
  {
    private class QueryParams
    {
      public string LastName="";
      public string FirstName="";
      public string MiddleName="";
      public string StateAbbr="";

      public string Key
      {
        get
        {
          return LastName + "," + FirstName + "," + StateAbbr;
        }
      }
    }

    private static Regex regex = new Regex(
        @"://(www\.)?(public|criminal)records\.com/(people-search-)?records/(?<query>[^""<>?\s]*)", RegexOptions.Singleline);

    public static void ProcessSelection(Outlook.Selection selection)
    {
      Dictionary<string, string> dictQueries = new Dictionary<string, string>();
      foreach (Outlook.MailItem mailItem in selection)
      {
        dictQueries = ParseMailItem(mailItem, dictQueries);
      }
      CopyToClipboard(dictQueries);
    }

    public static void ProcessMailItem(Outlook.MailItem mailItem)
    {
      Dictionary<string, string> dictQueries = new Dictionary<string, string>();
      dictQueries = ParseMailItem(mailItem, dictQueries);
      CopyToClipboard(dictQueries);
    }

    private static Dictionary<string,string> ParseMailItem( Outlook.MailItem mailItem, Dictionary<string,string> dictQueries)
    {
      string strSubject = mailItem.Subject;
      string strBody = mailItem.Body;
      if (strBody != null)
      {
        MatchCollection mc = regex.Matches(strBody);
        foreach (Match m in mc)
        {
          if (m.Groups["query"] != null && m.Groups["query"].Value != null)
          {
            dictQueries[m.Groups["query"].Value.Replace("%20", "")] = null;
          }
        }
      }
      return dictQueries;
    }

    private static string CapFirst(string str)
    {
      var strTemp = (str ?? "").Trim().ToLower();
      return (strTemp.Length < 2)
        ? strTemp.ToUpper()
        : strTemp.Substring(0,1).ToUpper() + strTemp.Substring(1).ToLower();
    }

    private static string Filler(int intSize)
    {
      StringBuilder sb = new StringBuilder();
      for (int i=0; i<intSize;++i)
      { 
        sb.Append(" "); 
      }
      return sb.ToString();
    }


    private static void CopyToClipboard( Dictionary<string,string> dictQueries)
    {
      if (dictQueries == null || dictQueries.Count == 0)
      {
        System.Windows.Forms.MessageBox.Show("No search results URLs found in the selected mail items", "None found", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
      }
      else 
      {
        StringBuilder sb = new StringBuilder();
        SortedDictionary<string, QueryParams> dictClipboardEntries = new SortedDictionary<string, QueryParams>();
        int intMaxFirstNameLen = 0;
        int intMaxLastNameLen = 0;
        foreach (string strKey in dictQueries.Keys)
        {
          try
          {
            QueryParams queryParams = new QueryParams();
            ParseSearchQuery(strKey, out queryParams);
            if (!dictClipboardEntries.ContainsKey(queryParams.Key))
            {
              dictClipboardEntries[queryParams.Key] = queryParams;
              if (queryParams.FirstName.Length > intMaxFirstNameLen)
                intMaxFirstNameLen = queryParams.FirstName.Length;
              if (queryParams.LastName.Length > intMaxLastNameLen)
                intMaxLastNameLen = queryParams.LastName.Length;
            }
          }
          catch (Exception ex)
          {
            sb.AppendFormat("-- key={0}: {1} at {2}\n", strKey??"NULL", ex.Message, ex.StackTrace);
          }
        }
        foreach (var strEntryKey in dictClipboardEntries.Keys)
        {
          var qp = dictClipboardEntries[strEntryKey];
          var state = string.IsNullOrEmpty(qp.StateAbbr) ? "" : String.Format(",{0}'{1}'", Filler(intMaxLastNameLen - qp.LastName.Length), qp.StateAbbr.ToUpper());
          sb.AppendFormat("EXEC spSearchResultsDelete '{0}',{1}'{2}'{3}\n",
            CapFirst(qp.FirstName), Filler(intMaxFirstNameLen - qp.FirstName.Length), CapFirst(qp.LastName), state);
        }
        System.Windows.Forms.Clipboard.SetText(sb.ToString());
        System.Windows.Forms.MessageBox.Show("The following has been added to the Clipboard:\n\n" + sb.ToString(), dictQueries.Count.ToString() + " opt-outs found", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
      }
    }


    private static void ParseSearchQuery(string strName, out QueryParams queryParams)
    {
      try
      {
        queryParams = new QueryParams();
        if (strName != null)
        {
          if (strName.EndsWith("-"))
          {
            strName = strName.Substring(0, strName.Length - 1);
          }
          int idx = strName.IndexOf("-in-", 0);
          if (idx > 0)
          {
            string strStateName = strName.Substring(idx + 4, strName.Length - (idx + 4)).Replace('-', ' ');
            strName = strName.Substring(0, idx);
            if (StateDicts.StateAbbrByName.ContainsKey(strStateName))
            {
              queryParams.StateAbbr = StateDicts.StateAbbrByName[strStateName];
              strName = strName.Substring(0, idx);
            }
            else
            {
              // Not found. Let's see if this is already an abbreviation
              if (StateDicts.StateNameByAbbr.ContainsKey(strStateName))
              {
                queryParams.StateAbbr = strStateName;
                strName = strName.Substring(0, idx);
              }
            }
          }
          else
          {
            queryParams.StateAbbr = "";
          }
          string[] strParts = strName.Split(new char[] { '-'/*, ' ' */}, StringSplitOptions.RemoveEmptyEntries);
          queryParams.FirstName = (strParts[0]);
          if (strParts.Length > 2)
          {
            queryParams.MiddleName = strParts[1].ToUpper();
            queryParams.LastName = (strParts[2]);
          }
          else if (strParts.Length >1 )
          {
            queryParams.MiddleName = "";
            queryParams.LastName = (strParts[1]);
          }
          else
          {
            queryParams.MiddleName = queryParams.LastName = string.Empty;
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Link.ParseSearchQuery({0}): {1} at {2}", strName, ex.Message, ex.StackTrace));
      }
    }
  }
}
