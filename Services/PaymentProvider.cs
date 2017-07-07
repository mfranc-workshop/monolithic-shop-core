using monolithic_shop_core.Data;
using NLog;

namespace monolithic_shop_core.Services
{
    public interface IPaymentProvider
    {
        bool SendPaymentData(Payment payment);
    }

    public class PaymentProvider : IPaymentProvider
    {
        private ILogger _logger = LogManager.GetCurrentClassLogger();
        // Fake call to external payment provider
        public bool SendPaymentData(Payment payment)
        {
            _logger.Info($"Sending payment info for order - '{payment.OrderId}'");

            if (payment.Card.Number == "-1") return false;

            _logger.Info($"Payment succesfull for order - '{payment.OrderId}'");
            return true;
        }
    }
}
