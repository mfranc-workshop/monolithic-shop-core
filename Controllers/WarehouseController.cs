using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;

namespace monolithic_shop_core.Controllers
{
    [Route("/warehouse")]
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            using (var context = new MainDatabaseContext())
            {
                var products = context.ProductsWarehouse
                    .Include(c => c.Product)
                    .ToList();
                return View(products);
            }
        }

        [HttpPost]
        [Route("{id}")]
        public IActionResult Update([FromForm] int numberAvailable, int id)
        {
            using (var context = new MainDatabaseContext())
            {
                var productWarehouse = context.ProductsWarehouse
                    .SingleOrDefault(x => x.Product.Id == id);

                productWarehouse.NumberAvailable = numberAvailable;

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}