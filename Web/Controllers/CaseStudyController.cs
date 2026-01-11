using Data;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class CaseStudyController(AppDbContext context) : Controller
{

    // Index Page
    public IActionResult Index()
    {
        return View();
    }

    // AJAX Data
    [HttpGet]
    [Route("CaseStudy/GetCaseStudies")]
    public async Task<IActionResult> GetCaseStudies()
    {
        var data = await context.CaseStudies
            .Where(x => x.IsActive)
            .OrderBy(x => x.SortOrder)
            .Select(x => new
            {
                x.Id,
                x.Title,
                x.Slug,
                x.ShortDescription,
                x.ImagePath,
                x.Category,
                x.Technology,
                x.CountryCode
            })
            .ToListAsync();

        return Json(data);
    }
}
