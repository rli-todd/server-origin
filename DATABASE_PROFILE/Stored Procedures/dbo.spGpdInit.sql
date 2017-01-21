
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGpdInit]( @InitCityLookupSummary BIT=1) AS
  SET NOCOUNT ON
  EXEC spPrint 'Started'
  EXEC spGpdBuildStop
  EXEC spPrint 'All previous build jobs stopped'
  TRUNCATE TABLE GpdNode
  TRUNCATE TABLE GpdNextLetter
  TRUNCATE TABLE GpdTopN
  TRUNCATE TABLE GpdBuildStats
  EXEC spPrint 'Tables truncated'
  --EXEC spLastNameCreatePartial
  --EXEC spFirstNameCreatePartial
  --EXEC spPrint 'Partial names created'
  IF @InitCityLookupSummary=0
    EXEC spPrint 'Skipping initialization of CityLookupSummary'
  ELSE 
  BEGIN
    EXEC spCityLookupSummaryInit
    EXEC spPrint 'CityLookupSummary initialized'
  END
GO
