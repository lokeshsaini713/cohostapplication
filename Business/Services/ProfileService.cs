using Business.IServices;
using Data.IRepository;
using Shared.Common;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Entities;
using Shared.Model.Request.Profile;
using Shared.Resources;

namespace Business.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IAccountRepository _accountRepository;
        public ProfileService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ApiResponse<GetUserDetailsDto>> GetUserDetails(int userId)
        {
            var getUserDetail = await _accountRepository.GetByIdAsync(userId);

            if (getUserDetail is null)
            {
                return new ApiResponse<GetUserDetailsDto>(null, message: ResourceString.UserDetailsNotFound, apiName: "GetUserDetails");
            }

            GetUserDetailsDto userDetailsDto = new GetUserDetailsDto
            {
                FirstName = getUserDetail.FirstName,
                LastName = getUserDetail.LastName,
                Email = getUserDetail.Email,
                ProfileImage = getUserDetail.ProfileImage,
                PhoneNumber = getUserDetail.PhoneNumber,
                UserType = Convert.ToInt16(getUserDetail.UserType),
            };

            userDetailsDto.ProfileImage = CommonFunctions.GetRelativeFilePath(userDetailsDto.ProfileImage, Constants.UserImageFolderPath, Constants.DefaultUserPng);
            return new ApiResponse<GetUserDetailsDto>(userDetailsDto, message: ResourceString.GetUserDetails, apiName: "GetUserDetails");


        }

        public async Task<ApiResponse<bool>> UpdateProfile(UpdateProfileRequest profileRequest, int userId)
        {
            int updateUser = await _accountRepository.AddUpdateAsync(
                new UserDetail()
                {
                    Id = userId,
                    FirstName = profileRequest.FirstName,
                    LastName = profileRequest.LastName,
                    PhoneNumber = profileRequest.PhoneNumber,
                });

            if (updateUser <= 0)
            {
                return new ApiResponse<bool>(false, message: ResourceString.UpdateNotProfile, apiName: "UpdateProfile");
            }

            return new ApiResponse<bool>(true, message: ResourceString.UpdateProfile, apiName: "UpdateProfile");
        }


        public async Task<ApiResponse<bool>> DeleteProfile(int userId)
        {
            var getUserDetail = await _accountRepository.GetByIdAsync(userId);
            if (getUserDetail is null)
            {
                return new ApiResponse<bool>(false, message: ResourceString.UserDetailsNotFound, apiName: "DeleteProfile");
            }
            int updateUser = await _accountRepository.AddUpdateAsync(
                new UserDetail()
                {
                    Id = userId,
                    FirstName = "deleted",
                    LastName = "deleted",
                    PhoneNumber = "deleted",
                    Email = "deleted",
                    IsDeleted = true,
                    AccessToken = "deleted"
                });

            if (updateUser <= 0)
            {
                return new ApiResponse<bool>(false, message: ResourceString.ProfileNotDeleted, apiName: "DeleteProfile");
            }

            return new ApiResponse<bool>(true, message: ResourceString.ProfileDeleted, apiName: "DeleteProfile");
        }
    }
}
