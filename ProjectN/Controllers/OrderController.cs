using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Controllers
{
    [Authorize(Roles = "User")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Checkout()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Cart> carts = await _db.Carts
                .Include(c => c.Product)
                .Where(c => c.AppUserId == user.Id)
                .ToListAsync();

            if (!carts.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            return View(carts);
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Cart> carts = await _db.Carts
                .Include(c => c.Product)
                .Where(c => c.AppUserId == user.Id)
                .ToListAsync();

            if (!carts.Any())
            {
                return RedirectToAction(nameof(Checkout));
            }

            Order order = new Order
            {
                AppUserId = user.Id,
                TotalPrice = carts.Sum(c => c.Product.Price * c.Count)
            };

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            foreach (Cart cart in carts)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cart.ProductId,
                    Count = cart.Count,
                    Price = cart.Product.Price
                };

                await _db.OrderItems.AddAsync(orderItem);
            }

            _db.Carts.RemoveRange(carts);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Index()
        {
            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Order> orders = await _db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.AppUserId == user.Id)
                .ToListAsync();

            return View(orders);
        }
    }
}
