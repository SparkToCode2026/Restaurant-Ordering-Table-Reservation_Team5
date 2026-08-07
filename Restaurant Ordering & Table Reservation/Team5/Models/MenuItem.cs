
using RestaurantApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class MenuItem
    {
        [Key]
        [JsonIgnore]
        public int MenuItemId { get; set; }

        [Required]
        public string MenuItemName { get; set; }

        [Required]
        public string MenuItemDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        //Relationships

        //One Menu Category can have many Menu Items (MenuCategory 1 - M MenuItem)
        [ForeignKey("Category")]
        public int MenuCategoryId { get; set; }
        [JsonIgnore]
        public MenuCategory Category { get; set; }

        // One Menu Item can appear in many Order Items (MenuItem 1 - M OrderItem)
        [JsonIgnore]
        public List<OrderItem> OrderItems { get; set; }

        // One Menu Item can have many MenuItemIngredients 
        // (MenuItem 1 - M MenuItemIngredient)
        [JsonIgnore]
        public List<MenuItemIngredient> MenuItemIngredients { get; set; }

        // One Menu Item can receive many Reviews (MenuItem 1 - M Review)
        [JsonIgnore]
        public List<Review> Reviews { get; set; }
    }
}
