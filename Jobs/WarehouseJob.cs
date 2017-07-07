using Microsoft.EntityFrameworkCore;
using System.Linq;
using monolithic_shop_core.Data;
using monolithic_shop_core.EmailHelpers;
using monolithic_shop_core.Services;
using Quartz;
using System;
using System.Threading.Tasks;
using NLog;

namespace monolithic_shop_core.Jobs
{
    public class WarehouseJob : IJob
    {
        private readonly IEmailService _emailService;
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public WarehouseJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Execute(IJobExecutionContext jobContext)
        {
            _logger.Info("Starting warehouse job check");

            using (var context = new MainDatabaseContext())
            {
                _logger.Info("Checking number of orders waiting for warehouse");

                var orders = context.Orders
                    .Where(x => x.Status == OrderStatus.WaitingForWarehouse)
                    .Include(o => o.Buyer)
                    .Include(o => o.ProductOrders).ThenInclude(p => p.Product)
                    .ToList();

                _logger.Info($"Found - {orders.Count} orders waiting for warehouse");

                foreach (var order in orders)
                {
                    _logger.Info($"Checking if order '{order.Id}' can be fulfiled");
                    var cannotFulfill = (from productOrder in order.ProductOrders
                        let productWarehouse = context.ProductsWarehouse.FirstOrDefault(pw => pw.Product.Id == productOrder.ProductId)
                        where productWarehouse.NumberAvailable < productOrder.Count
                        select productOrder).Any();

                    if (cannotFulfill)
                    {
                        _logger.Info($"Order '{order.Id}' cannot be fulfilled");
                        break;
                    }
                    else
                    {
                        foreach (var productOrder in order.ProductOrders)
                        {
                            _logger.Info($"Taking {productOrder.Count} of {productOrder.Product.Name} out of warehouse");
                            var productW = context.ProductsWarehouse
                                .Include(pw => pw.Product)
                                .FirstOrDefault(x => x.Product.Id == productOrder.ProductId);

                            productW.NumberAvailable = productW.NumberAvailable - productOrder.Count;

                            _logger.Info($"{productW.NumberAvailable} of {productOrder.Product.Name} left in warehouse");
                        }

                        _emailService.SendEmail(order.Buyer.Email, EmailType.OrderSend);
                        order.Delivering();
                    }
                }

                context.SaveChanges();

                return Task.FromResult(true);
            }
        }
    }
}