using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ShopConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasOne(s => s.Owner)
                .WithOne(u => u.Stores)
                .HasForeignKey<Store>(s => s.SellerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
