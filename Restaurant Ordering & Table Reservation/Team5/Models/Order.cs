
using RestaurantApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class Order
    {
        // Primary Key
        [Key]
        [JsonIgnore]
        public int OrderId { get; set; }

        // Order Information

        // DineIn or Takeaway

        [Required]
        public string OrderType { get; set; }

        // Pending, Preparing, Ready

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }



        //Relationships



        // One User can have many Orders (User 1 - M Order)
        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }


        // One Table can have many Orders (Table 1 - M Order)

        [ForeignKey("Table")]
        public int TableId { get; set; }
        [JsonIgnore]
        public Table Table { get; set; }


        // One Order can contain many Order Items (Order 1 - M OrderItem)
        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }


        // One Order can have many Payments ( Order 1 - M Payment)
        [JsonIgnore]
        public List<Payment> Payments { get; set; }


        // One Order can receive many Reviews (Order 1 - M Review)
        [JsonIgnore]
        public List<Review> Reviews { get; set; }
    }
}
