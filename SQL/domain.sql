USE [bookmarks]
GO

/****** Object:  Table [dbo].[Domain]    Script Date: 12/22/2017 2:51:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Domain](
	[Id] [numeric](18, 0) IDENTITY(100,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Script] [varchar](300) NULL,
	[Height] [nvarchar](10) NULL,
	[Width] [nvarchar](10) NULL,
	[ShowCount] [bit] NULL,
	[CreatedUser] [numeric](18, 0) NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
 CONSTRAINT [PK_Domain] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

　
　
