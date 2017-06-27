using Microsoft.EntityFrameworkCore;
using System.Linq;
using monolithic_shop_core.Data;
using monolithic_shop_core.EmailHelpers;
using monolithic_shop_core.Services;
using Quartz;
using System.Threading.Tasks;

namespace monolithic_shop_core.Jobs
{
    public class CheckTransferJob : IJob
    {
        private readonly IEmailService _emailService;
        private readonly ITransferCheckService _transferCheckService;

        public CheckTransferJob(IEmailService emailService, ITransferCheckService transferCheckService)
        {
            _emailService = emailService;
            _transferCheckService = transferCheckService;
        }

        public Task Execute(IJobExecutionContext jobContext)
        {
            using (var context = new MainDatabaseContext())
            {
                var orders = context.Orders
                    .Where(x => x.Status == OrderStatus.WaitingForPayment)
                    .Include(o => o.Buyer)
                    .ToList();

                foreach (var order in orders)
                {
                    var hasReceivedMoney = _transferCheckService.Check(order.Id);
                    if(hasReceivedMoney)
                    {
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