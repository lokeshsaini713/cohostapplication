using Business.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Resources;
using System.IdentityModel.Tokens.Jwt;

namespace Api.Filters
{
    public class AuthorizeHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var svc = actionContext.HttpContext.RequestServices;
            var userAccountService = svc.GetService<IAccountService>();
            ApiResponse<JsonResult> jsonResponse = new ApiResponse<JsonResult>();

            string authorizationToken = Convert.ToString(actionContext.HttpContext.Request.Headers["Authorization"]);
            string? utcOffsetInSecond = actionContext.HttpContext.Request.Headers["UtcOffsetInSecond"];
            string? accessToken = actionContext.HttpContext.Request.Headers["AccessToken"];
            string? appVersion = actionContext.HttpContext.Request.Headers["AppVersion"];
            string? deviceTypeId = actionContext.HttpContext.Request.Headers["DeviceTypeId"];

            #region Header check
            bool hasAllowAnonymous = actionContext.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is

            if (!hasAllowAnonymous && authorizationToken == null)
            {
                jsonResponse.Message = ResourceString.JWTtokenRequired;
                actionContext.Result = new UnauthorizedObjectResult(jsonResponse);
                return;
            }

            if (string.IsNullOrEmpty(utcOffsetInSecond))
            {
                jsonResponse.Message = ResourceString.UtcOffsetInSecond;
                actionContext.Result = new BadRequestObjectResult(jsonResponse);
                return;
            }
            else
            {
                SiteKeys.UtcOffsetInSecond_API = Convert.ToInt32(utcOffsetInSecond);
            }

            if (appVersion == null)
            {
                jsonResponse.Message = ResourceString.AppVersion;
                actionContext.Result = new BadRequestObjectResult(jsonResponse);
                return;
            }
            else
            {
                if (deviceTypeId is not null)
                {
                    // get device type id from enum

                    var isForceUpdateAvailable = userAccountService?.CheckAppVersion(appVersion, Convert.ToInt16(deviceTypeId));
                    if (isForceUpdateAvailable ?? false)
                    {
                        jsonResponse.Message = ResourceString.OldAppVersion;
                        actionContext.Result = new JsonResult(jsonResponse)
                        {
                            StatusCode = 426
                        };
                        return;
                    }
                }

            }
            #endregion
            #region Access Token check here
            if (!hasAllowAnonymous)
            {
                if (accessToken == null)
                {
                    jsonResponse.Message = ResourceString.AccessTokenRequired;
                    actionContext.Result = new BadRequestObjectResult(jsonResponse);
                    return;
                }
                else
                {

                    SiteKeys.AccessToken = accessToken;
                    var userId = getUserIdByJwtToken(authorizationToken.Split(" ")[1]);
                    var objUserTokenModel = userAccountService?.GetByIdAsync(userId).Result;
                    if (objUserTokenModel != null)
                    {
                        if (!objUserTokenModel.IsActive ?? false)
                        {
                            jsonResponse.Message = ResourceString.UserIsNotActive;
                            actionContext.Result = new JsonResult(jsonResponse)
                            {
                                StatusCode = 403
                            };
                            return;                            
                        }
                        else if (objUserTokenModel.IsDeleted ?? false)
                        {
                            jsonResponse.Message = ResourceString.UserAccountDeleted;
                            actionContext.Result = new JsonResult(jsonResponse)
                            {
                                StatusCode = 403
                            };
                            return;
                        }
                        else if (accessToken != objUserTokenModel.AccessToken)
                        {
                            jsonResponse.Message = ResourceString.AnotherDeviceLogin;
                            actionContext.Result = new JsonResult(jsonResponse)
                            {
                                StatusCode = 403
                            };
                            return;
                        }

                    }
                    else
                    {
                        jsonResponse.Message = ResourceString.NotAuthorized;
                        actionContext.Result = new JsonResult(jsonResponse)
                        {
                            StatusCode = 403
                        };
                        return;
                    }
                }
            }

            #endregion

            base.OnActionExecuting(actionContext);
        }

        private int getUserIdByJwtToken(string jwtToken)
        {
            var stream = jwtToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var userIdentity = tokenS?.Claims.First(claim => claim.Type == "userId").Value;
            if (!string.IsNullOrEmpty(userIdentity))
            {
                return Convert.ToInt32(userIdentity);
            }
            else
            {
                return 0;
            }
        }
    }
}
