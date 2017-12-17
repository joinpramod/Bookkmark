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

