SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spScheduled_Nightly] AS
	SET NOCOUNT ON
  EXEC spUtilRebuildFragmentedIndexes
 

GO
