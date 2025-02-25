using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(15)]
        [Required]
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; } = UserRole.Buyer; // Значение по умолчанию
        public List<Store> Stores { get; set; } = new(); // Магазины пользователя (если продавец)
        public List<Order> Orders { get; set; } = new(); // Заказы пользователя
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }

    
}
