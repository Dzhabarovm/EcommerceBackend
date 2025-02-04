using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sellers.Commands
{
    // Application/Sellers/Commands/ApproveSellerCommand.cs
    public record ApproveSellerCommand(Guid UserId, string ShopName, string ShopDescription) : IRequest;

    public class ApproveSellerCommandHandler : IRequestHandler<ApproveSellerCommand>
    {
        private readonly EcommerceDbContext _context;

        public async Task Handle(ApproveSellerCommand request, CancellationToken ct)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            user.Role = UserRole.Seller;

            var shop = new Shop
            {
                Name = request.ShopName,
                Description = request.ShopDescription,
                SellerId = request.UserId
            };

            await _context.Shops.AddAsync(shop, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}
