using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class MenuCategory
    {
        [Key]
        [JsonIgnore]
        public int MenuCategoryId { get; set; }
        public string MenuCategoryName { get; set; }
        public string MenuCategoryDescription { get; set; }
        public int DisplayOrder { get; set; }

        //Relationships
        [JsonIgnore]
        public List<MenuItem> MenuItems { get; set; }
    }
}
