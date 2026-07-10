using Microsoft.AspNetCore.Mvc;

namespace ProjectN.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
