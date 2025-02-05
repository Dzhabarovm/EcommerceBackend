using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasOne(s => s.Seller)
                .WithOne(u => u.Shop)
                .HasForeignKey<Shop>(s => s.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
