SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spInteliusAdminInit As
	SET NOCOUNT ON
	DECLARE @InteliusUsers TABLE (EmailAddress VARCHAR(255))
	INSERT INTO @InteliusUsers(EmailAddress) VALUES
	('todd@solishine.com'),
	('i-yunus@intelius.com'),
	('i-bmadaiah@intelius.com'),
	('i-nwany@intelius.com'),
	('i-apraveena@intelius.com'),
	('i-nparveen@intelius.com'),
	('i-dpeters@intelius.com '),
	('i-reshma@intelius.com '),
	('i-wpereira@intelius.com'),
	('Philip.Anthony@fadv.com'),
	('Nooreen.Azeem@fadv.com'),
	('Edle.Nirmala@fadv.com'),
	('Solomon.Yesudas@fadv.com'),
	('Paul.Jackson@fadv.com'),
	('slee@intelius.com'),
	('lduce@intelius.com'),
	('sschultz@intelius.com'),
	('jagen@intelius.com'),
	('btaylor@intelius.com'),
	('fcantua@intelius.com'),
	('lsybert@intelius.com'),
	('klambert@intelius.com'),
	('cwilliams@intelius.com'),
	('i-adhikary@intelius.com'),
	('i-csamuel@intelius.com'),
	('i-jkhan@intelius.com'),
	('i-knirmal@intelius.com'),
	('i-kthammaiah@intelius.com'),
	('i-manthony@intelius.com'),
	('i-mgall@intelius.com'),
	('i-ssiraj@intelius.com'),
	('i-vreddy@intelius.com'),
	('i-sagar@intelius.com'),
	('i-deepakm@intelius.com'),
	('i-djoshua@intelius.com'),
	('i-jkhanna@intelius.com'),
	('i-kprakash@intelius.com'),
	('i-rmane@intelius.com'),
	('i-sahari@intelius.com'),
	('i-ssubramanyam@intelius.com'),
	('i-saugustine@intelius.com'),
	('i-dignatius@intelius.com'),
	('i-mgogoi@intelius.com'),
	('i-sjoseph@intelius.com'),
	('i-fchandrashekar@intelius.com'),
	('i-mebenezer@intelius.com'),
	('i-sgupta@intelius.com'),
	('i-sathish@intelius.com'),
	('i-rbhushan@intelius.com'),
	('i-aahmed@intelius.com'),
	('i-edas@intelius.com'),
	('i-miyengar@intelius.com'),
	('i-shussain@intelius.com'),
	('i-hmanjunath@intelius.com'),
	('i-psahu@intelius.com'),
	('i-sjena@intelius.com'),
	('i-saditya@intelius.com'),
	('i-kkumar@intelius.com'),
	('i-aupadhyay@intelius.com'),
	('i-skarimullah@intelius.com'),
	('i-bsanthosh@intelius.com'),
	('i-prajalu@intelius.com'),
	('i-cmiller@intelius.com'),
	('i-hrozario@intelius.com'),
	('i-svimal@intelius.com'),
	('i-skumar@intelius.com'),
	('i-rsingh@intelius.com'),
	('i-akhanna@intelius.com'),
	('i-sdas@intelius.com'),
	('i-jantony@intelius.com'),
	('i-vthirumurthy@intelius.com'),
	('i-snath@intelius.com')

	INSERT INTO InteliusAdmin(EmailAddress)
		SELECT EmailAddress
			FROM @InteliusUsers i
			WHERE NOT EXISTS (
				SELECT 1
					FROM InteliusAdmin ia
					WHERE Ia.EMailAddress=i.EmailAddress
			)
	UPDATE Users SET IsBackofficeReader=1
		FROM Users u
		JOIN InteliusAdmin ia
			ON ia.EmailAddress=u.EmailAddress

	SELECT ID,EmailAddress
		FROM Users
		WHERE IsBackofficeReader=1

GO
