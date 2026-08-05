using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Team5.Models
{
    public class User
    {
        [Key]
        public int  UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }



        //Relationships 
        
        //One User can have many Reservations (User 1 - M Reservation)
        public List<Reservation> Reservations { get; set; } 
        
        // One User can have many Orders (User 1 - M Order)
        public List<Order> Orders { get; set; } 
        
        // One User can have many Reviews (User 1 - M Review)
        public List<Review> Reviews { get; set; } 
        
        


    }
}


