using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        [MaxLength(150)]
        public string Name { get; set; } = string.Empty; // Защита от null
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Цена должна быть неотрицательной.")]
        public decimal Price { get; set; }

        public Guid StoreId { get; set; }
        public Store Store { get; set; }

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
