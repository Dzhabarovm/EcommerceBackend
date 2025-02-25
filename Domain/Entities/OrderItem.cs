using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть минимум 1.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Цена должна быть неотрицательной.")]
        public decimal PricePerUnit { get; set; }
    }
}