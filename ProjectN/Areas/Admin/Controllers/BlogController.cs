using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.Areas.Admin.ViewModels.Blogs;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.Utilities;

namespace ProjectN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _db.Blogs
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(blogs);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogVM blogVM)
        {
            if (!ModelState.IsValid)
                return View(blogVM);

            if (blogVM.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image is required");
                return View(blogVM);
            }

            if (blogVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                return View(blogVM);
            }

            if (!blogVM.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Image must be image type");
                return View(blogVM);
            }

            Blog blog = new Blog
            {
                Title = blogVM.Title,
                Description = blogVM.Description,
                Content = blogVM.Content,
                IsFeatured = blogVM.IsFeatured,
                Author = "Admin",
                ImageUrl = blogVM.ImageFile.SaveImage(_env, "uploads/blogs")
            };

            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Blog? blog = await _db.Blogs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blog is null)
                return NotFound();

            blog.IsDeleted = true;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            Blog? blog = await _db.Blogs
                .FirstOrDefaultAsync(x => x.Id == id);

            if (blog is null)
                return NotFound();

            blog.IsDeleted = false;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Blog? blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);

            if (blog is null)
                return NotFound();

            UpdateBlogVM blogVM = new UpdateBlogVM
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                Content = blog.Content,
                ImageUrl = blog.ImageUrl,
                IsFeatured = blog.IsFeatured
            };

            return View(blogVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateBlogVM blogVM)
        {
            if (!ModelState.IsValid)
                return View(blogVM);

            Blog? existedBlog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == blogVM.Id);

            if (existedBlog is null)
                return NotFound();

            if (blogVM.ImageFile is not null)
            {
                if (blogVM.ImageFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Image can not exceed 2MB");
                    return View(blogVM);
                }

                if (!blogVM.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Image must be image type");
                    return View(blogVM);
                }

                existedBlog.ImageUrl.DeleteFile(_env, "uploads/blogs");

                existedBlog.ImageUrl = blogVM.ImageFile.SaveImage(_env, "uploads/blogs");
            }

            existedBlog.Title = blogVM.Title;
            existedBlog.Description = blogVM.Description;
            existedBlog.Content = blogVM.Content;
            existedBlog.IsFeatured = blogVM.IsFeatured;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
