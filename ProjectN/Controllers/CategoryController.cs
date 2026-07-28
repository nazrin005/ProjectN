using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.ViewModels.Category;

namespace ProjectN.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public CategoryController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _db.Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null)
            {
                return NotFound();
            }

            var products = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Wishlists)
                .Where(p => p.CategoryId == id && !p.IsDeleted)
                .ToListAsync();

            AppUser? user = await _userManager.GetUserAsync(User);
            ViewBag.UserId = user?.Id;

            CategoryDetailsVM vm = new()
            {
                Category = category,
                Products = products
            };

            return View(vm);
        }
    }
}
