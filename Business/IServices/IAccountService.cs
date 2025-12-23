using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Entities;
using Shared.Model.Request.Account;
using Shared.Model.Request.WebUser;

namespace Business.IServices
{
    public interface IAccountService
    {
        Task<ApiResponse<ProfileDto>> Login(ApiLoginRequest request);
        Task<UserDetailsDto> FindByEmailAsync(string email);
        Task<ApiResponse<bool>> SignUp(RegistrationRequest request);
        Task<ApiResponse<bool>> VerifyEmail(EmailVerifyRequest request);
        Task<ApiResponse<bool>> ForgetPassword(ForgetPasswordRequest request);
        Task<ApiResponse<bool>> ResendVerificationLink(ResendVerificationLinkRequest request);

        Task<ApiResponse<bool>> SaveContactUsDetails(ContactUsRequestModel requestModel);

        #region Web User
        Task<ApiResponse<ForgotPasswordDto>> ResetPasswordTokenAsync(long userId);
        Task<bool> CheckResetPasswordTokenExist(string token);
        Task<ResponseTypes> ResetPassword(ResetPasswordModel model);
        Task<ApiResponse<bool>> UpdateEmailVerificationToken(int userId, string email, string name);
        #endregion
        CheckUserAccessTokenDto CheckUserAccessToken(string accessToken);
        bool CheckAppVersion(string appVersion, short deviceTypeId);
        Task<UserDetail> GetByIdAsync(int userId);
    }
}
