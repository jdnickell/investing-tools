USE [InvestingTools]
GO
/****** Object:  Table [dbo].[ExtendedHoursBiggestMovers]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtendedHoursBiggestMovers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SymbolId] [int] NOT NULL,
	[PriceAfterHours] [decimal](18, 2) NOT NULL,
	[PriceOpen] [decimal](18, 2) NOT NULL,
	[PriceClose] [decimal](18, 2) NOT NULL,
	[MarketDate] [date] NOT NULL,
	[CreatedDateTimeUtc] [datetime] NOT NULL,
 CONSTRAINT [PK_ExtendedHoursBiggestMovers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recommendation]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recommendation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RecommendationTypeId] [int] NOT NULL,
	[EventDateTime] [datetime] NOT NULL,
	[CurrentPrice] [decimal](18, 2) NOT NULL,
	[Ticker] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Recommendation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecommendationActionType]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecommendationActionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RecommendationActionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Source]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Source](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Url] [varchar](200) NULL,
	[Username] [varchar](200) NULL,
	[Password] [varchar](200) NULL,
	[ApiKey] [varchar](150) NULL,
	[IsSandbox] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Source] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Symbol]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Symbol](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NULL,
	[Symbol] [varchar](10) NOT NULL,
	[SymbolTypeId] [int] NOT NULL,
 CONSTRAINT [PK_Symbol] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Symbol_Source]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Symbol_Source](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SymbolId] [int] NOT NULL,
	[SourceId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Symbol_Source] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SymbolNews]    Script Date: 6/14/2021 7:53:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SymbolNews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SymbolId] [int] NOT NULL,
	[PublisherName] [varchar](500) NULL,
	[PublisherUrl] [varchar](MAX) NULL,
	[Title] [varchar](500) NULL,
	[Summary] [varchar](MAX) NULL,
	[Author] [varchar](500) NULL,
	[PublishedDateTime] [datetime] NULL,
	[Url] [varchar](MAX) NULL,
	[RelevantSymbolsCSV] [varchar](MAX) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_SymbolNews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ExtendedHoursBiggestMovers]  WITH CHECK ADD  CONSTRAINT [FK_ExtendedHoursBiggestMovers_Symbol] FOREIGN KEY([SymbolId])
REFERENCES [dbo].[Symbol] ([Id])
GO
ALTER TABLE [dbo].[ExtendedHoursBiggestMovers] CHECK CONSTRAINT [FK_ExtendedHoursBiggestMovers_Symbol]
GO
ALTER TABLE [dbo].[Symbol_Source]  WITH CHECK ADD  CONSTRAINT [FK_Symbol_Source_Source] FOREIGN KEY([SourceId])
REFERENCES [dbo].[Source] ([Id])
GO
ALTER TABLE [dbo].[Symbol_Source] CHECK CONSTRAINT [FK_Symbol_Source_Source]
GO
ALTER TABLE [dbo].[Symbol_Source]  WITH CHECK ADD  CONSTRAINT [FK_Symbol_Source_Symbol] FOREIGN KEY([SymbolId])
REFERENCES [dbo].[Symbol] ([Id])
GO
ALTER TABLE [dbo].[Symbol_Source] CHECK CONSTRAINT [FK_Symbol_Source_Symbol]
GO
ALTER TABLE [dbo].[SymbolNews]  WITH CHECK ADD  CONSTRAINT [FK_SymbolNews_SymbolNews] FOREIGN KEY([SymbolId])
REFERENCES [dbo].[Symbol] ([Id])
GO
ALTER TABLE [dbo].[SymbolNews] CHECK CONSTRAINT [FK_SymbolNews_SymbolNews]
GO
