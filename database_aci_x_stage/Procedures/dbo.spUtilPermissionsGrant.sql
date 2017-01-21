SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[spUtilPermissionsGrant] AS
  EXEC spPrint 'spUtilPermissionsGrant starting'
	GRANT SELECT ON vOrderSummary TO analytics_readers

EXEC spPrint 'spUtilPermissionsGrant completed'
GO
