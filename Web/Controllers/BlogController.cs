using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    public class BlogController(AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("blog/{slug}")]
        public IActionResult Details(string slug)
        {
            var article = context.Articles
                .FirstOrDefault(x => x.Slug == slug && x.IsActive);

            if (article == null)
                return NotFound();

            return View(article);
        }
    }
}
