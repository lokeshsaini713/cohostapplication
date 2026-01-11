using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Shared.Model.DTO;
using Shared.Utility;
[Area("Admin")]
public class CaseStudiesController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    public CaseStudiesController(AppDbContext context, IWebHostEnvironment env  )
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var data = _context.CaseStudies.ToList();

        //var data = _context.CaseStudies
        //    .OrderBy(x => x.SortOrder)
        //    .ToList();

        return View(data);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CaseStudy model, IFormFile imageFile)
    {
        if (imageFile != null)
        {
            model.ImagePath = await SaveImage(imageFile);
        }

        // AUTO SLUG (fallback)
        if (string.IsNullOrEmpty(model.Slug))
            model.Slug = model.Title.ToLower().Replace(" ", "-");

        model.Technology = model.Technologies;
        model.CountryCode ="India";
        _context.CaseStudies.Add(model);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    private async Task<string> SaveImage(IFormFile image)
    {
        string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "casestudy");
        Directory.CreateDirectory(uploadsFolder);

        string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        string filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        return "/uploads/casestudy/" + fileName;
    }

    public  IActionResult Edit(int id)
    {
        return View(_context.CaseStudies.Find(id));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CaseStudy model, IFormFile imageFile)
    {
        var data = _context.CaseStudies.Find(model.Id);

        if (imageFile != null && imageFile.Length > 0)
        {
            data.ImagePath = await SaveImage(imageFile);

            //var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            //    var path = Path.Combine("wwwroot/assets/uploads/casestudy", fileName);

            //    using var stream = new FileStream(path, FileMode.Create);
            //    imageFile.CopyTo(stream);

            data.ImagePath = model.ImagePath;
        }

        data.Title = model.Title;
        if (string.IsNullOrEmpty(model.Slug))
            model.Slug = model.Title.ToLower().Replace(" ", "-");
        data.Technology = model.Technologies;
        data.CountryCode = "India";
        data.ShortDescription = model.ShortDescription;
        data.Category = model.Category;
        data.Technology = model.Technology;
        data.CountryCode = model.CountryCode;
        data.IsActive = model.IsActive;
        data.SortOrder = model.SortOrder;

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var data = _context.CaseStudies.Find(id);
        _context.CaseStudies.Remove(data);
        _context.SaveChanges();
        return Json(true);
    }
}

