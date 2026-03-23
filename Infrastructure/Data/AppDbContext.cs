using Microsoft.EntityFrameworkCore;
using PurchaseOrderAPI.Domain.Entities;

namespace PurchaseOrderAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 🔹 Tabelas do banco
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchaseOrder>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 2);
        }
    }
}