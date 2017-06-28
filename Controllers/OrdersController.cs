using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;
using monolithic_shop_core.Helpers;

namespace monolithic_shop_core.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        [Route("/orders")]
        public IActionResult Index()
        {
            using (var context = new MainDatabaseContext())
            {
                var email = User.GetEmail();

                var orders = context.Orders
                    .Where(x => x.Buyer.Email == email)
                    .Include(x => x.Buyer)
                    .Include(x => x.Payment)
                    .Include(x => x.ProductOrders).ThenInclude(po => po.Product)
                    .ToList();

                return View(orders);
            }
        }
    }
}
