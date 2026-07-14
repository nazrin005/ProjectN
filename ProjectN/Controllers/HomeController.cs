using Microsoft.AspNetCore.Mvc;

namespace ProjectN.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            return View();
         
        }
    }
}
