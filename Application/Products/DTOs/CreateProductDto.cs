using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.DTOs
{
    public record CreateProductDto(
    string Name,
    decimal Price,
    Guid ShopId // Добавляем идентификатор магазина
);
}
