using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team5.Models 
{ 

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
        [Key]
        public int ReservationId { get; set; }

        public DateOnly ReservationDate { get; set; }
        public TimeOnly ReservationTime { get; set; }
        public int PartySize { get; set; }

        // Lifecycle: Pending, Confirmed, Completed, Cancelled at any point
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        //Relationships 
        
        // One User can have many Reservations (User 1 - M Reservation)
        [ForeignKey("User")] 
        public int UserId { get; set; } 
        public User User { get; set; } 
        
        // One Table can have many Reservations (Table 1 - M Reservation)
        [ForeignKey("Table")] 
        public int TableId { get; set; } 
        public Table Table { get; set; }
    }
}
﻿
