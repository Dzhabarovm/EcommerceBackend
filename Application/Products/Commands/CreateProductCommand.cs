using MediatR;

namespace Application.Products.Commands
{
    public record CreateProductCommand(
    string Name,
    decimal Price,
    string Description,
    int StockQuantity,
    Guid ShopId,
    string ImageUrl // URL изображения (генерируется в обработчике)
) : IRequest<Guid>;
}
