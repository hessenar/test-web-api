using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Services
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasIdentityOptions(null, null, 0L, null, null, null)
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Processed)
                    .HasColumnName("processed");

                entity.Property(e => e.ConvertedOrder).HasColumnName("converted_order");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("date");

                entity.Property(e => e.OrderNumber).HasColumnName("order_number");

                entity.Property(e => e.SourceOrder).HasColumnName("source_order");

                entity.Property(e => e.SystemType).HasColumnName("system_type");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}