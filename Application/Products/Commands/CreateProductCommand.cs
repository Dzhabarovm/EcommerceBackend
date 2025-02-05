using Microsoft.AspNetCore.Http;
using MediatR;

namespace Application.Products.Commands
{
    public record CreateProductCommand(
    string Name,
    decimal Price,
    string Description,
    int StockQuantity,
    Guid ShopId,
    Guid CategoryId,
    IFormFile Image // URL изображения (генерируется в обработчике)
) : IRequest<Guid>;
}
