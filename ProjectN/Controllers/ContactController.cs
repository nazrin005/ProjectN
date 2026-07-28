using Microsoft.AspNetCore.Mvc;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.ViewModels.Contact;

namespace ProjectN.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _db;

        public ContactController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new CreateContactMessageVM());
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(CreateContactMessageVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill in all required fields.";
                return RedirectToAction(nameof(Index));
            }

            ContactMessage message = new ContactMessage
            {
                Name = vm.Name,
                Email = vm.Email,
                Subject = vm.Subject,
                Message = vm.Message
            };

            await _db.ContactMessages.AddAsync(message);
           await _db.SaveChangesAsync();

           TempData["Success"] = "Your message has been sent successfully!";

           return RedirectToAction(nameof(Index));
        }
    }
}