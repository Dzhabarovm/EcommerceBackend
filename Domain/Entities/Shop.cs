namespace Domain.Entities
{
    public class Shop
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoUrl { get; set; }
        public Guid SellerId { get; set; }
        public User Seller { get; set; }
        public List<Product> Products { get; set; } = new();
    }
}
