using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;

namespace monolithic_shop_core.Controllers
{
    [Authorize]
    public class CheckoutController: Controller
    {
        [HttpGet]
        [Route("/checkout/{orderId}")]
        public IActionResult Index(Guid orderId)
        {
            using (var context = new MainDatabaseContext())
            {
                var order = context.Orders
                    .Where(x => x.Id == orderId)
                    .Include(p => p.ProductOrders).ThenInclude(po => po.Product)
                    .FirstOrDefault();

                return View(order);
            }
        }
    }
}