SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spProfileSave](
	@ProfileID CHAR(11), 
	@ProfileAttributes VARCHAR(255), 
	@CompressedJson VARBINARY(MAX),
	@DurationMsecs INT) AS
	SET NOCOUNT ON
	IF EXISTS (
		SELECT 1 
			FROM Profile 
			WHERE ProfileID=@ProfileID
			AND ProfileAttributes=@ProfileAttributes
	)
		UPDATE Profile SET 
				CompressedJson=@CompressedJson,
				DurationMsecs=@DurationMsecs,
				DateCached=GETDATE()
			WHERE ProfileID=@ProfileID
	ELSE
		INSERT INTO Profile(ProfileID,ProfileAttributes,DurationMsecs,CompressedJson,DateCached)
			VALUES(@ProfileID,@ProfileAttributes,@DurationMsecs,@CompressedJson,GETDATE())
GO
