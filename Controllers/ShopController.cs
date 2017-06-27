using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;

namespace monolithic_shop_core.Controllers
{
    [Authorize]
    public class ShopController: Controller
    {
        public IActionResult Index()
        {
            using (var context = new MainDatabaseContext())
            {
                var products = context.Products.ToList();
                return View(products);
            }
        }
    }
}
