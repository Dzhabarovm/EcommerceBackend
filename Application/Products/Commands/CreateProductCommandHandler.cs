using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var shopExists = await _context.Shops.AnyAsync(s => s.Id == request.ShopId, cancellationToken);
            if (!shopExists)
                throw new InvalidOperationException("Магазин не найден");

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
                throw new InvalidOperationException("Категория не найдена");

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ShopId = request.ShopId,
                Images = new List<string> { request.ImageUrl },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
