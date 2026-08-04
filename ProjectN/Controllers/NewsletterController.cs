using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly AppDbContext _db;
        public NewsletterController(AppDbContext db)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe(Newsletter newsletter, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter a valid email address.";

                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }

            bool exists = await _db.Newsletters
                .AnyAsync(x => x.Email == newsletter.Email);

            if (exists)
            {
                TempData["Error"] = "This email is already subscribed.";

                return Redirect(returnUrl ?? Url.Action("Index", "Home"));
            }

            _db.Newsletters.Add(newsletter);

            await _db.SaveChangesAsync();

            TempData["Success"] = "You have successfully subscribed!";

            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }
    }
}
