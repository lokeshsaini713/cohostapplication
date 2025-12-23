using Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Shared.Common;
using System.Security.Claims;

namespace Api.Controllers.Base
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Policy = "AuthorisedUser")]
    [AuthorizeHeader]
    [ValidateApiModel]
    [Route("v{version:apiVersion}/[controller]")]
    public class ApiBaseController : Controller
    {

        private ClaimsIdentity? Identity
        {
            get
            {
                return (ClaimsIdentity?)User.Identity;
            }
        }
        private IQueryable<Claim> IdentityClaim()
        {
            ClaimsIdentity identity = Identity == null ? new ClaimsIdentity() : Identity;
            return identity.Claims.AsQueryable();
        }
        private string? GetClaimByValue(string value)
        {
            return IdentityClaim().Where(c => c.Type.Equals(value)).Select(c => c.Value).SingleOrDefault();
        }
        public int UserId
        {
            get
            {
                var userId = GetClaimByValue("userId");
                if (userId != null)
                {
                    _ = int.TryParse(userId, out int numuserId);
                    return numuserId;
                }
                return 0;
            }
        }
        public string Offset
        {
            get
            {
                var offset = GetClaimByValue("Offset");
                if (offset != null)
                {
                    return offset;
                }
                return "";
            }
        }
        
    }
}
