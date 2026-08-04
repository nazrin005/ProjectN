using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;
using ProjectN.ViewModels.Blogs;

namespace ProjectN.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;

        public BlogController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search)
        {
            IQueryable<Blog> query = _db.Blogs
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    x.Description.Contains(search));
            }

            List<Blog> blogs = await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            List<Blog> popularBlogs = await _db.Blogs
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Take(3)
                .ToListAsync();

            ViewBag.Search = search;

            BlogVM vm = new BlogVM
            {
                Blogs = blogs,
                PopularBlogs = popularBlogs
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            Blog? blog = await _db.Blogs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (blog is null)
                return NotFound();

            ViewBag.PopularBlogs = await _db.Blogs
                .Where(x => !x.IsDeleted && x.Id != id)
                .OrderByDescending(x => x.CreatedAt)
                .Take(4)
                .ToListAsync();

            return View(blog);
        }
    }
}
