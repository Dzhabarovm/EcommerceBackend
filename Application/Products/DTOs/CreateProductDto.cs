namespace Application.Products.DTOs
{
    public record CreateProductDto(
    string Name,
    decimal Price,
    Guid ShopId // Добавляем идентификатор магазина
);
}
