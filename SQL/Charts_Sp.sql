USE [booqmarqs]
GO
/****** Object:  StoredProcedure [dbo].[Reports_Sp]    Script Date: 12/31/2017 1:05:38 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Charts_Sp]
@DomainCreatedUser As Numeric(9,0) = NULL,
@DomainId As Numeric(9,0) = NULL

As  

  DECLARE @domainName nvarchar(300);  
  set @domainName = (select DomainName from VwDomains where DomainCreatedUser = @DomainCreatedUser 
  and DomainId = @DomainId)
  
  if(@domainName!= null)
  begin
  SELECT url, COUNT(url) AS URLCount from (SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count
				FROM bookmarks  where url like '%' + @domainName + '%' and IsFolder = 0 GROUP BY url, createddatetime, city, country, Id, FolderId)
				as bookmarks
				GROUP BY url	
	   return
  end
