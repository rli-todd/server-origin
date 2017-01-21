CREATE TABLE [dbo].[GeoIP]
(
[IntIpAddrStart] [int] NULL,
[IntIpAddrEnd] [int] NULL,
[GeoLocationID] [int] NULL
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_GeoIP_End] ON [dbo].[GeoIP] ([IntIpAddrEnd]) INCLUDE ([GeoLocationID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_GeoIP_Start] ON [dbo].[GeoIP] ([IntIpAddrStart]) INCLUDE ([GeoLocationID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoIP] ADD CONSTRAINT [FK_GeoIP_GeoLocation] FOREIGN KEY ([GeoLocationID]) REFERENCES [dbo].[GeoLocation] ([ID])
GO
GRANT SELECT ON  [dbo].[GeoIP] TO [public]
GO
