using Data;
using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("admin")]
public class ContactController : Controller
{
    private readonly AppDbContext _db;

    public ContactController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Leads(string search)
    {
        var query = _db.Leads.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.FullName.Contains(search) ||
                x.Email.Contains(search) ||
                x.Phone.Contains(search)
            );
        }

        var leads = await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return View(leads);
    }
}
