SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spReportTypeInitAll] AS
	SET NOCOUNT ON
	DECLARE @T TABLE(TypeCode VARCHAR(3), Title VARCHAR(50), ProfileAttributes VARCHAR(255))
	INSERT INTO @T(TypeCode, Title,ProfileAttributes) VALUES 
		('PL','People Lookup','Name,Addresses,Phones,Relatives,Aliases,Phones,Professional,DOB'),
		('SCC','Statewide Criminal Check','Name,Addresses,Phones,Relatives,Aliases,Phones,Professional,CriminalRecords,DOB'),
		('NCC','Nationwide Criminal Check','Name,Addresses,Phones,Relatives,Aliases,Phones,Professional,CriminalRecords,DOB'),
		('BC','Background Check','Name,Addresses,Relatives,Aliases,Phones,Emails,Professional,BusinessRecords,CivilRecords,CriminalRecords,MarriageDivorceRecords,DOB')

	UPDATE ReportType SET Title=t.Title, ProfileAttributes=t.ProfileAttributes
		FROM ReportType rt
		JOIN @T t
			ON rt.TypeCode=t.TypeCOde

	INSERT INTO ReportType(SiteID,TypeCode,Title,ProfileAttributes)
		SELECT DISTINCT 1,TypeCode,Title, ProfileAttributes
			FROM @T t
			WHERE NOT EXISTS (
				SELECt 1
					FROM ReportType rt
					WHERE rt.TypeCode=t.TypeCode
			)
GO
