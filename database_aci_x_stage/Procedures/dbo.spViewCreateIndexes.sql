SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spViewCreateIndexes]( @DropIndex BIT=0, @CreateIndex BIT=1) AS

	SET NOCOUNT ON

	EXEC spViewCreateIndex 
		@ViewName='xvVariationCounts',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='BlockID,SectionID'
GO
