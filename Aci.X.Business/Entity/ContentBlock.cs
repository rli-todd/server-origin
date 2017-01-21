using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business
{
  public class ContentBlock
  {

    private static void ReplaceKeyValuePairs( DBSectionTemplate sectionTemplate, IEnumerable<KeyValuePair<string,string>> keyValuePairs)
    {
      /*
        * Replace name/value pairs supplied by the page in the templates themselves
        */
      if (keyValuePairs != null)
      {
        if (!String.IsNullOrEmpty(sectionTemplate.HeaderTemplate))
        {
          foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
          {
            sectionTemplate.HeaderTemplate = sectionTemplate.HeaderTemplate.Replace(
              "{" + keyValuePair.Key + "}",
              keyValuePair.Value);
          }
        }
        if (!String.IsNullOrEmpty(sectionTemplate.BodyTemplate))
        {
          foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
          {
            sectionTemplate.BodyTemplate = sectionTemplate.BodyTemplate.Replace(
              "{" + keyValuePair.Key + "}",
              keyValuePair.Value);
          }
        }
      }
    }


    public static ClientLib.ContentBlock[] ToClient(
      DBSectionTemplate[] sectionTemplates, 
      IEnumerable<KeyValuePair<string,string>> keyValuePairs=null)
    {
      Dictionary<string, ClientLib.ContentBlock> dictBlockNameToBlock = new Dictionary<string, ClientLib.ContentBlock>();
      foreach (DBSectionTemplate sectionTemplate in sectionTemplates)
      {
        // Replace key-value pairs in templates themselves.
        ReplaceKeyValuePairs(sectionTemplate, keyValuePairs);

        if (!dictBlockNameToBlock.ContainsKey(sectionTemplate.BlockName))
        {
          ClientLib.ContentBlock block = new ClientLib.ContentBlock
          {
            BlockName = sectionTemplate.BlockName,
            BlockType = sectionTemplate.BlockType
          };
          block.SectionList = new List<ContentSection>();
          dictBlockNameToBlock[sectionTemplate.BlockName] = block;
        }
        ContentSection section = new ContentSection
        {
          SectionName = sectionTemplate.SectionName,
          Description = sectionTemplate.Description,
          SectionType = sectionTemplate.SectionType,
          BlockType = sectionTemplate.BlockType,
          SelectedVariationID = sectionTemplate.VariationID
        };
        string strHeaderContent = null;
        string strMultirowPrefix = "";
        string strMultirowSuffix = "";
        string strMultirowDelimiter = "";
        List<string> listBodyContentStrings = new List<string>();
        /*
         * ContentHeader will use values only from the first row (if there are multiple rows)
         */
        if (sectionTemplate.HeaderTemplate != null)
        {
          if (sectionTemplate.ValueRows == null || sectionTemplate.ValueRows.Length == 0)
          {
            strHeaderContent = (sectionTemplate.HeaderTemplate.Contains('{'))
              ? sectionTemplate.HeaderDefault??""
              : sectionTemplate.HeaderTemplate;
          }
          else
          {
            try
            {
              strHeaderContent = String.Format(
                sectionTemplate.HeaderTemplate,
                sectionTemplate.ValueRows[0].Values);
            }
            catch (Exception ex)
            {
              throw new Exception(
                String.Format("Error: Block={0}, Section={1}, Prefix={2}, Suffix={3}, Delim={4}, Var={5}: {6}",
                  sectionTemplate.BlockName,
                  sectionTemplate.SectionName,
                  strMultirowPrefix,
                  strMultirowSuffix,
                  strMultirowDelimiter,
                  sectionTemplate.VariationID,
                  ex.Message));
            }
          }
        }
        if (sectionTemplate.BodyTemplate != null)
        {
          if (sectionTemplate.ValueRows == null || sectionTemplate.ValueRows.Length == 0)
          {
            listBodyContentStrings.Add(
              sectionTemplate.BodyTemplate.Contains('{')
                ? sectionTemplate.BodyDefault??""
                : sectionTemplate.BodyTemplate);
          }
          else
          {
            foreach (DBSectionValues valueRow in sectionTemplate.ValueRows)
            {
              try
              {
                listBodyContentStrings.Add(String.Format(sectionTemplate.BodyTemplate, valueRow.Values));
                if (!String.IsNullOrEmpty(sectionTemplate.MultirowPrefix))
                {
                  strMultirowPrefix = String.Format(sectionTemplate.MultirowPrefix, valueRow.Values);
                }
                if (!String.IsNullOrEmpty(sectionTemplate.MultirowSuffix))
                {
                  strMultirowSuffix = String.Format(sectionTemplate.MultirowSuffix, valueRow.Values);
                }
                if (!String.IsNullOrEmpty(sectionTemplate.MultirowDelimiter))
                {
                  strMultirowDelimiter = String.Format(sectionTemplate.MultirowDelimiter, valueRow.Values);
                }
              }
              catch (Exception ex)
              {
                throw new Exception(
                  String.Format("Error: Block={0}, Section={1}, Prefix={2}, Suffix={3}, Delim={4}, Var={5}: {6}",
                    sectionTemplate.BlockName,
                    sectionTemplate.SectionName,
                    strMultirowPrefix,
                    strMultirowSuffix,
                    strMultirowDelimiter,
                    sectionTemplate.VariationID,
                    ex.Message));
              }
            }
          }
        }
        section.HeaderContent = strHeaderContent;
        section.BodyContent =
          strMultirowPrefix +
          String.Join(strMultirowDelimiter, listBodyContentStrings) +
          strMultirowSuffix;
        dictBlockNameToBlock[sectionTemplate.BlockName].SectionList.Add(section);
      }
      foreach (ClientLib.ContentBlock block in dictBlockNameToBlock.Values)
      {
        block.Sections = block.SectionList.ToArray();
        block.SectionList = null;
      }
      return dictBlockNameToBlock.Values.ToArray();
    }
  }
}
