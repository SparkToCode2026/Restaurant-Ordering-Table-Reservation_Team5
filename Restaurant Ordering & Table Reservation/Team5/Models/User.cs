using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Team5.Models
{
    public class User
    {
        [Key]
        [JsonIgnore]
        public int  UserId { get; set; }
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }



        //Relationships 

        //One User can have many Reservations (User 1 - M Reservation)
        [JsonIgnore]
        public List<Reservation> Reservations { get; set; }

        // One User can have many Orders (User 1 - M Order)
        [JsonIgnore]
        public List<Order> Orders { get; set; }

        // One User can have many Reviews (User 1 - M Review)
        [JsonIgnore]
        public List<Review> Reviews { get; set; } 
        
        



    }
}

           

       
   

        
    

