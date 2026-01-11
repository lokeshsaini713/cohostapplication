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

            // 🔥 RELATED ARTICLES
            var relatedArticles = context.Articles
                .Where(x => x.Category == article.Category
                         && x.Id != article.Id
                         && x.IsActive)
                .OrderByDescending(x => x.PublishedDate)
                .Take(3)
                .ToList();

            ViewBag.RelatedArticles = relatedArticles;

            return View(article);
        }


    }
}
