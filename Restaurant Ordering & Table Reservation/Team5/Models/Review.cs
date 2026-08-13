using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        //Relationships

        // One User can write many Reviews (User 1 - M Review)
        [ForeignKey("User")]
        public int? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

        // One Order can receive many Reviews (Order 1 - M Review)
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }

        // One Menu Item can receive many Reviews (MenuItem 1 - M Review)
        [ForeignKey("MenuItem")]
        public int? MenuItemId { get; set; }
        [JsonIgnore]
        public MenuItem? MenuItem { get; set; }

    }
}