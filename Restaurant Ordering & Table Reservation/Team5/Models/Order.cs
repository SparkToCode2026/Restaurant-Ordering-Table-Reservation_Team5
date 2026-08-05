
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team5.Models
{
    public class Order
    {
        // Primary Key
        [Key]
        public int Id { get; set; }

        // Order Information

        // DineIn or Takeaway
        public string OrderType { get; set; }

        // Pending, Preparing, Ready
        public string Status { get; set; }         
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }



        //////Relationships



        // One User can have many Orders (User 1 - M Order)
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }


        // One Table can have many Orders (Table 1 - M Order)

        [ForeignKey("Table")]
        public int TableId { get; set; }

        public Table Table { get; set; }


        // One Order can contain many Order Items (Order 1 - M OrderItem)
        public List<OrderItem> OrderItems { get; set; }


        // One Order can have many Payments ( Order 1 - M Payment)
        public List<Payment> Payments { get; set; }


        // One Order can receive many Reviews (Order 1 - M Review)
        public List<Review> Reviews { get; set; }
    }
}
