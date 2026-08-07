using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.HeroBanners;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.Utilities;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HeroBannerController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public HeroBannerController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<HeroBanner> heroBanners = await _db.HeroBanners.ToListAsync();
            return View(heroBanners);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateHeroBannerVM heroBannerVM)
        {
            if (!ModelState.IsValid)
                return View(heroBannerVM);

            if (heroBannerVM.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View(heroBannerVM);
            }

            if (heroBannerVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                return View(heroBannerVM);
            }

            if (!heroBannerVM.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Image must be image type");
                return View(heroBannerVM);
            }

            HeroBanner heroBanner = new HeroBanner()
            {
                Title = heroBannerVM.Title,
                Subtitle = heroBannerVM.Subtitle,
                Description = heroBannerVM.Description,
                ButtonText = heroBannerVM.ButtonText,
                ButtonUrl = heroBannerVM.ButtonUrl,
                IsActive = heroBannerVM.IsActive,
                ImageUrl = heroBannerVM.ImageFile.SaveImage(_env, "uploads/heroBanners")
            };

            await _db.HeroBanners.AddAsync(heroBanner);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            HeroBanner? heroBanner = await _db.HeroBanners.FindAsync(id);

            if (heroBanner is null)
                return NotFound();

            heroBanner.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id is null)
                return BadRequest();

            HeroBanner? heroBanner = await _db.HeroBanners.FindAsync(id);

            if (heroBanner is null)
                return NotFound();

            heroBanner.IsDeleted = false;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            HeroBanner? heroBanner = await _db.HeroBanners
                .FirstOrDefaultAsync(c => c.Id == id);

            if (heroBanner is null)
                return NotFound();

            UpdateHeroBannerVM heroBannerVM = new UpdateHeroBannerVM()
            {
                Id = heroBanner.Id,
                Title = heroBanner.Title,
                Subtitle = heroBanner.Subtitle,
                Description = heroBanner.Description,
                ButtonText = heroBanner.ButtonText,
                ButtonUrl = heroBanner.ButtonUrl,
                ImageUrl = heroBanner.ImageUrl,
                IsActive = heroBanner.IsActive
            };

            return View(heroBannerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateHeroBannerVM heroBannerVM)
        {
            if (!ModelState.IsValid)
                return View(heroBannerVM);

            HeroBanner? heroBanner = await _db.HeroBanners
                .FirstOrDefaultAsync(c => c.Id == heroBannerVM.Id);

            if (heroBanner is null)
                return NotFound();

            if (heroBannerVM.ImageFile is not null)
            {
                if (heroBannerVM.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                    return View(heroBannerVM);
                }

                if (!heroBannerVM.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Image must be image type");
                    return View(heroBannerVM);
                }

                heroBanner.ImageUrl = heroBannerVM.ImageFile.SaveImage(_env, "uploads/campaigns");
            }

            heroBanner.Title = heroBannerVM.Title;
            heroBanner.Subtitle = heroBannerVM.Subtitle;
            heroBanner.Description = heroBannerVM.Description;
            heroBanner.ButtonText = heroBannerVM.ButtonText;
            heroBanner.ButtonUrl = heroBannerVM.ButtonUrl;
            heroBanner.IsActive = heroBannerVM.IsActive;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
