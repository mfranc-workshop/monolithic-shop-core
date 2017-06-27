using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;
using monolithic_shop_core.EmailHelpers;
using monolithic_shop_core.Services;

namespace monolithic_shop_core.Controllers
{
    [Route("api")]
    public class OrderApi : Controller
    {
        private readonly IEmailService _emailService;

        public OrderApi(IEmailService emailService)
        {
            _emailService = emailService;
        }

        // Get here for ease of use, normally state change shouldnt be modified by Get
        [HttpGet]
        [Route("order/{guid}/delivered")]
        public string OrderDelivered(Guid guid)
        {
            using (var context = new MainDatabaseContext())
            {
                var order = context.Orders
                    .Where(x => x.Id == guid)
                    .Include(x => x.Buyer)
                    .FirstOrDefault();

                if (order == null) return "order not found";
                if (order.Status != OrderStatus.Delivering) return "invalid order status";

                order.Delivered();

                _emailService.SendEmail(order.Buyer.Email, EmailType.OrderReceived);

                context.SaveChanges();
            }

            return "order delivered";
        }
    }
}
