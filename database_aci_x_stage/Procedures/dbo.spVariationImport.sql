SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
/*
http://www.excel-sql-server.com/excel-import-to-sql-server-using-linked-servers.htm

EXEC sp_dropserver
    @server = N'Variation_Templates',
    @droplogins='droplogins'

EXEC sp_addlinkedserver
    @server = 'Variation_Templates',
    @srvproduct = 'Excel', 
    @provider = 'Microsoft.ACE.OLEDB.12.0',
    @datasrc = 'D:\Data\Variation_Templates.xlsx',
    @provstr = 'Excel 12.0;IMEX=1;HDR=YES;'

EXEC sp_MSset_oledb_prop N'Microsoft.ACE.OLEDB.12.0', N'AllowInProcess', 1
EXEC sp_MSset_oledb_prop N'Microsoft.ACE.OLEDB.12.0', N'DynamicParameters', 1
EXEC sp_addlinkedsrvlogin 
	@rmtsrvname = N'Variation_Templates', 
	@locallogin = 'sa' , 
	@useself = N'False', 
	@rmtuser = NULL,
	@rmtpassword = NULL

*/
CREATE PROC [dbo].[spVariationImport] 
AS
  SET NOCOUNT ON

  TRUNCATE TABLE VariationImport
  
  DECLARE @SQL NVARCHAR(MAX)='
  WITH cte AS
  (
    SELECT Valid,Done,PageCode,BlockNum,BlockName,BlockType,SectionNum,SectionName,
                              SectionType,
                              CONVERT(VARCHAR(MAX),Description)''Description'',
                              CONVERT(VARCHAR(MAX),MultirowPrefix)''MultirowPrefix'',
                              CONVERT(VARCHAR(MAX),MultirowSuffix)''MultirowSuffix'',
                              CONVERT(VARCHAR(MAX),MultirowDelimiter)''MultirowDelimiter'',
                              CONVERT(VARCHAR(MAX),ViewName)''ViewName'',
                              CONVERT(VARCHAR(MAX),ViewFieldNames)''ViewFieldNames'',
                              CONVERT(VARCHAR(MAX),HeaderTemplate)''HeaderTemplate'',
                              CONVERT(VARCHAR(MAX),HeaderDefault)''HeaderDefault'',
                              CONVERT(VARCHAR(MAX),BodyTemplate)''BodyTemplate'',
                              CONVERT(VARCHAR(MAX),BodyDefault)''BodyDefault''
      FROM [Variation_Templates]...[Variation_Templates$]
      WHERE Valid=''y''
      AND (BodyTemplate IS NOT NULL OR HeaderTemplate IS NOT NULL)
  )
    INSERT INTO VariationImport(Valid,Done,PageCode,BlockNum,BlockName,BlockType,SectionNum,SectionName,
                              SectionType,Description,MultirowPrefix,MultirowSuffix,MultirowDelimiter,
                              ViewName,ViewFieldNames,HeaderTemplate,HeaderDefault,BodyTemplate,BodyDefault)
      SELECT Valid,Done,PageCode,BlockNum,BlockName,BlockType,SectionNum,SectionName,
                              SectionType,
                              CASE Description WHEN ''NULL'' THEN NULL ELSE Description END,
                              CASE MultirowPrefix WHEN ''NULL'' THEN NULL ELSE MultirowPrefix END,
                              CASE MultirowSuffix WHEN ''NULL'' THEN NULL ELSE MultirowSuffix END,
                              CASE MultirowDelimiter WHEN ''NULL'' THEN NULL ELSE MultirowDelimiter END,
                              CASE ViewName WHEN ''NULL'' THEN NULL ELSE ViewName END,
                              CASE ViewFieldNames WHEN ''NULL'' THEN NULL ELSE ViewFieldNames END,
                              CASE HeaderTemplate WHEN ''NULL'' THEN NULL ELSE HeaderTemplate END,
                              CASE HeaderDefault WHEN ''NULL'' THEN NULL ELSE HeaderDefault END,
                              CASE BodyTemplate WHEN ''NULL'' THEN NULL ELSE BodyTemplate END,
                              CASE BodyDefault WHEN ''NULL'' THEN NULL ELSE BodyDefault END
      FROM cte'

  PRINT @SQL
  EXEC (@SQL)
  EXEC spVariationBackup
  SET ROWCOUNT 0
  SELECT DISTINCT PageCode
    INTO #page
    FROM VariationImport vi

  SELECT DISTINCT Valid,PageCode,PageCode+':'+BlockName'BlockName',ISNULL(BlockType,'text')'BlockType'
    INTO #block
    FROM VariationImport vi

  SELECT DISTINCT Valid,PageCode+':'+BlockName'BlockName',PageCode+':'+BlockName+':'+SectionName'SectionName',ISNULL(SectionType,'NormalText')'SectionType'
    INTO #section
    FROM VariationImport vi

  SELECT DISTINCT 
      Valid,
      PageCode+':'+BlockName+':'+SectionName'SectionName',
      Description,
      MultirowPrefix,
      MultirowDelimiter,
      MultirowSuffix,
      ViewName,
      ViewFieldNames,
      HeaderTemplate,
			HeaderDefault,
      BodyTemplate,
			BodyDefault
    INTO #variation
    FROM VariationImport vi

  DELETE PageBlock
  DELETE Variation
  DELETE Section
  DELETE Block
  
  INSERT INTO Page(PageCode,Description)
    SELECT DISTINCT PageCode,PageCode
      FROM #page tp
      WHERE PageCode IS NOT NULL
      AND NOT EXISTS 
      (
        SELECT 1
          FROM Page p
          WHERE p.PageCode=tp.PageCode
      );

  INSERT INTO Block(IsEnabled,BlockName,BlockType)
    SELECT CASE WHEN Valid='y' THEN 1 ELSE 0 END,BlockName,BlockType
      FROM #block tb

  INSERT INTO PageBlock(PageID,BlockID)
    SELECT DISTINCT p.ID,b.ID
      FROM Page p
      JOIN #block tb
        ON tb.PageCode=p.PageCode
      JOIN Block b
        ON b.BlockName=tb.BlockName

  INSERT INTO Section(BlockID,SectionName,SectionType,IsEnabled)
    SELECT DISTINCT b.ID,ts.SectionName,ts.SectionType,CASE WHEN Valid='y' THEN 1 ELSE 0 END
      FROM #section ts
      JOIN Block b
        ON b.BLockName=ts.BlockName

  INSERT INTO Variation(
                SectionID,Description,MultirowPrefix,MultirowDelimiter,MultirowSuffix,
                HeaderTemplate,HeaderDefault,BodyTemplate,BodyDefault,ViewName,ViewFieldNames,IsEnabled)
    SELECT DISTINCT s.ID,tv.Description,tv.MultirowPrefix,tv.MultirowDelimiter,tv.MultirowSuffix,
            tv.HeaderTemplate,tv.HeaderDefault,tv.BodyTemplate,tv.BodyDefault,tv.ViewName,tv.ViewFieldNames, CASE WHEN Valid='y' THEN 1 ELSE 0 END
      FROM #variation tv
      JOIN Section s
        ON s.SectionName=tv.SectionName

  SELECT 'VariationImport' 'Table',* FROM VariationImport
  SELECT '#Page' 'Table',* FROM #Page
  SELECT 'Page' 'Table',* FROM Page
  SELECT '#Block' 'Table',* FROM #Block
  SELECT 'Block' 'Table',* FROM Block
  SELECT '#Section' 'Table',* FROM #Section
  SELECT 'Section' 'Table',* FROM Section
  SELECT '#Variation' 'Table',* FROM #Variation
  SELECT 'Variation' 'Table',* FROM Variation
  SELECT 'PageBlock' 'Table',* FROM PageBLock

--  UPDATE Variation SET IsEnabled=0 WHERE ViewName IS NULL
GO
