using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int PaymentMethod { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public string TransactionRef { get; set; }

        //Relationships

        //One Order can have many Payments (Order 1 - M Payment)
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }

    }
}