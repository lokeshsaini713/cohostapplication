using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Extensions;
using Shared.Model.Request.Account;

namespace Web.Areas.Admin.Controllers.Base
{
    public class AdminBaseController : Controller
    {
        public AdminBaseController()
        {
        }
        public int UserId { get; init; } = LoginMemberSession.UserDetailSession?.UserId ?? 0;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginSessionModel? userObj = HttpContext.Session.GetComplexData<LoginSessionModel>("LoginMemberSession");

            if (userObj != null)
            {
                if (userObj.UserTypeId != (int)UserTypes.Admin)
                {

                    TempData["ReturnUrl"] = filterContext.HttpContext.Request.Path.ToString();

                    filterContext.Result = RedirectToAction("Logout", "Account", new { area = "" });
                }
                
            }
            else
            {
                var requestType = HttpContext.Request.Headers["X-Requested-With"];

                if (!string.IsNullOrEmpty(requestType) && requestType == "XMLHttpRequest")
                {
                    filterContext.Result = new UnauthorizedResult();
                }
                else
                {
                    TempData["ReturnUrl"] = filterContext.HttpContext.Request.Path.ToString();

                    filterContext.Result = RedirectToAction("Logout", "Account", new { area = "" });
                }
            }
        }
    }
}
