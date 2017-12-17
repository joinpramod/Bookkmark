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

