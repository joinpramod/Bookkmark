USE [booqmarqs]
GO

/****** Object:  StoredProcedure [dbo].[Suggestion_Sp]    Script Date: 12/17/2017 10:07:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Suggestion_Sp]
@OptID as int = 1,
@SuggestionId As Numeric(9,0) = NULL,
@Suggestion As VarChar(1000) = NULL,
@CreatedUser As Numeric(9,0) = NULL,
@CreatedDate As DateTime  = NULL
   
As
   
   
      If @OptID = 1
          Begin
            Insert into Suggestion(Suggestion,CreatedUser,CreatedDate) Values(@Suggestion,@CreatedUser,@CreatedDate)
            Select @@identity as KeyID
          End
   
   
   
   
      If @OptID = 2
          Begin
             Update Suggestion Set Suggestion=@Suggestion,CreatedUser=@CreatedUser,CreatedDate=@CreatedDate where SuggestionId = @SuggestionId
          End
   
   
   
   
      If @OptID = 3
          Begin
            Delete from Suggestion where SuggestionId = @SuggestionId
          End
   
   
   
   
      If @OptID = 4
          Begin
            Select * From Suggestion where SuggestionId = @SuggestionId
              Return
          End
   
   




GO

