USE [bookmarks]
GO
/****** Object:  StoredProcedure [dbo].[Bookmarks_Sp]    Script Date: 12/26/2017 12:46:49 PM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

　
　
　
ALTER PROCEDURE [dbo].[Bookmarks_Sp]
@OptID as int = 1,
@Id As Numeric(9,0) = NULL,
@URL As VarChar(500) = NULL,
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
            Delete from Bookmarks where Id in (Select Id from bookmarks where folderid = @Id)
			Delete from Bookmarks where Id = @Id
          End
   
   
  If @OptID = 5
          Begin
            Update Bookmarks set FolderId=@FolderId where Id = @Id
          End
   

　
　
　
　
