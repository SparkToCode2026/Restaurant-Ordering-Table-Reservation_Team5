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
        [StringLength(50, MinimumLength = 2)]
        public string MenuCategoryName { get; set; }

        [Required]
        [StringLength(200)]
        public string MenuCategoryDescription { get; set; }

        [Required]
        public int DisplayOrder { get; set; }

        //Relationships
        [JsonIgnore]
        public List<MenuItem> MenuItems { get; set; }
    }
}
