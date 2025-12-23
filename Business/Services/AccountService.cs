using Business.Communication;
using Business.IServices;
using Data.IRepository;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Entities;
using Shared.Model.Request.Account;
using Shared.Model.Request.WebUser;
using Shared.Resources;
using Shared.Utility;

namespace Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepositry;
        private readonly INotificationService _notificationService;
        public AccountService(IAccountRepository accountRepositry, INotificationService notificationService)
        {
            _accountRepositry = accountRepositry;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<ProfileDto>> Login(ApiLoginRequest request)
        {
            var objUserContext = await _accountRepositry.FindByEmailAsync(request.Email);

            if (objUserContext == null)
            {
                return new ApiResponse<ProfileDto>(null, message: ResourceString.UserNotExist, apiName: "Login");
            }
            else if (request.UserType != null && objUserContext.UserType != (short)request.UserType)
            {
                return new ApiResponse<ProfileDto>(null, message: ResourceString.UserNotExist, apiName: "Login");
            }
            else if (!objUserContext.IsEmailVerified)
            {
                return new ApiResponse<ProfileDto>(null, message: ResourceString.EmailNotVerified, apiName: "Login");
            }
            else if (!(objUserContext.IsActive ?? false))
            {
                return new ApiResponse<ProfileDto>(null, message: ResourceString.UserIsNotActive, apiName: "Login");
            }
            else if (!Encryption.VerifyHash(request.Password, objUserContext.PasswordHash))
            {
                return new ApiResponse<ProfileDto>(null, message: ResourceString.InvalidPassword, apiName: "Login");
            }

            var accessToken = Guid.NewGuid().ToString();

            await _accountRepositry.UpdateDeviceToken(new UpdateDeviceTokenRequest()
            {
                Email = request.Email,
                DeviceToken = request.DeviceToken,
                DeviceType = request.DeviceType,
                AccessToken = accessToken,
            });
            string folderPath = Constants.UserImageFolderPath;

            ProfileDto profileDto = new ProfileDto
            {
                UserId = objUserContext.UserId,
                FirstName = objUserContext.FirstName,
                LastName = objUserContext.LastName,
                Email = objUserContext.Email,
                ProfileImage = objUserContext.ProfileImage,
                AccessToken = accessToken,
                UserType = objUserContext.UserType
            };

            profileDto.ProfileImage = CommonFunctions.GetRelativeFilePath(profileDto.ProfileImage, folderPath, Constants.DefaultUserPng);

            return new ApiResponse<ProfileDto>(profileDto, message: ResourceString.Success, apiName: "Login");
        }

        public Task<UserDetailsDto> FindByEmailAsync(string email) => _accountRepositry.FindByEmailAsync(email);

        public async Task<ApiResponse<bool>> SignUp(RegistrationRequest request)
        {
            var getUserDetail = await _accountRepositry.FindByEmailAsync(request.Email);

            if (getUserDetail is not null)
            {
                return new ApiResponse<bool>(false, message: ResourceString.EmailExists, apiName: "Signup");
            }

            var token = Guid.NewGuid().ToString();
            var newUser = await _accountRepositry.AddUpdateAsync(new UserDetail()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = Encryption.ComputeHash(request.Password),
                UserType = (short)request.UserType,
                EmailVerifiedToken = token,
                IsActive = true,
                IsDeleted = false,
            });

            if (newUser <= 0)
            {
                return new ApiResponse<bool>(false, message: ResourceString.Error, apiName: "Signup");
            }

            await _notificationService.EmailVerification(request.Email, request.FirstName + " " + request.LastName, token, ResourceString.RegistrationSubject, Convert.ToInt32(request.DeviceType));

            return new ApiResponse<bool>(true, message: ResourceString.SignUp, apiName: "Signup");
        }

        public async Task<ApiResponse<bool>> VerifyEmail(EmailVerifyRequest request)
        {
            var verifyEmail = await _accountRepositry.AddUpdateAsync(new UserDetail()
            {
                EmailVerifiedToken = request.Token,
            });

            if (verifyEmail <= 0)
            {
                return new ApiResponse<bool>(false, message: ResourceString.InvalidConfirmationToken, apiName: "VerifyEmail");
            }

            return new ApiResponse<bool>(true, message: ResourceString.EmailVerified, apiName: "VerifyEmail");
        }

        public async Task<ApiResponse<bool>> ForgetPassword(ForgetPasswordRequest request)
        {
            var getUserByEmail = await _accountRepositry.FindByEmailAsync(request.Email);
            if (getUserByEmail == null)
            {
                return new ApiResponse<bool>(false, message: ResourceString.UserNotExist, apiName: "ForgetPassword");
            }

            ForgetPasswordTokenRequest tokenRequest = new();
            tokenRequest.Email = request.Email;
            tokenRequest.Token = Guid.NewGuid().ToString();

            var updateUser = await _accountRepositry.AddUpdateAsync(new UserDetail()
            {
                Email = tokenRequest.Email,
                ResetPasswordToken = tokenRequest.Token,
            });

            if (updateUser <= 0)
            {
                return new ApiResponse<bool>(false, message: ResourceString.Error, apiName: "ForgetPassword");
            }

            await _notificationService.SendResetPasswordEmail(ResourceString.ForgetPasswordSubject, tokenRequest.Token, tokenRequest.Email, getUserByEmail.UserName);
            return new ApiResponse<bool>(true, message: ResourceString.ForgetPassword, apiName: "ForgetPassword");
        }




        public async Task<ApiResponse<bool>> ResendVerificationLink(ResendVerificationLinkRequest request)
        {
            var getUserByEmail = await _accountRepositry.FindByEmailAsync(request.Email);
            if (getUserByEmail == null)
            {
                return new ApiResponse<bool>(false, message: ResourceString.UserNotExist, apiName: "ResendVerificationLink");
            }
            int updateUser = 0;
            ResendVerificationLinkTokenRequest tokenRequest = new();

            if (string.IsNullOrEmpty(getUserByEmail.EmailVerificationToken))
            {
                tokenRequest.Email = request.Email;
                tokenRequest.Token = Guid.NewGuid().ToString();

                updateUser = await _accountRepositry.AddUpdateAsync(new UserDetail()
                {
                    Email = tokenRequest.Email,
                    EmailVerifiedToken = tokenRequest.Token,
                    IsEmailVerified = false,
                    Id = getUserByEmail.UserId
                });

            }
            else
            {
                tokenRequest.Email = getUserByEmail.Email ?? "";
                tokenRequest.Token = getUserByEmail.EmailVerificationToken;
            }

            if (updateUser <= 0 && string.IsNullOrEmpty(getUserByEmail.EmailVerificationToken))
            {
                return new ApiResponse<bool>(false, message: ResourceString.Error, apiName: "ResendVerificationLink");
            }

            await _notificationService.EmailVerification(tokenRequest.Email, getUserByEmail.UserName ?? "", tokenRequest.Token, ResourceString.ResendVerificationLinkSubject, 0);
            return new ApiResponse<bool>(true, message: ResourceString.ResendVerificationLink, apiName: "ResendVerificationLink");
        }

        public async Task<ApiResponse<ForgotPasswordDto>> ResetPasswordTokenAsync(long userId)
        {
            ApiResponse<ForgotPasswordDto> response = new();
            var userDetail = await _accountRepositry.ResetPasswordTokenAsync(userId, Guid.NewGuid().ToString());

            if (userDetail is null)
            {
                response.Message = ResourceString.UserNotExist;
                return response;
            }

            switch ((ForgotPasswordResponseTypes)userDetail.IsValid)
            {
                case ForgotPasswordResponseTypes.TokenUpdatedSuccess:
                    response.Message = ResourceString.ForgetPassword;
                    response.Data = userDetail;
                    await _notificationService.SendResetPasswordEmailToWebUser("Forgot Password", userDetail.ForgotPasswordToken, userDetail.Email, userDetail.UserName);
                    break;

                case ForgotPasswordResponseTypes.ForgotEmailAlreadySent:
                    response.Message = ResourceString.ForgotEmailAlreadySent;
                    break;

                default:
                    if (userDetail.IsDeleted)
                    {
                        response.Message = ResourceString.UserAccountDeleted;
                    }
                    else if (!userDetail.IsActive)
                    {
                        response.Message = ResourceString.UserIsNotActive;
                    }
                    else
                    {
                        response.Message = ResourceString.EmailNotVerified;
                    }
                    break;
            }

            return response;
        }

        public async Task<bool> CheckResetPasswordTokenExist(string token) => await _accountRepositry.CheckResetPasswordTokenExist(token);

        public async Task<ResponseTypes> ResetPassword(ResetPasswordModel model)
        {
            var getUserDetail = await _accountRepositry.GetUserDetailByToken(model.Token);

            if (getUserDetail is null)
            {
                return ResponseTypes.Error;
            }

            if (Encryption.VerifyHash(model.Password, getUserDetail.PasswordHash))
            {
                return ResponseTypes.OldNewPasswordMatched;
            }

            var resetPasswordObj = await _accountRepositry.ResetPassword(model);
            return resetPasswordObj > 0 ? ResponseTypes.Success : ResponseTypes.Error;
        }



        public async Task<ApiResponse<bool>> SaveContactUsDetails(ContactUsRequestModel requestModel)
        {
            // save details
            await _notificationService.SendContactUsMailToAdmin(ResourceString.ContactUsSubject, requestModel.Name, requestModel.Email, requestModel.Query);
            return new ApiResponse<bool>(true, message: ResourceString.ContactUsMailSuccess, apiName: "ContactUs");
        }

        public async Task<ApiResponse<bool>> UpdateEmailVerificationToken(int userId, string email, string name)
        {
            var emailVerificationToken = Guid.NewGuid().ToString();
            var verifyEmail = await _accountRepositry.AddUpdateAsync(new UserDetail()
            {
                Id = userId,
                EmailVerifiedToken = emailVerificationToken,
            });
            if (verifyEmail <= 0)
            {
                return new ApiResponse<bool>(false, message: ResourceString.VerificationLinkSentFailed, apiName: "");
            }

            await _notificationService.EmailVerification(email, name, emailVerificationToken, ResourceString.VerifyEmailSubject, 0);
            return new ApiResponse<bool>(true, message: ResourceString.VerificationLinkSent, apiName: "");
        }


        public CheckUserAccessTokenDto CheckUserAccessToken(string accessToken) => _accountRepositry.CheckUserAccessToken(accessToken);

        public bool CheckAppVersion(string appVersion, short deviceTypeId) => _accountRepositry.CheckAppVersion(appVersion, deviceTypeId);

        public async Task<UserDetail> GetByIdAsync(int userId)
        {
            return await _accountRepositry.GetByIdAsync(userId);
        }
    }
}
