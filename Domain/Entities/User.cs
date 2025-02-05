using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public UserRole Role { get; set; } = UserRole.Buyer; // Значение по умолчанию
        public Shop? Shop { get; set; } // Магазин пользователя (если продавец)
    }

    
}
