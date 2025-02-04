namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty; // Защита от null
        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public List<string> Images { get; set; } = new();

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.Available; // Значение по умолчанию

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum ProductStatus
    {
        Available,  // В наличии
        OutOfStock,  // Нет в наличии
        ComingSoon  // Скоро в поступит
    }
}
