using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.ViewModels.Home;

namespace ProjectN.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products
                .Include(p => p.Category)
                .Include(p=>p.Tags)
                .Include(p => p.Wishlists)
                .ToListAsync();
            List<Product> bestSellers = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.OrderItems)
                .OrderByDescending(p => p.OrderItems.Sum(oi => oi.Count))
                .Take(4)
                .ToListAsync();
            AppUser? user = await _userManager.GetUserAsync(User);

            ViewBag.UserId = user?.Id;
            HomeVM vm = new HomeVM
            {
                Products = products,
                BestSellers = bestSellers
            };

            return View(vm);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return View();
            }
            Product? product = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.AppUser)
                .Include(p => p.Wishlists)
                .FirstOrDefaultAsync(p => p.Id == id);
            AppUser? user = await _userManager.GetUserAsync(User);
            ViewBag.UserId = user?.Id;
            return View(product);
        }
    }
}
