using Business.IServices;
using Data.IRepository;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Entities;
using Shared.Model.Request.Account;
using Shared.Model.Request.Admin;
using Shared.Resources;
using Shared.Utility;
using System;

namespace Business.Services
{
    public class ManageService : IManageService
    {
        private readonly IAccountRepository _accountRepository;
        public ManageService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<ApiResponse<List<UsersDto>>> UserList(UsersRequestModel request)
        {
            ApiResponse<List<UsersDto>> response = new();
            var getUserList = await _accountRepository.UserList(request);

            if (getUserList is null)
            {
                response.Data = new List<UsersDto>();
                response.Message = ResourceString.Error;
                return response;
            }

            getUserList.ForEach(u =>
            {
                u.PhoneNumber = u.PhoneNumber ?? "N/A";
                u.RegisterDate = u.CreatedOn.ToShortDateString();
            });

            response.Data = getUserList;
            response.Message = ResourceString.Success;

            return response;
        }

        public async Task<ApiResponse<UserDetailsDto>> GetUserDetails(int userId)
        {
            ApiResponse<UserDetailsDto> response = new();
            UserDetailsDto userDetailsObj = new();
            var getUserDetail = await _accountRepository.GetByIdAsync(userId);

            if (getUserDetail is null)
            {
                response.Data = userDetailsObj;
                response.Message = ResourceString.Fail;
                return response;
            }

            userDetailsObj.UserId = (int)getUserDetail.Id;
            userDetailsObj.UserType = getUserDetail.UserType;
            userDetailsObj.FirstName = getUserDetail.FirstName ?? "N/A";
            userDetailsObj.LastName = getUserDetail.LastName ?? "N/A";
            userDetailsObj.PhoneNumber = getUserDetail.PhoneNumber;
            userDetailsObj.Email = getUserDetail.Email ?? "N/A";
            userDetailsObj.IsActive = getUserDetail.IsActive;
            userDetailsObj.IsDeleted = getUserDetail.IsDeleted;
            userDetailsObj.ProfileImage = getUserDetail.ProfileImage;
            userDetailsObj.ProfileImageUrl = CommonFunctions.GetRelativeFilePath(getUserDetail.ProfileImage, Constants.UserImageFolderPath, Constants.DefaultUserPng);

            response.Data = userDetailsObj;
            response.Message = ResourceString.Success;
            return response;
        }

        public async Task<ApiResponse<bool>> UpdateUserDetail(UserDetailsDto requestModel)
        {
            ApiResponse<bool> response = new();
            bool activeDeactiveObj = false;

            string newImageName = string.Empty;
            if (requestModel.Image != null)
            {
                newImageName = Guid.NewGuid() + Path.GetExtension(requestModel.Image.FileName);

                var folderPath = string.Format("{0}/{1}/{2}", SiteKeys.SitePhysicalPath, "wwwroot", Constants.UserImageFolderPath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var filePath = Path.Combine(folderPath, newImageName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    requestModel.Image.CopyTo(stream);
                }
            }

            if (!string.IsNullOrEmpty(newImageName))
            {
                requestModel.ProfileImage = newImageName;
            }
            else
            {
                requestModel.ProfileImage = null;
            }

            var updateUser = await _accountRepository.AddUpdateAsync(new UserDetail
            {
                Id = requestModel.UserId,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                PhoneNumber = requestModel.PhoneNumber,
                ProfileImage = requestModel.ProfileImage,
            });

            if (updateUser > 0)
            {
                response.Message = ResourceString.ProfileUpdateSuccess;
                response.Data = true;
            }
            else
            {
                response.Message = ResourceString.ProfileUpdateFailed;
                response.Data = activeDeactiveObj;
            }
            return response;
        }

        public async Task<ResponseTypes> ChangePassword(ChangePasswordModel model, int userId)
        {
            var getUserDetail = await _accountRepository.GetByIdAsync(userId);

            if (getUserDetail is null)
            {
                return ResponseTypes.Error;
            }

            if (!Encryption.VerifyHash(model.OldPassword, getUserDetail.PasswordHash))
            {
                return ResponseTypes.OldPasswordWrong;
            }
            else
            {
                if (Encryption.VerifyHash(model.Password, getUserDetail.PasswordHash))
                {
                    return ResponseTypes.OldNewPasswordMatched;
                }
                else
                {
                    model.Password = Encryption.ComputeHash(model.Password);
                    var updatePassword = await _accountRepository.ChangePassword(model, userId);
                    if (updatePassword > 0)
                    {
                        return ResponseTypes.Success;
                    }
                    else
                    {
                        return ResponseTypes.Error;
                    }
                }

            }
        }
        public async Task<int> ChangeUserStatus(int userId, bool activeStatus, bool deleteStatus)
        {
            UserDetail requestModel = new();
            requestModel.Id = userId;
            requestModel.IsActive = activeStatus;
            requestModel.IsDeleted = deleteStatus;
            requestModel.AccessToken = Guid.NewGuid().ToString();
            
            return await _accountRepository.AddUpdateAsync(requestModel);
        }

        public async Task<ApiResponse<bool>> LogoutUser(int id)
        {
            var res = await _accountRepository.LogoutUser(id);
            if (res > 0)
            {
                return new ApiResponse<bool>(true, message: ResourceString.LogoutSuccess, apiName: "LogoutUser");
            }
            else
            {
                return new ApiResponse<bool>(false, message: ResourceString.SomethingWrong, apiName: "LogoutUser");
            }
        }
    }
}
