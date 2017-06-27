using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using monolithic_shop_core.Data;

namespace monolithic_shop_core.Services
{
    public interface IReportGenerator
    {
        Report GenerateReport();
    }

    public class ReportGenerator : IReportGenerator
    {
        public Report GenerateReport()
        {
            var report = new Report();

            using (var ctx = new MainDatabaseContext())
            {
                var orders = ctx.Orders
                    .Include(x => x.Buyer)
                    .Include(x => x.Payment)
                    .Include(x => x.ProductOrders.Select(po => po.Product))
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

            return report;
        }
    }
}
