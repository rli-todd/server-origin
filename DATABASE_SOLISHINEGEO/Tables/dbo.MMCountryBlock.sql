CREATE TABLE [dbo].[MMCountryBlock]
(
[Network] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IpAddrStart] [int] NULL,
[IpAddrEnd] [int] NULL,
[GeoNameID] [bigint] NULL,
[CountryIsoCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RegisteredCountryGeoNameID] [bigint] NULL,
[RegisteredCountryIsoCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RepresentedCountryGeoNameID] [bigint] NULL,
[RepresentedCountryIsoCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsAnonymousProxy] [bit] NULL,
[IsSatelliteProvider] [bit] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [CIX_MMCountryBlock] ON [dbo].[MMCountryBlock] ([IpAddrStart], [IpAddrEnd]) ON [PRIMARY]
GO
