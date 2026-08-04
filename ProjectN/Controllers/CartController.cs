using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public CartController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            List<Cart> carts = await _db.Carts
                .Include(c => c.Product)
                    .ThenInclude(p => p.Category)
                .Where(c => c.AppUserId == user.Id)
                .ToListAsync();

            return View(carts);
        }

        public async Task<IActionResult> AddToCart(int id, int count = 1)
        {
            if (count < 1)
                count = 1;

            AppUser? user = await _userManager.GetUserAsync(User);

            Product? product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            Cart? cart = await _db.Carts.FirstOrDefaultAsync(c =>
                c.ProductId == id &&
                c.AppUserId == user.Id);

            if (cart != null)
            {
                cart.Count += count;
            }
            else
            {
                cart = new Cart
                {
                    ProductId = id,
                    AppUserId = user.Id,
                    Count = count
                };

                await _db.Carts.AddAsync(cart);
            }

            await _db.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public async Task<IActionResult> Increase(int id)
        {
            Cart? cart = await _db.Carts
        .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                return NotFound();
            }

            cart.Count++;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Decrease(int id)
        {
            Cart? cart = await _db.Carts
        .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                return NotFound();
            }
            if (cart.Count > 1)
            {
                cart.Count--;
            }
            else
            {
                _db.Carts.Remove(cart);
            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Remove(int id)
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            Cart? cart = await _db.Carts
                .FirstOrDefaultAsync(c => c.Id == id && c.AppUserId == user.Id);

            if (cart == null)
            {
                return NotFound();
            }

            _db.Carts.Remove(cart);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
