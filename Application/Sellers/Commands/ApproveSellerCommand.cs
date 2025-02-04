using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Sellers.Commands
{
    // Application/Sellers/Commands/ApproveSellerCommand.cs
    public record ApproveSellerCommand(Guid UserId, string ShopName, string ShopDescription) : IRequest;

    public class ApproveSellerCommandHandler : IRequestHandler<ApproveSellerCommand>
    {
        private readonly IApplicationDbContext _context;

        public ApproveSellerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ApproveSellerCommand request, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
                throw new InvalidOperationException("Пользователь не найден");

            user.Role = UserRole.Seller;

            var shop = new Shop
            {
                Id = Guid.NewGuid(),
                Name = request.ShopName,
                Description = request.ShopDescription,
                SellerId = request.UserId
            };

            await _context.Shops.AddAsync(shop, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}
