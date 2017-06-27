using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;

namespace monolithic_shop_core.Controllers
{
    [Route("/order")]
    public class OrderController : Controller
    {
        [HttpPut]
        public IActionResult Put([FromBody] ICollection<ProductOrder> productOrders)
        {
            var order = new Order(productOrders);

            using (var context = new MainDatabaseContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }

            return Ok(new { orderId = order.Id });
        }
    }
}
