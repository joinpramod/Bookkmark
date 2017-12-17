USE [booqmarqs]
GO

/****** Object:  StoredProcedure [dbo].[Bookmarks_Sp]    Script Date: 12/17/2017 10:06:40 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Bookmarks_Sp]
@OptID as int = 1,
@Id As Numeric(9,0) = NULL,
@URL As VarChar(50) = NULL,
@FolderId As Numeric(9,0) = NULL,
@IsFolder As VarChar(50) = NULL,
@Name As VarChar(50) = NULL,
@IpAddr As VarChar(50) = NULL,
@City As VarChar(50) = NULL,
@Country As VarChar(50) = NULL,
@CreatedUser As Numeric(9,0) = NULL,
@CreatedDateTime As DateTime  = NULL,
@ModifiedUser As Numeric(9,0) = NULL,
@ModifiedDateTime As DateTime  = NULL
   
As
   
   
      If @OptID = 1
          Begin
            Insert into Bookmarks(URL,FolderId,IsFolder,Name,IpAddr,City,Country,CreatedUser,CreatedDateTime) 
		Values(@URL,@FolderId,@IsFolder,@Name,@IpAddr,@City,@Country,@CreatedUser,@CreatedDateTime)
            Select @@identity as KeyID
          End
   
   
   
   
      If @OptID = 2
          Begin
             Update Bookmarks Set URL=@URL,FolderId=@FolderId,IsFolder=@IsFolder,Name=@Name,IpAddr=@IpAddr,City=@City,Country=@Country,ModifiedUser=@ModifiedUser,ModifiedDateTime=@ModifiedDateTime where Id = @Id
          End
   
   
   
   
      If @OptID = 3
          Begin
            Delete from Bookmarks where Id = @Id
          End
   
   
   
   
      If @OptID = 4
          Begin
            Select * From Bookmarks where Id = @Id
              Return
          End


		   If @OptID = 5
          Begin
             Update Bookmarks Set URL=@URL,FolderId=@FolderId,Name=@Name,ModifiedUser=@ModifiedUser,ModifiedDateTime=@ModifiedDateTime where Id = @Id
		End
   
    If @OptID = 6
          Begin
             Update Bookmarks Set URL=@URL,FolderId=@FolderId,Name=@Name,ModifiedUser=@ModifiedUser,ModifiedDateTime=@ModifiedDateTime where Id = @Id
          End


GO


USE [booqmarqs]
GO

/****** Object:  Table [dbo].[Domain]    Script Date: 12/17/2017 10:04:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Domain](
	[Id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Script] [varchar](300) NULL,
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


USE [booqmarqs]
GO

/****** Object:  StoredProcedure [dbo].[Domain_Sp]    Script Date: 12/17/2017 10:06:54 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[Domain_Sp]
@OptID as int = 1,
@Id As Numeric(9,0) = NULL,
@Name As VarChar(50) = NULL,
@Script As VarChar(150) = NULL,
@CreatedUser As Numeric(9,0) = NULL,
@CreatedDateTime As DateTime  = NULL,
@ModifiedDateTime As DateTime  = NULL
   
As
   
   
      If @OptID = 1
          Begin
            Insert into Domain(Name,Script,CreatedUser,CreatedDateTime) 
		Values(@Name,@Script,@CreatedUser,@CreatedDateTime)
            Select @@identity as KeyID
          End
   
   
   
   
      If @OptID = 2
          Begin
             Update Domain Set Name=@Name,Script=@Script,ModifiedDateTime=@ModifiedDateTime where Id = @Id
          End
   
   
   
   
      If @OptID = 3
          Begin
            Delete from Domain where Id = @Id
          End
   
   
   
   
      If @OptID = 4
          Begin
            Select * From Domain where Id = @Id
              Return
          End



GO


USE [booqmarqs]
GO

/****** Object:  StoredProcedure [dbo].[Reports_Sp]    Script Date: 12/17/2017 10:07:05 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Reports_Sp]
@DomainCreatedUser As Numeric(9,0) = NULL,
@DomainId As Numeric(9,0) = NULL

As
   
  DECLARE @domainName nvarchar(300);  

  set @domainName = (select DomainName from VwDomains where DomainCreatedUser = @DomainCreatedUser 
  and DomainId = @DomainId)

  if(@domainName!= null)

  begin
	   select * from bookmarks where url like '%' + @domainName + '%'
	   return
  end


GO


USE [booqmarqs]
GO

/****** Object:  Table [dbo].[Suggestion]    Script Date: 12/17/2017 10:05:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Suggestion](
	[SuggestionId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[Suggestion] [varchar](1000) NULL,
	[CreatedUser] [numeric](18, 0) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Suggestion] PRIMARY KEY CLUSTERED 
(
	[SuggestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [booqmarqs]
GO

/****** Object:  StoredProcedure [dbo].[Suggestion_Sp]    Script Date: 12/17/2017 10:07:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Suggestion_Sp]
@OptID as int = 1,
@SuggestionId As Numeric(9,0) = NULL,
@Suggestion As VarChar(1000) = NULL,
@CreatedUser As Numeric(9,0) = NULL,
@CreatedDate As DateTime  = NULL
   
As
   
   
      If @OptID = 1
          Begin
            Insert into Suggestion(Suggestion,CreatedUser,CreatedDate) Values(@Suggestion,@CreatedUser,@CreatedDate)
            Select @@identity as KeyID
          End
   
   
   
   
      If @OptID = 2
          Begin
             Update Suggestion Set Suggestion=@Suggestion,CreatedUser=@CreatedUser,CreatedDate=@CreatedDate where SuggestionId = @SuggestionId
          End
   
   
   
   
      If @OptID = 3
          Begin
            Delete from Suggestion where SuggestionId = @SuggestionId
          End
   
   
   
   
      If @OptID = 4
          Begin
            Select * From Suggestion where SuggestionId = @SuggestionId
              Return
          End
   
   




GO


USE [booqmarqs]
GO

/****** Object:  StoredProcedure [dbo].[User_Sp]    Script Date: 12/17/2017 10:07:34 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[User_Sp]
@OptID as int = 1,
@UserId As Numeric(9,0) = NULL,
@FirstName As VarChar(50) = NULL,
@LastName As VarChar(50) = NULL,
@Password As VarChar(50) = NULL,
@Email As VarChar(50) = NULL,
@Status As VarChar(50) = NULL,
@IsPublisher As VarChar(50) = NULL,
@IsWebUser As VarChar(2000) = NULL,
@CreatedDateTime As DateTime  = NULL,
@ModifiedDateTime As DateTime  = NULL
   
As
   
   
      If @OptID = 1
          Begin
            Insert into Users(FirstName,LastName,Password,Email,Status,IsPublisher,IsWebUser,CreatedDateTime,ModifiedDateTime) 
		Values(@FirstName,@LastName,@Password,@Email,@Status,@IsPublisher,@IsWebUser,@CreatedDateTime,@ModifiedDateTime)
            Select @@identity as KeyID
          End
   
   
   
   
      If @OptID = 2
          Begin
             Update Users Set FirstName=@FirstName,LastName=@LastName,Password=@Password,Email=@Email,Status=@Status,IsPublisher=@IsPublisher,IsWebUser=@IsWebUser,CreatedDateTime=@CreatedDateTime,ModifiedDateTime=@ModifiedDateTime where UserId = @UserId
          End
   
   
   
   
      If @OptID = 3
          Begin
            Delete from Users where UserId = @UserId
          End
   
   
   
   
      If @OptID = 4
          Begin
            Select * From Users where UserId = @UserId
              Return
          End


		   If @OptID = 5
          Begin
             Update Users Set FirstName=@FirstName,LastName=@LastName,Email=@Email,Status=@Status,CreatedDateTime=@CreatedDateTime,ModifiedDateTime=@ModifiedDateTime where UserId = @UserId
          End
   
    If @OptID = 6
          Begin
             Update Users Set Password=@Password,ModifiedDateTime=@ModifiedDateTime where UserId = @UserId
          End

		  
		   If @OptID = 7
          Begin
             Update Users Set FirstName=@FirstName,LastName=@LastName,Email=@Email,Status=@Status,CreatedDateTime=@CreatedDateTime,ModifiedDateTime=@ModifiedDateTime where UserId = @UserId
          End




GO


USE [booqmarqs]
GO

/****** Object:  Table [dbo].[UserActivation]    Script Date: 12/17/2017 10:05:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserActivation](
	[ActId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UserId] [numeric](18, 0) NULL,
	[EMailId] [varchar](100) NULL,
	[ActivationCode] [varchar](50) NULL,
 CONSTRAINT [PK_ActivationCode] PRIMARY KEY CLUSTERED 
(
	[ActId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [booqmarqs]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 12/17/2017 10:05:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[IsPublisher] [bit] NULL,
	[IsWebUser] [bit] NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


USE [booqmarqs]
GO

/****** Object:  View [dbo].[VwDomains]    Script Date: 12/17/2017 10:06:00 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VwDomains]
AS
SELECT dbo.Users.Email, dbo.Users.IsPublisher, dbo.Users.IsWebUser, dbo.Domain.CreatedUser AS DomainCreatedUser, dbo.Domain.Name AS DomainName, dbo.Domain.Id AS DomainId, dbo.Domain.Script
FROM  dbo.Users INNER JOIN
         dbo.Domain ON dbo.Users.UserId = dbo.Domain.CreatedUser

GO


USE [booqmarqs]
GO

/****** Object:  View [dbo].[VwUserBookmarks]    Script Date: 12/17/2017 10:06:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[VwUserBookmarks]
AS
SELECT        dbo.Bookmarks.Id, dbo.Bookmarks.URL, dbo.Bookmarks.FolderId, dbo.Bookmarks.IsFolder, dbo.Bookmarks.Name, dbo.Bookmarks.IpAddr, dbo.Bookmarks.City, dbo.Bookmarks.Country, 
                         dbo.Bookmarks.CreatedUser, dbo.Bookmarks.CreatedDateTime, dbo.Bookmarks.ModifiedUser, dbo.Bookmarks.ModifiedDateTime, dbo.Users.Email, dbo.Users.IsPublisher, dbo.Users.IsWebUser
FROM            dbo.Bookmarks INNER JOIN
                         dbo.Users ON dbo.Bookmarks.CreatedUser = dbo.Users.UserId


GO


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

