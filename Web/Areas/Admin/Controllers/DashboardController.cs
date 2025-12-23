using Microsoft.AspNetCore.Mvc;
using Shared.Common;

namespace Web.Areas.Admin.Controllers
{
    [Area(areaName: "Admin")]
    [ValidateModel]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            ViewBag.LocalDateTime = DateTime.UtcNow.ToLocal(_httpContextAccessor);
            return View();
        }
    }
}
