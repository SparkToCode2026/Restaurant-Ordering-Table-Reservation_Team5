using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApi.Models;

namespace RestaurantApi.Data.Configurations;

/// <summary>
/// EF Core Fluent API configuration for OrderItem.
/// Written as an IEntityTypeConfiguration so it can be dropped into the
/// shared ApplicationDbContext without editing the same OnModelCreating
/// method other teammates are also working in — just add, in
/// OnModelCreating:  modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
/// </summary>
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(oi => oi.OrderItemId);

        builder.Property(oi => oi.Quantity).IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(oi => oi.Subtotal)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        // A line item always belongs to exactly one Order.
        // Cascade delete: deleting an order deletes its line items with it —
        // an order line has no meaning outside its parent order.
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // A line item always references exactly one MenuItem.
        // Restrict delete: a menu item that has ever been ordered can't be
        // hard-deleted (it would corrupt order history) — it should be
        // deactivated (IsAvailable = false) instead.
        builder.HasOne(oi => oi.MenuItem)
            .WithMany(mi => mi.OrderItems)
            .HasForeignKey(oi => oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Speeds up "all line items for this order" (order detail view)
        // and "how many times has this menu item been ordered" (reporting).
        builder.HasIndex(oi => oi.OrderId);
        builder.HasIndex(oi => oi.MenuItemId);
    }
}
