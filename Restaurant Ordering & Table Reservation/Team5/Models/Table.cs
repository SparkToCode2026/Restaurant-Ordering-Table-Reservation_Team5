using RestaurantApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class Table
    {
        // Primary Key
        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        // Table Information
        [Required]
        public string TableNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public bool IsActive { get; set; }


        ///////Relationships


        // One Table can have many Reservations ( Table 1 - M Reservation)
        [JsonIgnore]
        public List<Reservation> Reservations { get; set; }


        // One Table can have many Orders (Table 1 - M Order)
        [JsonIgnore]
        public List<Order> Orders { get; set; }
    }
}
