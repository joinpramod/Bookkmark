USE [bookmarks]
GO
/****** Object:  StoredProcedure [dbo].[Domain_Sp]    Script Date: 12/22/2017 9:56:42 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

　
　
　
ALTER PROCEDURE [dbo].[Domain_Sp]
@OptID as int = 1,
@Id As Numeric(9,0) = NULL,
@Name As VarChar(50) = NULL,
@Script As VarChar(150) = NULL,
@Height As VarChar(150) = NULL,
@Width As VarChar(150) = NULL,
@ShowCount As VarChar(150) = NULL,
@CreatedUser As Numeric(9,0) = NULL,
@CreatedDateTime As DateTime  = NULL,
@ModifiedDateTime As DateTime  = NULL
   
As
   
   
      If @OptID = 1
          Begin
            Insert into Domain(Name,Script,Height,Width,ShowCount,CreatedUser,CreatedDateTime) 
		Values(@Name,@Script,@Height,@Width,@ShowCount,@CreatedUser,@CreatedDateTime)
            Select @@identity as KeyID
          End
   
   
   
   
      If @OptID = 2
          Begin
             Update Domain Set Name=@Name,Script=@Script,Height=@Height,Width=@Width,ShowCount=@ShowCount,ModifiedDateTime=@ModifiedDateTime where Id = @Id
			Select @Id as KeyID
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

　
　
	 If @OptID = 5
          Begin
             Update Domain Set Script=@Script where Id = @Id
          End
