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
