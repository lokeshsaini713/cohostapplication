using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Request.Account;
using Shared.Model.Request.Admin;

namespace Business.IServices
{
    public interface IManageService
    {
        Task<ApiResponse<List<UsersDto>>> UserList(UsersRequestModel request);
        Task<ApiResponse<UserDetailsDto>> GetUserDetails(int userId);
        Task<ApiResponse<bool>> UpdateUserDetail(UserDetailsDto requestModel);
        Task<ResponseTypes> ChangePassword(ChangePasswordModel model, int userId);
        Task<int> ChangeUserStatus(int userId, bool activeStatus, bool deleteStatus);
        Task<ApiResponse<bool>> LogoutUser(int id);
    }
}
