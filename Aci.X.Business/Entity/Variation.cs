using Aci.X.DatabaseEntity;

namespace Aci.X.Business
{
  public class Variation
  {
    public static ClientLib.Variation[] ToClient(DBVariation[] dbVariations)
    {
      ClientLib.Variation[] retVal = new ClientLib.Variation[dbVariations.Length];
      for (int idx = 0; idx < dbVariations.Length; ++idx)
      {
        retVal[idx] = ToClient(dbVariations[idx]);
      }
      return retVal;
    }

    public static ClientLib.Variation ToClient(DBVariation dbVariation)
    {
      return new ClientLib.Variation
      {
        VariationID = dbVariation.VariationID,
        SectionID = dbVariation.SectionID,
        Description = dbVariation.Description,
        MultirowPrefix = dbVariation.MultirowPrefix,
        MultirowSuffix = dbVariation.MultirowSuffix,
        MultirowDelimiter = dbVariation.MultirowDelimiter,
        HeaderTemplate = dbVariation.HeaderTemplate,
        BodyTemplate = dbVariation.BodyTemplate,
        ViewName = dbVariation.ViewName,
        ViewFieldNames = dbVariation.ViewFieldNames,
        IsEnabled = dbVariation.IsEnabled
      };
    }
  }
}
