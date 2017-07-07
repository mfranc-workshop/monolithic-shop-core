using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using monolithic_shop_core.Data;
using NLog;

namespace monolithic_shop_core.Services
{
    public interface IReportGenerator
    {
        Report GenerateReport();
    }

    public class ReportGenerator : IReportGenerator
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public Report GenerateReport()
        {
            var report = new Report();
            _logger.Info("Generating report");

            using (var ctx = new MainDatabaseContext())
            {
                var orders = ctx.Orders
                    .Include(x => x.Buyer)
                    .Include(x => x.Payment)
                    .Include(x => x.ProductOrders).ThenInclude(po => po.Product)
                    .ToList();

                var pendingOrders = orders.Count(x => x.Status == OrderStatus.Delivering);
                report.PendingOrders = pendingOrders;

                var deliveredOrders = orders.Count(x => x.Status == OrderStatus.Finished);
                report.DeliveredOrders = deliveredOrders;

                var moneySold = orders.Sum(x => x.Price);
                report.MoneySold = moneySold;

                var numberSold = orders.Sum(x => x.ProductOrders.Sum(po => po.Count));
                report.NumberSold = numberSold;
            }

                //Magic :D
            Thread.Sleep(4000);

            _logger.Info("Report generated");
            return report;
        }
    }
}
