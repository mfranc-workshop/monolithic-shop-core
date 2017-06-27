using System;
using System.Collections.Generic;

namespace monolithic_shop_core.EmailHelpers
{
    public static class Email
    {
        public static Dictionary<EmailType, Tuple<string, string>> Templates = new Dictionary
            <EmailType, Tuple<string, string>>
            {
                { EmailType.PaymentAccepted, new Tuple<string, string>("Payment Accepted", "Thank you, your payment has been accepted. Order should be sent soon.") },
                { EmailType.PaymentRefused, new Tuple<string, string>("Payment Refused", "Sadly we couldnt accept this type of payment.") },
                { EmailType.OrderSend, new Tuple<string, string>("Your order has been sent", "Your ourder has been sent.") },
                { EmailType.OrderDelayed, new Tuple<string, string>("Your order has been delayed", "Your ourder has been delayed.") },
                { EmailType.OrderReceived, new Tuple<string, string>("Your order has been delivered", "Your ourder has been delivered.") },
                { EmailType.TransferReceived, new Tuple<string, string>("We have received the money", "Your ourder will be sent.") },
                { EmailType.WaitingForTransfer, new Tuple<string, string>("We are waiting for your money", "Your order will be send after moeny is received.") },
            };
    }
}
