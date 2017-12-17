USE [booqmarqs]
GO

/****** Object:  Table [dbo].[Bookmarks]    Script Date: 12/17/2017 10:04:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Bookmarks](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](max) NULL,
	[FolderId] [numeric](18, 0) NOT NULL,
	[IsFolder] [bit] NULL,
	[Name] [varchar](100) NULL,
	[IpAddr] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[CreatedUser] [numeric](18, 0) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[ModifiedUser] [numeric](18, 0) NULL,
	[ModifiedDateTime] [datetime] NULL,
 CONSTRAINT [PK_Bookmarks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

