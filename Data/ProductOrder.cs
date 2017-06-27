namespace monolithic_shop_core.Data
{
    public class ProductOrder
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
    }
}