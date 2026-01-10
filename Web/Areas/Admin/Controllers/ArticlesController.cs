using Data;
using Microsoft.AspNetCore.Mvc;

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
        return View(_context.Articles.OrderByDescending(x => x.PublishedDate).ToList());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Article model, IFormFile image)
    {
        if (image != null)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads/articles");
            Directory.CreateDirectory(folder);

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(folder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);

            model.ImagePath = "/uploads/articles/" + fileName;
        }

        _context.Articles.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var article = _context.Articles.Find(id);
        return View(article);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Article model, IFormFile image)
    {
        if (image != null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var path = Path.Combine(_env.WebRootPath, "uploads/articles", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);

            model.ImagePath = "/uploads/articles/" + fileName;
        }

        _context.Articles.Update(model);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
