using Business.IServices;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Request.Account;
using Shared.Resources;
using Web.Controllers.Base;

namespace Web.Controllers
{
    //[ValidateModel]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : UserBaseController
    {
        private readonly IManageService _manageService;
        public UserController(IManageService manageService)
        {
            _manageService = manageService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Profile()
        {
            var customerDetail = await _manageService.GetUserDetails(UserId);
            return View(customerDetail.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserDetailsDto requestmodel)
        {
            var updateUser = await _manageService.UpdateUserDetail(requestmodel);
            if (updateUser.Data)
            {
                LoginSessionModel sessionobj = new();
                sessionobj.FirstName = requestmodel.FirstName;
                sessionobj.LastName = requestmodel.LastName;
                sessionobj.UserId = requestmodel.UserId;
                sessionobj.UserTypeId = Convert.ToInt16(UserTypes.User);
                sessionobj.EmailId = requestmodel.Email;
                LoginMemberSession.UserDetailSession = sessionobj;
            }
            return Json(updateUser);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            ChangePasswordModel model = new();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var changePasswordResponse = await _manageService.ChangePassword(model, UserId);
            if (changePasswordResponse == ResponseTypes.Success)
            {
                return Ok(new ApiResponse<bool> { Message = ResourceString.PasswordUpdated });
            }
            else if (changePasswordResponse == ResponseTypes.OldPasswordWrong)
            {
                return BadRequest(new ApiResponse<bool> { Message = ResourceString.WrongOldPassword });
            }
            else if (changePasswordResponse == ResponseTypes.OldNewPasswordMatched)
            {
                return BadRequest(new ApiResponse<bool> { Message = ResourceString.OldNewPasswordNotSame });
            }
            else
            {
                return BadRequest(new ApiResponse<bool> { Message = ResourceString.FailedToSetNewPassword });
            }
        }
    }
}
