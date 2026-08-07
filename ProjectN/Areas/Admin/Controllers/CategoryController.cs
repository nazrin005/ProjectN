using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.Categories;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.Utilities;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
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

            if (!categoryVM.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "File must be an image.");
                return View(categoryVM);
            }

            if (categoryVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image size cannot exceed 2 MB.");
                return View(categoryVM);
            }

            Category category = new Category()
            {
                Name = categoryVM.Name,
                Description = categoryVM.Description,
                ImageUrl = categoryVM.ImageFile.SaveImage(_env, "uploads/categories")
            };

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _db.Categories.FindAsync(id);

            if (category == null) return NotFound();

            category.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Category? category = await _db.Categories.FindAsync(id);

            if (category == null) return NotFound();

            category.IsDeleted = false;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Category? category = await _db.Categories.FindAsync(id);

            if (category == null) return NotFound();

            UpdateCategoryVM categoryVM = new()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };

            return View(categoryVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View(categoryVM);

            Category? oldCategory = await _db.Categories.FindAsync(categoryVM.Id);

            if (oldCategory == null) return NotFound();

            oldCategory.Name = categoryVM.Name;
            oldCategory.Description = categoryVM.Description;

            if (categoryVM.ImageFile != null)
            {
                if (!categoryVM.ImageFile.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("ImageFile", "File must be an image.");
                    return View(categoryVM);
                }

                if (categoryVM.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image size cannot exceed 2 MB.");
                    return View(categoryVM);
                }

                oldCategory.ImageUrl = categoryVM.ImageFile.SaveImage(_env, "uploads/categories");
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
