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
    }
}
