
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace Team5.Models
{
    public class Ingredient
    {
        [Key]
        [JsonIgnore]
        public int IngredientId { get; set; }

        [Required]
        public string IngredientName { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }

        [Required]
        public decimal QuantityInStock { get; set; }

        [Required]
        public decimal ReorderLevel { get; set; }

        //Relationships 

        //One Ingredient can be used in many MenuItemIngredients 
        // (Ingredient 1 - M MenuItemIngredient)
        [JsonIgnore]
        public List<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}
    
