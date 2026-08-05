
using System.ComponentModel.DataAnnotations.Schema;

namespace Team5.Models
{
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }

        //Relationships
        [ForeignKey("Category")]
        public int MenuCategoryId { get; set; }
        public MenuCategory Category { get; set; }
    }
}
