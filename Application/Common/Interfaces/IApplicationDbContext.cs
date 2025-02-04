namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Shop> Shops { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
