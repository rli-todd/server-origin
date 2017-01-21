using Aci.X.DatabaseEntity;

namespace Aci.X.Business
{
  public class Section
  {
    public static ClientLib.Section[] ToClient(DBSection[] dbSections)
    {
      ClientLib.Section[] retVal = new ClientLib.Section[dbSections.Length];
      for (int idx = 0; idx < dbSections.Length; ++idx)
      {
        retVal[idx] = ToClient(dbSections[idx]);
      }
      return retVal;
    }

    public static ClientLib.Section ToClient(DBSection dbSection)
    {
      return new ClientLib.Section
      {
        SectionID = dbSection.SectionID,
        BlockID = dbSection.BlockID,
        SectionName = dbSection.SectionName,
        SectionType = dbSection.SectionType,
        IsEnabled = dbSection.IsEnabled
      };
    }
  }
}
