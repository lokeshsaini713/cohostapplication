using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Request.Profile;

namespace Business.IServices
{
    public interface IProfileService
    {
        Task<ApiResponse<GetUserDetailsDto>> GetUserDetails(int userId);
        Task<ApiResponse<bool>> UpdateProfile(UpdateProfileRequest profileRequest, int userId);

        Task<ApiResponse<bool>> DeleteProfile(int userId);
    }
}
