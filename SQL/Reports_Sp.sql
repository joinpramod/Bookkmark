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

