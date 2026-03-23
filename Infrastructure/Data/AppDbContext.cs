using Microsoft.EntityFrameworkCore;
using PurchaseOrderAPI.Domain.Entities;

namespace PurchaseOrderAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 🔹 Tabelas do banco
        public DbSet<User> Users { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Approval> Approvals { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔥 USER (melhoria principal)
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(u => u.Email)
                    .IsUnique(); // 🔥 garante email único
            });

            // 🔹 Precisão decimal
            modelBuilder.Entity<PurchaseOrder>()
                .Property(p => p.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseOrderItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            // 🔹 Relação PurchaseOrder → User (criador)
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relação Approval → User (quem aprovou)
            modelBuilder.Entity<Approval>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relação OrderHistory → User (quem executou ação)
            modelBuilder.Entity<OrderHistory>()
                .HasOne(h => h.User)
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Relação PurchaseOrder → Itens
            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(i => i.PurchaseOrder)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relação PurchaseOrder → Aprovações
            modelBuilder.Entity<Approval>()
                .HasOne(a => a.PurchaseOrder)
                .WithMany(p => p.Approvals)
                .HasForeignKey(a => a.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Relação PurchaseOrder → Histórico
            modelBuilder.Entity<OrderHistory>()
                .HasOne(h => h.PurchaseOrder)
                .WithMany(p => p.History)
                .HasForeignKey(h => h.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}