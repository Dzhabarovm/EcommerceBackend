using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly IImageService _imageService;

        public CreateProductCommandHandler(IApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var shopExists = await _context.Shops.AnyAsync(s => s.Id == request.ShopId, cancellationToken);
            if (!shopExists)
                throw new InvalidOperationException("Магазин не найден");

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);
            if (!categoryExists)
                throw new InvalidOperationException("Категория не найдена");

            string imageUrl = await _imageService.SaveImageAsync(request.Image);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ShopId = request.ShopId,
                Images = new List<string> { imageUrl },
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
