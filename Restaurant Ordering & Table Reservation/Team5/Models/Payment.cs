using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team5.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionRef { get; set; }

        //Relationships

        //One Order can have many Payments (Order 1 - M Payment)
        [ForeignKey("Order")] 
        public int OrderId { get; set; } 
        public Order Order { get; set; }

    }
}
