using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Team5.Models;

namespace Team5.Models 
{ 
    public class Reservation
    {
        [Key]
        [JsonIgnore]
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
        [JsonIgnore]
        public User User { get; set; } 
        
        // One Table can have many Reservations (Table 1 - M Reservation)
        [ForeignKey("Table")] 
        public int TableId { get; set; }
        [JsonIgnore]
        public Table Table { get; set; }
    }
}
﻿
