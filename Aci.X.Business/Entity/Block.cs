using Aci.X.DatabaseEntity;

namespace Aci.X.Business
{
  public class Block
  {
    public static ClientLib.Block[] ToClient(DBBlock[] dbBlocks)
    {
      ClientLib.Block[] retVal = new ClientLib.Block[dbBlocks.Length];
      for (int idx = 0; idx < dbBlocks.Length; ++idx)
      {
        retVal[idx] = ToClient(dbBlocks[idx]);
      }
      return retVal;
    }

    public static ClientLib.Block ToClient(DBBlock dbBlock)
    {
      return new ClientLib.Block
      {
        BlockID = dbBlock.BlockID,
        BlockName = dbBlock.BlockName,
        BlockType = dbBlock.BlockType,
        IsEnabled = dbBlock.IsEnabled
      };
    }
  }
}
