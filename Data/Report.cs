namespace monolithic_shop_core.Data
{
    public class Report
    {
        public int PendingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int NumberSold { get; set; }
        public decimal MoneySold { get; set; }
    }
}
