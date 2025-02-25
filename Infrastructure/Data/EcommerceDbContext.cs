using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class EcommerceDbContext : DbContext, IApplicationDbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Store> Stores { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------
            // User -> Store (1:M)
            // -------------------------
            modelBuilder.Entity<Store>()
                .HasOne(store => store.Owner)
                .WithMany(user => user.Stores)
                .HasForeignKey(store => store.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);  // Удаление пользователя удаляет магазины

            // -------------------------
            // Store -> Product (1:M)
            // -------------------------
            modelBuilder.Entity<Product>()
                .HasOne(product => product.Store)
                .WithMany(store => store.Products)
                .HasForeignKey(product => product.StoreId)
                .OnDelete(DeleteBehavior.Cascade);  // Удаление магазина удаляет товары

            // -------------------------
            // User -> Order (1:M)
            // -------------------------
            modelBuilder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);  // Предотвращаем удаление заказов при удалении пользователя

            // -------------------------
            // Order -> OrderItem (1:M)
            // -------------------------
            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade);  // Удаление заказа удаляет связанные позиции

            // -------------------------
            // OrderItem -> Product (M:1)
            // -------------------------
            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Product)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ProductId)
                .OnDelete(DeleteBehavior.Restrict);  // Предотвращаем удаление товаров при наличии заказов

            // -------------------------
            // Уникальные индексы и ограничения
            // -------------------------
            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);  // Прецизионное хранение цены

            // -------------------------
            // Значения по умолчанию
            // -------------------------
            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Store>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Product>()
                .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Order>()
                .Property(o => o.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
