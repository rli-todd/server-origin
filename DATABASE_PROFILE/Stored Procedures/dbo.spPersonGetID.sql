
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spPersonGetID](
  @ProfileID CHAR(11), 
  @FirstNameID INT, 
  @MiddleNameID INT,
  @LastNameID INT,
  @DateOfBirth DATE=NULL, 
  @PhoneCount TINYINT=NULL,
  @EmailCount TINYINT=NULL,
  @ListSchoolIDs VARCHAR(MAX)=NULL,
  @ListRelativeIDs VARCHAR(MAX)=NULL,
  @ListCompanyIDs VARCHAR(MAX)=NULL,
  @ListGeoLocationIDs VARCHAR(MAX)=NULL,
  @ListAliasIDs VARCHAR(MAX)=NULL) AS
  SET NOCOUNT ON

  DECLARE 
    @PersonID INT,
	  @CurrentFirstNameID INT,
	  @CUrrentLastNameID INT,
	  @CurrentMiddleNameID INT,
	  @CurrentDateOfBirth DATE,
	  @CurrentPhoneCount TINYINT,
	  @CurrentEmailCount TINYINT

  SELECT 
		@PersonID=ID,
		@CurrentFirstNameID=FirstNameID,
		@CurrentMiddleNameID=MiddleNameID,
		@CurrentLastNameID=LastNameID,
		@CurrentDateOfBirth=DateOfBirth,
		@CurrentPhoneCount=PhoneCount,
		@CurrentEmailCount=EmailCount
    FROM Person
    WHERE ProfileID=@ProfileID

  --EXEC spPrint '@PersonID=',@PersonID
  --EXEC spPrint '@CurrentFIrstNameID=',@CurrentFirstNameID
  --EXEC spPrint '@CurrentMiddleNameID=',@CurrentMiddleNameID
  --EXEC spPrint '@CurrentLastNameID=',@CUrrentLastNameID
  --EXEC spPrint '@CurrentDateOfBirth=',@CurrentDateOfBirth
  --EXEC spPrint '@CurrentPhoneCount=',@CurrentPhoneCount
  --EXEC spPrint '@CurrentEmailCount=',@CurrentEmailCount

  IF @PersonID IS NULL
  BEGIN
    INSERT INTO Person(ProfileID,FirstNameID,LastNameID,MiddleNameID,DateOfBirth,PhoneCount,EmailCount,DateModified)
      VALUES (@ProfileID,@FirstNameID,@LastNameID,@MiddleNameID,@DateOfBirth,@PhoneCount,@EmailCount,GETDATE())
    SET @PersonID=SCOPE_IDENTITY()
  END
  ELSE 
	IF ISNULL(@FirstNameID,0)<>ISNULL(@CurrentFirstNameID,@FirstNameID)
	OR ISNULL(@MiddleNameID,0)<>ISNULL(@CurrentMiddleNameID,@MiddleNameID)
	OR ISNULL(@LastNameID,0)<>ISNULL(@CurrentLastNameID,@LastNameID)
	OR (@DateOfBirth IS NOT NULL AND @DateOfBirth <>ISNULL(@CurrentDateOfBirth,'1/1/2000'))
	OR (@PhoneCount IS NOT NULL AND @PhoneCount <>ISNULL(@CurrentPhoneCount,0))
	OR (@EmailCount IS NOT NULL AND @EmailCount <>ISNULL(@CurrentEmailCount,0))
		UPDATE Person SET 
		  FirstNameID=ISNULL(@FirstNameID,FirstNameID),
		  LastNameID=ISNULL(@LastNameID,LastNameID),
		  MiddleNameID=ISNULL(@MiddleNameID,MiddleNameID),
		  DateOfBirth=ISNULL(@DateOfBirth,DateOfBirth),
		  PhoneCount=ISNULL(@PhoneCount,PhoneCount),
		  EmailCount=ISNULL(@EmailCount,EmailCount),
		  DateModified=GETDATE()
		  WHERE ID=@PersonID
  IF @ListRelativeIDs IS NOT NULL
    WITH cte AS
    (
      SELECT CHARINDEX('|',Item)'IdxDelim',Item
        FROM dbo.fnStrToTable(@ListRelativeIDs,',')
    ),
    relatives AS
    (
      SELECT SUBSTRING(item,1,IdxDelim-1)'Relationship',SUBSTRING(item,IdxDelim+1,LEN(Item)-IdxDelim)'RelatedPersonID'
        FROM cte
    )
    INSERT INTO PersonRelative(PersonID,RelatedPersonID,Relationship)
      SELECT DISTINCT @PersonID,RelatedPersonID,CONVERT(CHAR(1),Relationship)
        FROM relatives r
        WHERE NOT EXISTS (
          SELECT 1
            FROM PersonRelative pr
            WHERE pr.PersonID=@PersonID
            AND pr.RelatedPersonID=r.RelatedPersonID
        )

  IF @ListSchoolIDs IS NOT NULL
    WITH schools AS
    (
      SELECT Item'SchoolID'
        FROM dbo.fnStrToTable(@ListSchoolIDs,',')
    )
    INSERT INTO PersonSchool(PersonID,SchoolID)
      SELECT DISTINCT @PersonID,SchoolID
        FROM schools s
        WHERE NOT EXISTS (
          SELECT 1
            FROM PersonSchool
            WHERE PersonID=@PersonID
            AND SchoolID=s.SchoolID
        )


  IF @ListCompanyIDs IS NOT NULL
    WITH companies AS
    (
      SELECT Item'CompanyID'
        FROM dbo.fnStrToTable(@ListCompanyIDs,',')
    )
    INSERT INTO PersonCompany(PersonID,CompanyID)
      SELECT DISTINCT @PersonID,CompanyID
        FROM companies c
        WHERE NOT EXISTS (
          SELECT 1
            FROM PersonCompany
            WHERE PersonID=@PersonID
            AND CompanyID=c.CompanyID
        )

  IF @ListGeoLocationIDs IS NOT NULL
    WITH geos AS
    (
      SELECT Item'GeoLocationID'
        FROM dbo.fnStrToTable(@ListGeoLocationIDs,',')
    )
    INSERT INTO PersonGeoLocation(PersonID,GeoLocationID,CityFips)
      SELECT DISTINCT @PersonID,GeoLocationID,CityFips
        FROM geos g
        JOIN SolishineGeo..GeoLocation gl
          ON g.GeoLocationID=gl.ID
        WHERE NOT EXISTS (
          SELECT 1
            FROM PersonGeoLocation
            WHERE PersonID=@PersonID
            AND GeoLocationID=g.GeoLocationID
        )
    
  IF @ListAliasIDs IS NOT NULL
    WITH aliases AS
    (
      SELECT Item'AliasID'
        FROM dbo.fnStrToTable(@ListAliasIDs,',')
    )
    INSERT INTO PersonAlias(PersonID,AliasID)
      SELECT DISTINCT @PersonID,AliasID
        FROM aliases a
        WHERE NOT EXISTS (
          SELECT 1
            FROM PersonAlias
            WHERE PersonID=@PersonID
            AND AliasID=a.AliasID
        )
    RETURN @PersonID
GO
