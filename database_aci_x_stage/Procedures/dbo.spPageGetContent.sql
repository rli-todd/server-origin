SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spPageGetContent]( @PageCode VARCHAR(30), @WhereClause VARCHAR(MAX)=NULL, @VisitID INT=NULL) AS
  SET NOCOUNT ON;


  WITH _variation_index AS
  (
    SELECT pb.BlockID,SectionID,BlockName,SectionName,VariationCount,BlockType,SectionType,
            dbo.fnHashModString(VariationCount, BlockName+SectionName+ISNULL(@WHereClause,''))'VariationIndex'
      FROM Page p
      JOIN PageBlock pb
        ON pb.PageID=p.ID
      JOIN xvVariationCounts x WITH(NOEXPAND)
        ON x.BlockID=pb.BlockID
      WHERE PageCode=@PageCode
  ),
  _variation AS
  (
    SELECT 
        v.ID'VariationID',
        v.SectionID,
        v.Description,
        v.HeaderTemplate,
				v.HeaderDefault,
        v.BodyTemplate,
				v.BodyDefault,
        v.ViewName,
        v.ViewFieldNames,
        v.MultirowPrefix,
        v.MultirowSuffix,
        v.MultirowDelimiter,
        CONVERT(INT,ROW_NUMBER() OVER(PARTITION BY v.SectionID ORDER BY v.ID)-1)'VariationIndex'
      FROM Variation v
      JOIN _variation_index vi
				ON vi.SectionID=v.SectionID
      WHERE v.IsEnabled=1
  )
    SELECT BlockName,SectionName,BlockType,SectionType,v.*
      INTO #selected_variations
      FROM _variation v
      JOIN _variation_index vi
        ON vi.SectionID=v.SectionID
        AND vi.VariationIndex=v.VariationIndex
    /*
    ** First return the selected variations
    */
    SELECT * FROM #selected_variations

    DECLARE @SQL NVARCHAR(MAX)=''
    /*
    ** TODO: Need to update fnConcatenateNoDups to allow a (space) delimeter so that we can 
    ** avoid some of the problems that come when the query optimizer messes up this method
    ** of concatenation.
    */
    SELECT @SQL = @SQL + '

    SELECT DISTINCT
      '+CONVERT(NVARCHAR,SectionID)+'''SectionID'','+
      CONVERT(NVARCHAR,VariationID)+'''VariationID'','+
      ViewFieldNames+'
      FROM '+ViewName+
			CASE WHEN ISNULL(@WhereClause,'')='' THEN '' ELSE ' 
      WHERE '+@WhereClause END
      FROM #selected_variations
      WHERE ISNULL(ViewFieldNames,'')<>''
      AND ISNULL(ViewName,'')<>'' 

    EXEC spPrint @SQL
    EXEC (@SQL)
GO
