IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'api_dev')
CREATE LOGIN [api_dev] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [api_dev] FOR LOGIN [api_dev]
GO
