using Microsoft.EntityFrameworkCore;
using System.Linq;
using monolithic_shop_core.Data;
using monolithic_shop_core.EmailHelpers;
using monolithic_shop_core.Services;
using Quartz;
using System.Threading.Tasks;
using NLog;

namespace monolithic_shop_core.Jobs
{
    public class CheckTransferJob : IJob
    {
        private readonly IEmailService _emailService;
        private readonly ITransferCheckService _transferCheckService;
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public CheckTransferJob(IEmailService emailService, ITransferCheckService transferCheckService)
        {
            _emailService = emailService;
            _transferCheckService = transferCheckService;
        }

        public Task Execute(IJobExecutionContext jobContext)
        {
            _logger.Info("Starting check transfer job");
            using (var context = new MainDatabaseContext())
            {
                var orders = context.Orders
                    .Where(x => x.Status == OrderStatus.WaitingForPayment)
                    .Include(o => o.Buyer)
                    .ToList();

                _logger.Info($"Found - '{orders.Count}' orders awaiting payments");

                foreach (var order in orders)
                {
                    _logger.Info($"Checking transfer status for - '{order.Id}'");
                    var hasReceivedMoney = _transferCheckService.Check(order.Id);
                    if(hasReceivedMoney)
                    {
                        _logger.Info($"Received transfer for - '{order.Id}'");
                        order.TransferReceived();
                        _emailService.SendEmail(order.Buyer.Email, EmailType.TransferReceived);
                    }
                }

                context.SaveChanges();
                return Task.FromResult(true);
            }
        }
    }
}