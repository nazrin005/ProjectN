using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.Products;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.Utilities;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductController( AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products
                .Include(p=>p.Category)
                .Include(p=>p.Tags)
                .ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (!ModelState.IsValid)
                return View(productVM);

            if (productVM.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View(productVM);
            }

            if (productVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                return View(productVM);
            }

            if (!productVM.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Image must be image type");
                return View(productVM);
            }

            Product product = new Product()
            {
                Name = productVM.Name,
                Price = productVM.Price,
                Description = productVM.Description,
                CategoryId = productVM.CategoryId,
                ImageUrl = productVM.ImageFile.SaveImage(_env, "uploads/products")
            };

            if (productVM.TagIds is not null)
            {
                product.Tags = await _db.Tags
                    .Where(t => productVM.TagIds.Contains(t.Id))
                    .ToListAsync();
            }

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            // Additional Images
            if (productVM.ImageFiles != null && productVM.ImageFiles.Any())
            {
                foreach (var image in productVM.ImageFiles)
                {
                    if (image.Length > 2 * 1024 * 1024)
                        continue;

                    if (!image.ContentType.Contains("image/"))
                        continue;

                    ProductImage productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        ImageUrl = image.SaveImage(_env, "uploads/products")
                    };

                    await _db.ProductImages.AddAsync(productImage);
                }

                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            Product product = await _db.Products.FindAsync(id);
            product.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            Product product = await _db.Products.FindAsync(id);
            product.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {

            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            Product product = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.Tags)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            UpdateProductVM productVM = new UpdateProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                TagIds = product.Tags.Select(t => t.Id).ToList(),
                ProductImages = product.ProductImages.ToList()
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM productVM)
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();

            if (!ModelState.IsValid)
                return View(productVM);

            Product? oldProduct = await _db.Products
                .Include(p => p.Tags)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productVM.Id);

            if (oldProduct == null)
                return NotFound();

            oldProduct.Name = productVM.Name;
            oldProduct.Description = productVM.Description;
            oldProduct.Price = productVM.Price;
            oldProduct.CategoryId = productVM.CategoryId;

            // Main Image
            if (productVM.ImageFile != null)
            {
                if (productVM.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                    return View(productVM);
                }

                if (!productVM.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Image must be image type");
                    return View(productVM);
                }

                oldProduct.ImageUrl.DeleteFile(_env, "uploads/products");

                oldProduct.ImageUrl = productVM.ImageFile.SaveImage(_env, "uploads/products");
            }

            // Tags
            oldProduct.Tags.Clear();

            if (productVM.TagIds != null)
            {
                oldProduct.Tags = await _db.Tags
                    .Where(t => productVM.TagIds.Contains(t.Id))
                    .ToListAsync();
            }

            // Additional Images
            if (productVM.ImageFiles != null && productVM.ImageFiles.Any())
            {
                foreach (var image in productVM.ImageFiles)
                {
                    if (image.Length > 2 * 1024 * 1024)
                        continue;

                    if (!image.ContentType.Contains("image/"))
                        continue;

                    ProductImage productImage = new ProductImage
                    {
                        ProductId = oldProduct.Id,
                        ImageUrl = image.SaveImage(_env, "uploads/products")
                    };

                    await _db.ProductImages.AddAsync(productImage);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteImage(int id)
        {
            ProductImage? image = await _db.ProductImages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
                return NotFound();

            int productId = image.ProductId;

            image.ImageUrl.DeleteFile(_env, "uploads/products");

            _db.ProductImages.Remove(image);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Update), new { id = productId });
        }
    }
}
