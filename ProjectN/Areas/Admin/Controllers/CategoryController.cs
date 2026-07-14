using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.Categories;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _db.Categories.ToListAsync();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View(categoryVM);
            Category category = new Category()
            {
                Name = categoryVM.Name
            };
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _db.Categories.FindAsync(id);
            category.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Category category = await _db.Categories.FindAsync(id);
            category.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Category category = await _db.Categories.FindAsync(id);
            UpdateCategoryVM categoryVM = new UpdateCategoryVM()
            {
                Id = category.Id,
                Name = category.Name
            };
            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View(categoryVM);
            Category oldCategory = await _db.Categories.FindAsync(categoryVM.Id);
            oldCategory.Name = categoryVM.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
