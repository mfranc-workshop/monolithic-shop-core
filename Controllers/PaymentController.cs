using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using monolithic_shop_core.Data;
using monolithic_shop_core.EmailHelpers;
using monolithic_shop_core.Helpers;
using monolithic_shop_core.Services;
using RawRabbit;
using Newtonsoft.Json;

namespace monolithic_shop_core.Controllers
{
    public class OrderAwaitingTransfer
    {
        public Guid Id { get; set; }
    }

    public class PaymentController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IPaymentProvider _paymentProvider;
        private IBusClient _client;

        public PaymentController(IEmailService emailService, IPaymentProvider paymentProvider, IBusClient client)
        {
            _emailService = emailService;
            _paymentProvider = paymentProvider;
            _client = client;
        }

        [HttpGet]
        [Route("/pay/{orderId}")]
        public IActionResult Pay(Guid orderId)
        {
            return View(orderId);
        }

        [HttpPost]
        [Route("/pay/create/")]
        public IActionResult CreatePayment(Payment payment)
        {
            var email = User.GetEmail();

            var successfullPayment = false;

            using (var context = new MainDatabaseContext())
            {
                var order = context.Orders
                    .Where(x => x.Id == payment.OrderId)
                    .Include(p => p.ProductOrders).ThenInclude(po => po.Product)
                    .Include(p => p.Buyer)
                    .FirstOrDefault();

                if (order == null) return View("Error");

                order.AddBuyer(GetOrCreateNewBuyer(context, email));

                if(payment.Type == PaymentType.Card)
                {
                    successfullPayment = _paymentProvider.SendPaymentData(payment);
                    order.PayByCard(payment, successfullPayment);
                    _emailService.SendEmail(email, successfullPayment ? EmailType.PaymentAccepted : EmailType.PaymentRefused);
                    context.SaveChanges();
                    return successfullPayment ? View("Success") : View("Failure");
                }
                else
                {
                    _emailService.SendEmail(email, EmailType.WaitingForTransfer); 
                    order.PayByTransfer(payment);

                    var jsonData = JsonConvert.SerializeObject(new OrderAwaitingTransfer { Id = order.Id });

                    _client.PublishAsync(jsonData, default(Guid),
                        cfg => cfg.WithExchange(ex => ex.WithName("order_exchange")).WithRoutingKey("order_awaiting_transfer"));
                    context.SaveChanges();
                    return View("Success");
                }

            }
        }

        private Buyer GetOrCreateNewBuyer(MainDatabaseContext context, string email)
        {
            return context.Buyers.FirstOrDefault(x => x.Email == email)
                                ?? new Buyer(User.Identity.Name, User.GetEmail());
        }
    }
}
