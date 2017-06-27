namespace monolithic_shop_core.Data
{
    public class ProductWarehouse
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int NumberAvailable { get; set; }

        public ProductWarehouse()
        {
            
        }

        public ProductWarehouse(Product product, int number)
        {
            Product = product;
            NumberAvailable = number;
        }
    }
}