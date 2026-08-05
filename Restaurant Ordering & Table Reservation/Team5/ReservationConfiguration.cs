using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApi.Models;
using Team5.Models;

namespace RestaurantApi.Data.Configurations;

/// <summary>
/// EF Core Fluent API configuration for Reservation.
/// Written as an IEntityTypeConfiguration so it can be dropped into the
/// shared ApplicationDbContext without editing the same OnModelCreating
/// method other teammates are also working in — just add, in
/// OnModelCreating:  modelBuilder.ApplyConfiguration(new ReservationConfiguration());
/// (or modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly)
/// to pick up every configuration in the project automatically.)
/// </summary>
public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.ReservationId);

        builder.Property(r => r.ReservationDate).IsRequired();
        builder.Property(r => r.ReservationTime).IsRequired();
        builder.Property(r => r.PartySize).IsRequired();
        builder.Property(r => r.Status).IsRequired();
        builder.Property(r => r.CreatedAt).IsRequired();

        // A reservation always belongs to exactly one customer (User).
        // Restrict delete: don't allow deleting a User who still has
        // reservations on the books — force an explicit cleanup/cancel first.
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // A reservation always belongs to exactly one Table.
        // Restrict delete for the same reason — a table with active/future
        // reservations shouldn't be removable without resolving them first.
        builder.HasOne(r => r.Table)
            .WithMany(t => t.Reservations)
            .HasForeignKey(r => r.TableId)
            .OnDelete(DeleteBehavior.Restrict);

        // Speeds up the common "is this table free at this date/time" check
        // and the "show me this table's upcoming reservations" query.
        builder.HasIndex(r => new { r.TableId, r.ReservationDate, r.ReservationTime });
    }
}
