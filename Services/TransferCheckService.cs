using System;
using NLog;

namespace monolithic_shop_core.Services
{
    public interface ITransferCheckService
    {
        bool Check(Guid orderId);
    }

    public class TransferCheckService : ITransferCheckService
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();

        public bool Check(Guid orderId)
        {

            _logger.Info($"Checking transfer for order - {orderId}");

            var rand = new Random();

            var transferReceived = rand.NextDouble() <= 0.8;

            _logger.Info($"Transfer check performed - '{transferReceived}' for order id '{orderId}'");

            return transferReceived;
        }
    }
}
