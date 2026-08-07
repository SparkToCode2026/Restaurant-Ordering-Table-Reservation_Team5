using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class MenuCategory
    {
        [Key]
        [JsonIgnore]
        public int MenuCategoryId { get; set; }

        [Required]
        public string MenuCategoryName { get; set; }

        [Required]
        public string MenuCategoryDescription { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        //Relationships
        [JsonIgnore]
        public List<MenuItem> MenuItems { get; set; }
    }
}
