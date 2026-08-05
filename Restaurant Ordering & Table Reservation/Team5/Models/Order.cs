
using System.ComponentModel.DataAnnotations;

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
    }
}
