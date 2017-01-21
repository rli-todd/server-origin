
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spViewCreateIndexes]( @DropIndex BIT=0, @CreateIndex BIT=1) AS

	SET NOCOUNT ON


	

	EXEC spViewCreateIndex 
		@ViewName='xvCityStateInfoByName',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='CityAndState'

	EXEC spViewCreateIndex 
		@ViewName='xvFips',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='StateFips,CityFips'

	EXEC spViewCreateIndex 
		@ViewName='xvGeoLocation',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='GeoLocationID'

	EXEC spViewCreateIndex 
		@ViewName='xvStatePopulation',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='StateFips'

	EXEC spViewCreateIndex 
		@ViewName='xvStatePopulation',
		@NameSuffix='_Population',
		@IndexType='NONCLUSTERED',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='Population',
		@IncludeColumns='StateFips,StateName'

	EXEC spViewCreateIndex 
		@ViewName='xvStateCountyPopulation',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='StateFips,CountyFips'

	EXEC spViewCreateIndex 
		@ViewName='xvStateCountyPopulation',
		@NameSuffix='_Population',
		@IndexType='NONCLUSTERED',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='Population',
		@IncludeColumns='StateFips,StateName,CountyFips,CountyName'

	EXEC spViewCreateIndex 
		@ViewName='xvStateCityPopulation',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='StateFips,CityFips'

	EXEC spViewCreateIndex 
		@ViewName='xvStateCityPopulation',
		@NameSuffix='_Population',
		@IndexType='NONCLUSTERED',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='Population',
		@IncludeColumns='StateFips,StateName,CityFips,CityName'

	EXEC spViewCreateIndex 
		@ViewName='xvStateHouseholdIncome',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='StateFips,ItemCode'

	EXEC spViewCreateIndex 
		@ViewName='xvStateCityHouseholdIncome',
		@DropIndex=@DropIndex,@CreateIndex=@CreateIndex,
		@IndexColumns='StateFips,CityFips,ItemCode'


GO
