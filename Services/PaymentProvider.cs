using monolithic_shop_core.Data;

namespace monolithic_shop_core.Services
{
    public interface IPaymentProvider
    {
        bool SendPaymentData(Payment payment);
    }

    public class PaymentProvider : IPaymentProvider
    {
        // Fake call to external payment provider
        public bool SendPaymentData(Payment payment)
        {
            if (payment.Card.Number == "-1") return false;

            return true;
        }
    }
}
