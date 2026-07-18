using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.ViewModels.Review;

namespace ProjectN.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(AppDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
       
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateReviewVM reviewVM)
        {
            if (!ModelState.IsValid)
            {
                return Content("ModelState Invalid");
            }

            AppUser user = await _userManager.GetUserAsync(User);

            Product? product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == reviewVM.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            bool hasReview = await _db.Reviews.AnyAsync(r =>
                r.ProductId == reviewVM.ProductId &&
                r.AppUserId == user.Id);

            if (hasReview)
            {
                TempData["ReviewError"] = "You have already reviewed this product.";
                return RedirectToAction("Details", "Home", new { id = reviewVM.ProductId });
            }

            Review review = new Review()
            {
                Comment = reviewVM.Comment,
                Rating = reviewVM.Rating,
                ProductId = reviewVM.ProductId,
                AppUserId = user.Id
            };

            await _db.Reviews.AddAsync(review);
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Home", new { id = reviewVM.ProductId });
        }
    }
}
