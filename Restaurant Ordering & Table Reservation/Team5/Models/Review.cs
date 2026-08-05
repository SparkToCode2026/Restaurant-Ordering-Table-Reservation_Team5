
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team5.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        //Relationships

        // One User can write many Reviews (User 1 - M Review)
        [ForeignKey("User")] 
        public int? UserId { get; set; } 
        public User? User { get; set; }

        // One Order can receive many Reviews (Order 1 - M Review)
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        // One Menu Item can receive many Reviews (MenuItem 1 - M Review)
        [ForeignKey("MenuItem")]
        public int? MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

    }
}
