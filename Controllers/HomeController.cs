using Microsoft.AspNetCore.Mvc;

namespace monolithic_shop_core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
