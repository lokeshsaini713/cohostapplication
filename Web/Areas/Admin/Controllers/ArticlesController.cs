using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Shared.Utility;

[Area("Admin")]
public class ArticlesController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ArticlesController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        return View(_context.Articles
    .OrderBy(x => x.SortOrder)
    .ThenByDescending(x => x.PublishedDate)
    .ToList());
    }
    public async Task<IActionResult> Create()
    {
        var model= new Article();
        model.SortOrder = _context.Articles.Any()
    ? _context.Articles.Max(x => x.SortOrder) + 1
    : 1;
        return View(new Article());
    }
        [HttpPost]
    
    public async Task<IActionResult> Create(Article model, IFormFile image)
    {
        //if (!ModelState.IsValid)
        //    return View(model);

        // Generate Slug
        model.Slug = SlugHelper.Generate(model.Title);

        // Image Upload
        if (image != null && image.Length > 0)
        {
            model.ImagePath = await SaveImage(image);
        }
        model.SortOrder = model.SortOrder < 0 ? 0 : model.SortOrder;

        model.PublishedDate = DateTime.Now;

        _context.Articles.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // EDIT - GET
    // =========================
    public IActionResult Edit(int id)
    {
        var article = _context.Articles.Find(id);
        if (article == null)
            return NotFound();

        return View(article);
    }

    // =========================
    // EDIT - POST (UPDATE)
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Article model, IFormFile image)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Get existing record
        var existing = await _context.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == model.Id);
        model.SortOrder = model.SortOrder < 0 ? 0 : model.SortOrder;

        if (existing == null)
            return NotFound();

        // Keep old slug (important for SEO)
        model.Slug = existing.Slug;

        // Handle Image Change
        if (image != null && image.Length > 0)
        {
            model.ImagePath = await SaveImage(image);
        }
        else
        {
            model.ImagePath = existing.ImagePath;
        }

        model.PublishedDate = existing.PublishedDate;

        _context.Articles.Update(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // IMAGE SAVE METHOD
    // =========================
    private async Task<string> SaveImage(IFormFile image)
    {
        string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "articles");
        Directory.CreateDirectory(uploadsFolder);

        string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
        string filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(stream);

        return "/uploads/articles/" + fileName;
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSlug(int id, string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return BadRequest("Slug is required");

        bool exists = await _context.Articles
            .AnyAsync(x => x.Slug == slug && x.Id != id);

        if (exists)
            return Conflict("Slug already exists");

        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();

        article.Slug = SlugHelper.Generate(slug);

        await _context.SaveChangesAsync();

        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var article = await _context.Articles.FindAsync(id);

        if (article == null)
            return NotFound();

        article.IsActive = false;

        await _context.SaveChangesAsync();

        return Ok();
    }

}
