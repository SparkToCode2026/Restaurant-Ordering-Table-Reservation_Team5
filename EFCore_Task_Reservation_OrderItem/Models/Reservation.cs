namespace RestaurantApi.Models;

/// <summary>
/// Represents a customer's booking of a table for a given date/time and party size.
/// Assigned model — EF Core Code task (Reservation).
///
/// Relationships:
///   - Many-to-one with User      (the customer who made the booking)
///   - Many-to-one with Table     (the table being reserved)
/// Both FKs are required (non-nullable) because a reservation cannot exist
/// without a customer and a table.
/// </summary>
public class Reservation
{
    public int Id { get; set; }

    // ---- FK: User (customer) ----
    public int UserId { get; set; }
    public User? User { get; set; }

    // ---- FK: Table ----
    public int TableId { get; set; }
    public Table? Table { get; set; }

    public DateOnly ReservationDate { get; set; }
    public TimeOnly ReservationTime { get; set; }
    public int PartySize { get; set; }

    // Lifecycle: Pending -> Confirmed -> Completed, or -> Cancelled at any point.
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
