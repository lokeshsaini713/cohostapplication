// Ignore Spelling: Api jwt

using Api.Controllers.Base;
using Api.Helper;
using Api.JWT;
using Asp.Versioning;
using Business.IServices;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.JWT;
using Shared.Model.Request.Account;
using Shared.Resources;
using System.Net;
using static Google.Apis.Requests.BatchRequest;

namespace Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AccountController : ApiBaseController
    {
        private readonly IAccountService _accountService;
        private readonly IManageService _manageService;
        private readonly JwtTokenSettings _jwtTokenSettings;

        public AccountController(IAccountService accountService, IManageService manageService, IOptions<JwtTokenSettings> jwtOptions)
        {
            _accountService = accountService;
            _manageService = manageService;
            _jwtTokenSettings = jwtOptions.Value;
        }

        [Route("Login")]
        [HttpPost, AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ProfileDto>))]
        public async Task<IActionResult> Login([FromBody] ApiLoginRequest request)
        {
            request.UserType = UserTypes.User;
            var loginUserDetail = await _accountService.Login(request);
            if (loginUserDetail.Data == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, loginUserDetail);
            }
            else
            {
                JwtTokenBuilder tokenBuilder = new();
                var token = tokenBuilder.GetToken(_jwtTokenSettings, loginUserDetail.Data.UserId);
                loginUserDetail.Data.AuthorizationToken = token.Value;
                loginUserDetail.Data.UserId = 0;
                return StatusCode(StatusCodes.Status200OK, loginUserDetail);
            }
        }

        [Route("SignUp")]
        [HttpPost, AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> SignUp([FromBody] RegistrationRequest request)
        {
            request.UserType = UserTypes.User;
            request.DeviceType = DeviceTypeEnum.Web;
            var userDetail = await _accountService.SignUp(request);

            if (userDetail.Data)
            {
                return StatusCode(StatusCodes.Status200OK, userDetail);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, userDetail);
            }

        }
        
        [Route("Email/Verify/Resend")]
        [HttpPost, AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> ResendEmailVerificationLink([FromBody] ResendVerificationLinkRequest request)
        {
            var verifyEmail = await _accountService.ResendVerificationLink(request);
            return StatusCode(StatusCodes.Status200OK, verifyEmail);
        }

        [Route("ForgetPassword")]
        [HttpPost, AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
        {
            var updateUser = await _accountService.ForgetPassword(request);
            if (updateUser.Data)
            {
                return StatusCode(StatusCodes.Status200OK, updateUser);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, updateUser);
            }
        }

        [Route("ChangePassword")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            var changePasswordResponse = await _manageService.ChangePassword(model, UserId);
            if (changePasswordResponse == ResponseTypes.Success)
            {
                return Ok(new ApiResponse<bool> { Data = true,Message = ResourceString.PasswordUpdated, ApiName = nameof(ChangePassword) });
            }
            else if (changePasswordResponse == ResponseTypes.OldPasswordWrong)
            {
                return BadRequest(new ApiResponse<bool> { Data = false, Message = ResourceString.WrongOldPassword, ApiName = nameof(ChangePassword) });
            }
            else if (changePasswordResponse == ResponseTypes.OldNewPasswordMatched)
            {
                return BadRequest(new ApiResponse<bool> { Data = false, Message = ResourceString.OldNewPasswordNotSame, ApiName = nameof(ChangePassword) });
            }
            else
            {
                return BadRequest(new ApiResponse<bool> { Data = false, Message = ResourceString.FailedToSetNewPassword, ApiName = nameof(ChangePassword) });
            }
        }

        [Route("Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userLogoutSuccess = await _manageService.LogoutUser(UserId);
            return StatusCode((int)HttpStatusCode.OK, userLogoutSuccess);

        }
    }
}
