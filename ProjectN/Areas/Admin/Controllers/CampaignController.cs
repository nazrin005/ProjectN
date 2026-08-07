using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.Campaigns;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.Utilities;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CampaignController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CampaignController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task< IActionResult> Index()
        {
            List<Campaign> campaigns = await _db.Campaigns.ToListAsync();

            return View(campaigns);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignVM campaignVM)
        {
            if (!ModelState.IsValid)
                return View(campaignVM);

            if (campaignVM.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View(campaignVM);
            }

            if (campaignVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                return View(campaignVM);
            }

            if (!campaignVM.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Image must be image type");
                return View(campaignVM);
            }

            Campaign campaign = new Campaign()
            {
                Title = campaignVM.Title,
                Subtitle = campaignVM.Subtitle,
                Description = campaignVM.Description,
                ButtonText = campaignVM.ButtonText,
                ButtonUrl = campaignVM.ButtonUrl,
                IsActive = campaignVM.IsActive,
                ImageUrl = campaignVM.ImageFile.SaveImage(_env, "uploads/campaigns")
            };

            await _db.Campaigns.AddAsync(campaign);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            Campaign? campaign = await _db.Campaigns.FindAsync(id);

            if (campaign is null)
                return NotFound();

            campaign.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id is null)
                return BadRequest();

            Campaign? campaign = await _db.Campaigns.FindAsync(id);

            if (campaign is null)
                return NotFound();

            campaign.IsDeleted = false;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id is null)
                return BadRequest();

            Campaign? campaign = await _db.Campaigns
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign is null)
                return NotFound();

            UpdateCampaignVM campaignVM = new UpdateCampaignVM()
            {
                Id = campaign.Id,
                Title = campaign.Title,
                Subtitle = campaign.Subtitle,
                Description = campaign.Description,
                ButtonText = campaign.ButtonText,
                ButtonUrl = campaign.ButtonUrl,
                ImageUrl = campaign.ImageUrl,
                IsActive = campaign.IsActive
            };

            return View(campaignVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCampaignVM campaignVM)
        {
            if (!ModelState.IsValid)
                return View(campaignVM);

            Campaign? campaign = await _db.Campaigns
                .FirstOrDefaultAsync(c => c.Id == campaignVM.Id);

            if (campaign is null)
                return NotFound();

            if (campaignVM.ImageFile is not null)
            {
                if (campaignVM.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                    return View(campaignVM);
                }

                if (!campaignVM.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Image must be image type");
                    return View(campaignVM);
                }

                campaign.ImageUrl = campaignVM.ImageFile.SaveImage(_env, "uploads/campaigns");
            }

            campaign.Title = campaignVM.Title;
            campaign.Subtitle = campaignVM.Subtitle;
            campaign.Description = campaignVM.Description;
            campaign.ButtonText = campaignVM.ButtonText;
            campaign.ButtonUrl = campaignVM.ButtonUrl;
            campaign.IsActive = campaignVM.IsActive;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
