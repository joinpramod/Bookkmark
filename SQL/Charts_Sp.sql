USE [bookmarks]
GO
/****** Object:  StoredProcedure [dbo].[Charts_Sp]    Script Date: 01/04/2018 14:30:20 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Charts_Sp]
@DomainCreatedUser As Numeric(9,0) = NULL,
@DomainId As Numeric(9,0) = NULL,
@OptId as Numeric(9,0) = 0
As  

  DECLARE @domainName nvarchar(300);  
  set @domainName = (select DomainName from VwDomains where DomainCreatedUser = @DomainCreatedUser 
  and DomainId = @DomainId)
  
  if(@domainName!= null)
  begin
  
 If @OptID = 1
 Begin
	SELECT url, COUNT(url) AS URLCount from (SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count
				FROM bookmarks  where url like '%' + @domainName + '%' and IsFolder = 0 GROUP BY url, createddatetime, city, country, Id, FolderId)
				as bookmarks
				GROUP BY url	
	   return
	  End
	   
 If @OptID = 2
 Begin
	SELECT city, COUNT(city) AS CityCount from (SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count
				FROM bookmarks  where url like '%' + @domainName + '%' and IsFolder = 0 GROUP BY url, createddatetime, city, country, Id, FolderId)
				as bookmarks
				GROUP BY city	
	   return
	   
	   End
	   
  end
