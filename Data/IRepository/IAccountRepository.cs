using Shared.Model.DTO;
using Shared.Model.Entities;
using Shared.Model.Request.Account;
using Shared.Model.Request.Admin;

namespace Data.IRepository
{
    public interface IAccountRepository : ICurdRepository<UserDetail>
    {
        Task<int> UpdateDeviceToken(UpdateDeviceTokenRequest request);
        Task<UserDetailsDto> FindByEmailAsync(string email);
        Task<List<UsersDto>> UserList(UsersRequestModel request);
        Task<int> ChangePassword(ChangePasswordModel model, long userId);

        #region Web User

        Task<ForgotPasswordDto> ResetPasswordTokenAsync(long userId, string forgotPasswordToken);
        Task<bool> CheckResetPasswordTokenExist(string token);
        Task<UserDetailsDto> GetUserDetailByToken(string token);
        Task<int> ResetPassword(ResetPasswordModel model);
        #endregion
        CheckUserAccessTokenDto CheckUserAccessToken(string accessToken);
        bool CheckAppVersion(string appVersion, short deviceTypeId);
        Task<int> LogoutUser(int id);
    }
}
