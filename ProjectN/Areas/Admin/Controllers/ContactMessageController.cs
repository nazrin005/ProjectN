using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactMessageController : Controller
    {
        private readonly AppDbContext _db;
        public ContactMessageController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _db.ContactMessages
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return View(messages);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var message = await _db.ContactMessages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (message == null)
                return NotFound();

            return View(message);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var message = await _db.ContactMessages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (message == null)
                return NotFound();

            return View(message);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _db.ContactMessages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (message == null)
                return NotFound();

            _db.ContactMessages.Remove(message);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
