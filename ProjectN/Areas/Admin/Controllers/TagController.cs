using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.Tags;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _db;
        public TagController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _db.Tags.ToListAsync();
            return View(tags);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM tagVM)
        {
            if (!ModelState.IsValid) return View(tagVM);
            Tag tag = new Tag()
            {
                Name = tagVM.Name
            };
            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Tag tag = await _db.Tags.FindAsync(id);
            tag.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Tag tag = await _db.Tags.FindAsync(id);
            tag.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Tag tag = await _db.Tags.FindAsync(id);
            UpdateTagVM tagVM = new UpdateTagVM()
            {
                Id = tag.Id,
                Name = tag.Name
            };
            return View(tagVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateTagVM tagVM)
        {
            if (!ModelState.IsValid) return View(tagVM);
            Tag oldTag = await _db.Tags.FindAsync(tagVM.Id);
            oldTag.Name = tagVM.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
