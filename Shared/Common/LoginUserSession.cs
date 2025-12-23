using Microsoft.AspNetCore.Http;
using Shared.Extensions;

namespace Shared.Common
{
    public static class LoginMemberSession
    {
        private static HttpContext HttpContext => new HttpContextAccessor().HttpContext;
        public static LoginSessionModel? UserDetailSession
        {
            get
            {
                var data = HttpContext.Session.GetComplexData<LoginSessionModel>("LoginMemberSession") == null ? null : HttpContext.Session.GetComplexData<LoginSessionModel>("LoginMemberSession");
                return data;
            }
            set
            {
                HttpContext.Session.SetComplexData("LoginMemberSession", value);
            }
        }

    }

    public class LoginSessionModel
    {
        public int UserId { get; set; }
        public string? EmailId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        private string? _userName { get; set; }
        public string? UserName { get { return this._userName; } set => this._userName = FirstName + " " + LastName; }
        public int UserTypeId { get; set; }
        public string? ProfileImage { get; set; }
    }
}
