using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Shared.Model.DTO;
using Shared.Utility;

namespace YourNamespace.Areas.Admin.Controllers
{
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
            var model = new Article();
            model.SortOrder = _context.Articles.Any()
                ? _context.Articles.Max(x => x.SortOrder) + 1
                : 1;
            return View(model); // Fixed: was returning new Article() instead of model
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article model, IFormFile image)
        {
            // Remove image from ModelState validation
            ModelState.Remove("image");
            ModelState.Remove("ImagePath");
            ModelState.Remove("Slug");
            ModelState.Remove("PublishedDate");
            ModelState.Remove("MetaTitle");
            ModelState.Remove("MetaDescription");
            ModelState.Remove("MetaKeywords");

            if (!ModelState.IsValid)
            {
                // Log validation errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(model);
            }

            // Generate Slug
            model.Slug = SlugHelper.Generate(model.Title);

            // Image Upload
            if (image != null && image.Length > 0)
            {
                model.ImagePath = await SaveImage(image);
            }

            model.SortOrder = model.SortOrder < 0 ? 0 : model.SortOrder;
            model.PublishedDate = DateTime.Now;

            // AUTO SLUG (fallback)
            if (string.IsNullOrEmpty(model.Slug))
                model.Slug = model.Title.ToLower().Replace(" ", "-");

            // AUTO META FALLBACKS (BEST PRACTICE)
            model.MetaTitle ??= model.Title;
            model.MetaDescription ??= model.ShortDescription;

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
   
        public async Task<IActionResult> Edit(Article model, IFormFile image)
        {
            // Remove validation for optional fields
            ModelState.Remove("image");
            ModelState.Remove("ImagePath");
            ModelState.Remove("Slug");
            ModelState.Remove("PublishedDate");
            ModelState.Remove("MetaTitle");
            ModelState.Remove("MetaDescription");
            ModelState.Remove("MetaKeywords");

            if (!ModelState.IsValid)
            {
                // Log validation errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(model);
            }

            // Get existing record
            var existing = await _context.Articles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (existing == null)
                return NotFound();

            model.SortOrder = model.SortOrder < 0 ? 0 : model.SortOrder;

            // Keep old slug (important for SEO)
            model.Slug = existing.Slug;

            // AUTO META FALLBACKS (BEST PRACTICE)
            model.MetaTitle ??= model.Title;
            model.MetaDescription ??= model.ShortDescription;

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

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id, bool isActive)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
                return NotFound();

            article.IsActive = isActive;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, isActive = article.IsActive });
        }

        [HttpPost]
        public IActionResult UpdateSortOrder([FromBody] List<SortOrderDto> list)
        {
            foreach (var item in list)
            {
                var article = _context.Articles.Find(item.Id);
                if (article != null)
                {
                    article.SortOrder = item.SortOrder;
                }
            }

            _context.SaveChanges();
            return Ok();
        }
    }
}
