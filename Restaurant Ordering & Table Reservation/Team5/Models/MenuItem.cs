
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RestaurantApi.Models;

namespace Team5.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }

        //Relationships

        //One Menu Category can have many Menu Items (MenuCategory 1 - M MenuItem)
        [ForeignKey("Category")]
        public int MenuCategoryId { get; set; }
        public MenuCategory Category { get; set; }

        // One Menu Item can appear in many Order Items (MenuItem 1 - M OrderItem)
        public List<OrderItem> OrderItems { get; set; }

        // One Menu Item can have many MenuItemIngredients 
        // (MenuItem 1 - M MenuItemIngredient)
        public List<MenuItemIngredient> MenuItemIngredients { get; set; } 
        
        // One Menu Item can receive many Reviews (MenuItem 1 - M Review)
        public List<Review> Reviews { get; set; }
    }
}
