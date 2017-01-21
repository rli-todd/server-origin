SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spUtilNormalizeStateCountyCity] AS
/*
** Normalizes the data imported into StateCountyCity from http://www.opengeocode.org/download.ph
** such that those cities that are associated with multiple Counties (i.e. they have more than
** one entry in the CountyFips and CountyName columns, separated by commas) get a new distinct 
** row in StateCountyCity
*/
DECLARE 
  @StateAbbr CHAR(2),
  @StateFips VARCHAR(10),
  @CityName VARCHAR(255),
  @CityFips VARCHAR(10),
  @CountyFips VARCHAR(255),
  @COuntyName VARCHAR(255)

DECLARE c CURSOR FOR SELECT StateAbbr,StateFips,CityName,CityFips,CountyFips,CountyName
  FROM StateCountyCity
  WHERE CHARINDEX(',',CountyFips)>0

DECLARE @Fips TABLE(ID INT,Data VARCHAR(255))
DECLARE @Names TABLE(ID INT,Data VARCHAR(255))

OPEN c
FETCH NEXT FROM c INTO @StateAbbr,@StateFips,@CityName,@CityFips,@CountyFips,@CountyName
WHILE @@FETCH_STATUS=0
BEGIN
  INSERT INTO @Fips(ID,Data) SELECT ID,Data FROM dbo.fnSplit(@CountyFips,',')
  INSERT INTO @Names(ID,Data) SELECT ID,Data FROM dbo.fnSplit(@CountyName,',')
  INSERT INTO StateCountyCity(StateAbbr,StateFips,CityName,CityFips,CountyFips,CountyName)
    SELECT @StateAbbr,@StateFips,@CityName,@CityFips,f.Data,n.Data
      FROM @Fips f
      JOIN @Names n
        ON f.ID=n.ID
  DELETE @Fips
  DELETE @Names
  FETCH NEXT FROM c INTO @StateAbbr,@StateFips,@CityName,@CityFips,@CountyFips,@CountyName
END
CLOSE c
DEALLOCATE c


DELETE StateCountyCity
  WHERE CHARINDEX(',',CountyFips)>0
GO
