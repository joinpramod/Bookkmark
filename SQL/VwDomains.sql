USE [bookmarks]
GO

/****** Object:  View [dbo].[VwDomains]    Script Date: 12/21/2017 4:16:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[VwDomains]
AS
SELECT        dbo.Users.Email, dbo.Users.IsPublisher, dbo.Users.IsWebUser, dbo.Domain.CreatedUser AS DomainCreatedUser, dbo.Domain.Name AS DomainName, 
                         dbo.Domain.Id AS DomainId, dbo.Domain.Script, dbo.Domain.Width, dbo.Domain.Height, dbo.Domain.ShowCount
FROM            dbo.Users INNER JOIN
                         dbo.Domain ON dbo.Users.UserId = dbo.Domain.CreatedUser

GO

　
　
