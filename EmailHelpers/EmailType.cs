namespace monolithic_shop_core.EmailHelpers
{
    public enum EmailType
    {
        PaymentAccepted,
        PaymentRefused,
        OrderSend,
        OrderReceived,
        OrderDelayed,
        TransferReceived,
        WaitingForTransfer
    }
}