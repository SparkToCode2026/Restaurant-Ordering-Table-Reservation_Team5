using RestaurantApi.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Team5.Models
{
    public class Table
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Table Information
        public string TableNumber { get; set; }

        public int Capacity { get; set; }

        public string Location { get; set; }

        public bool IsActive { get; set; }


        ///////Relationships


        // One Table can have many Reservations ( Table 1 - M Reservation)
        public List<Reservation> Reservations { get; set; }


        // One Table can have many Orders (Table 1 - M Order)
        public List<Order> Orders { get; set; }
    }
}
