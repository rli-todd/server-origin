SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spCacheTrim( @Mins INT=30) AS
	SET NOCOUNT ON
	DELETE Cache WHERE DateCreated<DATEADD(minute,-@Mins,GETDATE())
GO