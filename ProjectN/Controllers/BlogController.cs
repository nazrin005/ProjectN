using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectN.DAL;
using ProjectN.Models;

namespace ProjectN.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;

        public BlogController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _db.Blogs
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Details(int id)
        {
            Blog? blog = await _db.Blogs
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (blog is null)
                return NotFound();

            return View(blog);
        }
    }
}
