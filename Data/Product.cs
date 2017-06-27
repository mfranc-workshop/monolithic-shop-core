namespace monolithic_shop_core.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product()
        {
        }

        public Product(decimal price, string name, int id)
        {
            Id = id;
            Price = price;
            Name = name;
        }
    }
}