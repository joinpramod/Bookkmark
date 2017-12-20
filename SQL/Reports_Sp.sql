USE [bookmarks]

GO
/****** Object:  StoredProcedure [dbo].[Reports_Sp]    Script Date: 12/19/2017 9:27:12 AM ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO


　

ALTER PROCEDURE [dbo].[Reports_Sp]
@DomainCreatedUser As Numeric(9,0) = NULL,
@DomainId As Numeric(9,0) = NULL

As  

  DECLARE @domainName nvarchar(300);  
  set @domainName = (select DomainName from VwDomains where DomainCreatedUser = @DomainCreatedUser 
  and DomainId = @DomainId)
  
  if(@domainName!= null)
  begin
  --select * from bookmarks where url like '%' + @domainName + '%'
	   SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count
				FROM bookmarks  where url like '%' + @domainName + '%' and IsFolder = 0 GROUP BY url, createddatetime, city, country, Id, FolderId
	   return
  end


　
