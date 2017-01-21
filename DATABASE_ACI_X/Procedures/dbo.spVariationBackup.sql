SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBackupVariations AS
  SET NOCOUNT ON
  DECLARE @BackupDate DATETIME=GETDATE()

  INSERT INTO BackupVariation
    SELECT @BackupDate,v.*
      FROM Variation v

  INSERT INTO BackupBlock 
    SELECT @BackupDate,v.*
      FROM Block v

  INSERT INTO BackupSection 
    SELECT @BackupDate,v.*
      FROM Section v

  INSERT INTO BackupPage 
    SELECT @BackupDate,v.*
      FROM Page v

  INSERT INTO BackupPageBlock 
    SELECT @BackupDate,v.*
      FROM PageBlock v

GO
