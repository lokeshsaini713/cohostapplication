// Ignore Spelling: Api jwt

using Api.Controllers.Base;
using Api.JWT;
using Asp.Versioning;
using Business.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.JWT;
using Shared.Model.Request.Profile;
using Shared.Resources;
using System.Net;

namespace Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class ProfileController : ApiBaseController
    {
        private readonly IProfileService _profileService;
        private readonly JwtTokenSettings _jwtTokenSettings;
        public ProfileController(IProfileService profileService, IOptions<JwtTokenSettings> jwtOptions)
        {
            _profileService = profileService;
            _jwtTokenSettings = jwtOptions.Value;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<GetUserDetailsDto>))]
        public async Task<IActionResult> GetUserDetails()
        {
            var getUserDetail = await _profileService.GetUserDetails(UserId);

            if (getUserDetail.Data == null)
            {
                return StatusCode((int)HttpStatusCode.NoContent, getUserDetail);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.OK, getUserDetail);
            }
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest profileRequest)
        {
            var updateUserProfile = await _profileService.UpdateProfile(profileRequest, UserId);
            if (updateUserProfile.Data)
            {
                return StatusCode((int)HttpStatusCode.OK, updateUserProfile);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NoContent, updateUserProfile);
            }
        }


        [Route("")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> DeleteAccount()
        {
            var updateUserProfile = await _profileService.DeleteProfile(UserId);
            if (updateUserProfile.Data)
            {
                return StatusCode(StatusCodes.Status200OK, updateUserProfile);
            }
            return StatusCode(StatusCodes.Status404NotFound, updateUserProfile);
        }

        [Route("RefreshToken")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        public IActionResult RefreshToken()
        {
            ApiResponse<string> apiResponse = new ApiResponse<string>();
            JwtTokenBuilder tokenBuilder = new();
            apiResponse.Data = tokenBuilder.GetToken(_jwtTokenSettings, UserId).Value;
            apiResponse.Message = ResourceString.TokenFetched;
            apiResponse.ApiName = "RefreshToken";
            return StatusCode(StatusCodes.Status200OK, apiResponse);
        }
    }
}
