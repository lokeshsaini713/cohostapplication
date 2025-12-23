-- =============================================
-- Author:		Satish Chandra Sudhansu
-- Create date: 29-Jan-2024
-- Description:	This Procedure is used for Logout Account user account
-- =============================================
CREATE PROCEDURE [dbo].[LogoutUser]
@Id INT
AS
/*
		EXEC LogoutAccount
		SELECT * FROM UserDetail
*/
BEGIN
	UPDATE UserDetail
	SET DeviceToken = NULL ,
		AccessToken = NULL
	WHERE UserId=@Id
END