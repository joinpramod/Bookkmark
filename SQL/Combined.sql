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

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[14] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users (dbo)"
            Begin Extent = 
               Top = 94
               Left = 313
               Bottom = 341
               Right = 629
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Domain (dbo)"
            Begin Extent = 
               Top = 101
               Left = 1049
               Bottom = 348
               Right = 1365
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
         Width = 750
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VwDomains'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'VwDomains'
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

