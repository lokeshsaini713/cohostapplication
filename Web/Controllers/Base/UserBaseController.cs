using Business.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Extensions;
using Shared.Model.Request.Account;

namespace Web.Controllers.Base
{
    public class UserBaseController : Controller
    {
        public UserBaseController()
        {
        }
        public int UserId { get; init; } = LoginMemberSession.UserDetailSession?.UserId ?? 0;
        

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LoginSessionModel? userObj = HttpContext.Session.GetComplexData<LoginSessionModel>("LoginMemberSession");

            if (userObj != null)
            {
                if (userObj.UserTypeId != (int)UserTypes.User)
                {
                    TempData["ReturnUrl"] = context.HttpContext.Request.Path.ToString();

                    context.Result = RedirectToAction("Logout", "Account", new { area = "" });
                }
                else
                {
                    IAccountService? commonServic = context.HttpContext.RequestServices.GetService(typeof(IAccountService)) as IAccountService;

                    if (commonServic != null && !string.IsNullOrEmpty(userObj.EmailId))
                    {
                        var isUserActive = commonServic.FindByEmailAsync(email: userObj.EmailId.ToString());
                        var controller = context.Controller as ControllerBase;
                        if (!(isUserActive.Result.IsActive ?? false))
                        {
                            if(controller != null)
                            {
                                context.Result = controller.RedirectToAction("Logout", "Account", new { area = "" });
                            }
                            
                            return;
                        }
                    }
                }
            }
            else
            {
                var requestType = HttpContext.Request.Headers["X-Requested-With"];

                if (!string.IsNullOrEmpty(requestType) && requestType == "XMLHttpRequest")
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    TempData["ReturnUrl"] = context.HttpContext.Request.Path.ToString();

                    context.Result = RedirectToAction("Logout", "Account", new { area = "" });
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
