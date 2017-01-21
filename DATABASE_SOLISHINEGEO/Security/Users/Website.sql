IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'Website')
CREATE LOGIN [Website] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [Website] FOR LOGIN [Website]
GO
