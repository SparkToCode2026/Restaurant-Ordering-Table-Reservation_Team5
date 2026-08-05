using System.ComponentModel.DataAnnotations;

namespace Team5.Models
{
    public class MenuCategory
    {
        [Key]
        public int MenuCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        //Relationships
        public List<MenuItem> MenuItems { get; set; }
    }
}
