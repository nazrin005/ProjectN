using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Controllers
{
    [Authorize(Roles = "User")]
    public class WishlistController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public WishlistController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            var wishlists = await _db.Wishlists
            .Include(w => w.Product)
            .ThenInclude(p => p.Category)
            .Where(w => w.AppUserId == user.Id)
            .ToListAsync();
            return View(wishlists);
        }
        public async Task<IActionResult> Toggle(int id)
        {
            AppUser user = await _userManager.GetUserAsync(User);

            Product? product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            Wishlist? wishlist = await _db.Wishlists
                .FirstOrDefaultAsync(w =>
                    w.ProductId == id &&
                    w.AppUserId == user.Id);

            if (wishlist != null)
            {
                _db.Wishlists.Remove(wishlist);
            }
            else
            {
                wishlist = new Wishlist
                {
                    ProductId = id,
                    AppUserId = user.Id
                };

                await _db.Wishlists.AddAsync(wishlist);
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Index","Home", new { id });
        }
    }
}
