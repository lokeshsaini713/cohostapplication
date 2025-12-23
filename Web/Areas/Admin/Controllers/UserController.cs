using Business.IServices;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Model.Base;
using Shared.Model.DTO;
using Shared.Model.Request.Account;
using Shared.Model.Request.Admin;
using Shared.Resources;
using Web.Areas.Admin.Controllers.Base;

namespace Web.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [ValidateModel]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class UserController : AdminBaseController
    {
        private readonly IManageService _manageService;
        public UserController(IManageService manageService)
        {
            _manageService = manageService;
        }
        public IActionResult Index()
        {
            UsersDto userlst = new UsersDto();
            ViewBag.PageIndex = Constants.DefultPageNumber;
            ViewBag.PageSize = Constants.DefultPageSize;
            return View(userlst);
        }

        [HttpPost]
        public async Task<JsonResult> Index(UsersRequestModel model)
        {
            var items = await _manageService.UserList(model);
            var users = items.Data;
            var result = new DataTableResult<UsersDto>
            {
                Draw = model.Draw,
                Data = users,
                RecordsFiltered = users?.FirstOrDefault()?.TotalRecord ?? 0,
                RecordsTotal = users?.Count() ?? 0
            };
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(int? id)
        {
            if (id > 0)
            {
                var userDetails = await _manageService.GetUserDetails(id.Value);
                if (userDetails.Data?.UserId > 0 && userDetails.Data?.UserType == (int)UserTypes.User)
                    return View(userDetails.Data);
            }
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Detail(UserDetailsDto requestmodel)
        {
            var updateUser = await _manageService.UpdateUserDetail(requestmodel);
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
            var changeAdminPassword = await _manageService.ChangePassword(model, UserId);

            return changeAdminPassword switch
            {
                ResponseTypes.Success => Ok(new ApiResponse<bool> { Message = ResourceString.PasswordUpdated }),
                ResponseTypes.OldPasswordWrong => BadRequest(new ApiResponse<bool> { Message = ResourceString.WrongOldPassword }),
                _ => BadRequest(new ApiResponse<bool> { Message = ResourceString.FailedToSetNewPassword }),
            };
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(int userId, bool activeStatus, bool deleteStatus, bool isActiveStatusChange)
        {
            var changeUserStatus = await _manageService.ChangeUserStatus(userId, activeStatus, deleteStatus);
            if (changeUserStatus > 0)
            {
                if (isActiveStatusChange)
                {
                    if (activeStatus)
                    {
                        return Ok(new ApiResponse<bool> { Message = ResourceString.UserActivated });
                    }
                    else
                    {
                        return Ok(new ApiResponse<bool> { Message = ResourceString.UserDeactivate });
                    }
                }
                else
                {
                    if (deleteStatus)
                    {
                        return Ok(new ApiResponse<bool> { Message = ResourceString.UserDeleteSuccess });
                    }
                    else
                    {
                        return Ok(new ApiResponse<bool> { Message = ResourceString.RecoverUser });
                    }
                }
            }
            else
            {
                if (isActiveStatusChange)
                {
                    if (activeStatus)
                    {
                        return BadRequest(new ApiResponse<bool> { Message = ResourceString.UserActivateFailed });
                    }
                    else
                    {
                        return BadRequest(new ApiResponse<bool> { Message = ResourceString.UserDeactivateFailed });
                    }
                }
                else
                {
                    if (deleteStatus)
                    {
                        return BadRequest(new ApiResponse<bool> { Message = ResourceString.UserDeleteFailed });
                    }
                    else
                    {
                        return BadRequest(new ApiResponse<bool> { Message = ResourceString.UserAccountRecoverFailed });
                    }
                }
            }
        }
    }
}
