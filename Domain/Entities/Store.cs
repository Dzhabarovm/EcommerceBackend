using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Store
    {
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Url]
        public string LogoUrl { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public List<Product> Products { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
