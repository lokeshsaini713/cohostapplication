// Ignore Spelling: Accessor

using Business.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.Request.Account;
using Shared.Resources;
using System.Resources;

namespace Web.Controllers
{
    [AllowAnonymous, ValidateModel]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountController(IHttpContextAccessor httpContextAccessor, IAccountService accountService)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;


        }
        [HttpGet]
        public IActionResult Login()
        {
            if (LoginMemberSession.UserDetailSession != null && LoginMemberSession.UserDetailSession.UserTypeId == Convert.ToInt16(UserTypes.Admin))
            {
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else if (LoginMemberSession.UserDetailSession != null && LoginMemberSession.UserDetailSession.UserTypeId == Convert.ToInt16(UserTypes.User))
            {
                return RedirectToAction("Profile", "User");
            }

            ViewBag.ShowEmailVerificationPopUp = false;
            var returnUrl = TempData["ReturnUrl"];
            LoginRequest model = new()
            {
                ReturnUrl = returnUrl == null ? string.Empty : (string)returnUrl,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginViewModel)
        {
            ViewBag.ShowEmailVerificationPopUp = false;
            var retrurnUrl = loginViewModel.ReturnUrl;

            var context = _httpContextAccessor.HttpContext;
            SiteKeys.UtcOffset = context?.Request.Cookies["timezoneoffset"];

            if (Request.Cookies["timezoneoffset"] != null)
            {
                _httpContextAccessor.HttpContext?.Session.SetInt32("UtcOffsetInSecond", Convert.ToInt32(Request.Cookies["timezoneoffset"]) * 60);
            }

            var loginResponse = await _accountService.Login(new ApiLoginRequest() { Email = loginViewModel.Email, Password = loginViewModel.Password });
            
            if (loginResponse.Data is null)
            {
                if (loginResponse.Message?.Equals(ResourceString.EmailNotVerified)?? false)
                {
                    ViewBag.ShowEmailVerificationPopUp = true;
                }
                ViewBag.message = loginResponse.Message;
                return View(loginViewModel);
            }

            //Store value in session
            LoginSessionModel sessionobj = new();
            sessionobj.UserId = loginResponse.Data.UserId;
            sessionobj.FirstName = loginResponse.Data.FirstName;
            sessionobj.LastName = loginResponse.Data.LastName;
            sessionobj.UserTypeId = loginResponse.Data.UserType;
            sessionobj.EmailId = loginResponse.Data.Email;
            LoginMemberSession.UserDetailSession = sessionobj;

            if (loginResponse.Data.UserType == Convert.ToInt16(UserTypes.Admin))
            {
                if (retrurnUrl != null)
                {
                    return Redirect(retrurnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "User", new { area = "Admin" });
                }
            }
            if (loginResponse.Data.UserType == Convert.ToInt16(UserTypes.User))
            {
                if (retrurnUrl != null)
                {
                    return Redirect(retrurnUrl);
                }
                else
                {
                    return RedirectToAction("Profile", "User", new { area = "" });
                }
            }
            else
            {
                ViewBag.message = ResourceString.NotAuthorized;
            }


            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [Route("SignUp")]
        public IActionResult Registration()
        {
            RegistrationRequest registrationRequest = new();
            return View(registrationRequest);
        }

        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationRequest request)
        {
            request.UserType = UserTypes.User;
            request.DeviceType = DeviceTypeEnum.Web;
            if (!request.TermsAndCondtion)
            {
                return BadRequest(new ApiResponse<int> { Message = ResourceString.AcceptTermAndConditons });
            }

            var registrationResponse = await _accountService.SignUp(request);
            if (registrationResponse.Data)
            {
                return Ok(new ApiResponse<int> { Message = registrationResponse.Message });
            }
            else
            {
                return BadRequest(new ApiResponse<int> { Message = registrationResponse.Message });
            }
        }
        public async Task<IActionResult> EmailVerification(string token, int type = 0)
        {
            EmailVerifyRequest request = new();
            request.Token = token;
            var isEmailVerified = await _accountService.VerifyEmail(request);
            ViewBag.type = type;
            if (isEmailVerified.Data)
            {
                ViewBag.EmailVerified = true;

            }
            else
            {
                ViewBag.EmailVerified = false;
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ForgetPassword(ForgotPasswordRequestModel model)
        {
            var getUserByEmail = await _accountService.FindByEmailAsync(model.Email);

            if (getUserByEmail != null)
            {
                if (getUserByEmail.UserType == Convert.ToInt64(UserTypes.Admin) || getUserByEmail.UserType == Convert.ToInt64(UserTypes.User))
                {
                    var updateResetToken = await _accountService.ResetPasswordTokenAsync(Convert.ToInt64(getUserByEmail.UserId));

                    if (updateResetToken.Data != null)
                    {
                        return Ok(new ApiResponse<string> { Message = updateResetToken.Message });
                    }
                    else
                    {
                        return BadRequest(new ApiResponse<string> { Message = updateResetToken.Message });
                    }
                }
                else
                {
                    return BadRequest(new ApiResponse<string> { Message = ResourceString.InvalidRequest });
                }
            }
            else
            {
                return NotFound(new ApiResponse<string> { Message = ResourceString.UserNotExist });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            ResetPasswordModel model = new();
            model.Token = token;
            if (!string.IsNullOrEmpty(token))
            {
                var isResetTokenExists = await _accountService.CheckResetPasswordTokenExist(token);
                model.ValidToken = isResetTokenExists;
            }
            else
            {
                model.ValidToken = false;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordModel model)
        {
            var passwordSetup = await _accountService.ResetPassword(model);

            if (passwordSetup == ResponseTypes.Success)
            {
                return Ok(new ApiResponse<bool> { Message = ResourceString.PasswordUpdated });
            }
            else if (passwordSetup == ResponseTypes.OldNewPasswordMatched)
            {
                return BadRequest(new ApiResponse<bool> { Message = ResourceString.OldNewPasswordNotSame });
            }
            else
                return NotFound(new ApiResponse<bool> { Message = ResourceString.InvalidOrResetTokenExpired });
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<ActionResult> VerifyEmail(VerifyEmailRequestModel model)
        {
            var getUserByEmail = await _accountService.FindByEmailAsync(model.Email);
            if (getUserByEmail != null)
            {
                if (getUserByEmail.UserType == Convert.ToInt64(UserTypes.Admin) || getUserByEmail.UserType == Convert.ToInt64(UserTypes.User))
                {
                    if (getUserByEmail.IsEmailVerified)
                    {
                        return BadRequest(new ApiResponse<string> { Message = ResourceString.EmailAlreadyVerified });
                    }
                    else if (getUserByEmail.IsDeleted ?? false)
                    {
                        return BadRequest(new ApiResponse<string> { Message = ResourceString.UserAccountDeleted });
                    }
                    else if (!(getUserByEmail.IsActive ?? false))
                    {
                        return BadRequest(new ApiResponse<string> { Message = ResourceString.DeactivateUser });
                    }
                    else
                    {
                        var updateEmailVerificationToken = await _accountService.UpdateEmailVerificationToken(getUserByEmail.UserId, getUserByEmail.Email ?? string.Empty, getUserByEmail.UserName ?? string.Empty);
                        if (updateEmailVerificationToken != null)
                        {
                            if (updateEmailVerificationToken.Data)
                            {
                                return Ok(new ApiResponse<string> { Message = updateEmailVerificationToken.Message });
                            }
                            else
                            {
                                return BadRequest(new ApiResponse<string> { Message = updateEmailVerificationToken.Message });
                            }
                        }
                        else
                        {
                            return BadRequest(new ApiResponse<string> { Message = ResourceString.VerificationLinkSentFailed });
                        }
                    }
                }
                else
                {
                    return BadRequest(new ApiResponse<string> { Message = ResourceString.InvalidRequest });
                }
            }
            else
            {
                return NotFound(new ApiResponse<string> { Message = ResourceString.UserNotExist });
            }
        }
    }
}
