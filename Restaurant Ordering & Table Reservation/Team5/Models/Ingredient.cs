
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

        public string IngredientName { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal QuantityInStock { get; set; }

        public decimal ReorderLevel { get; set; }

        //Relationships 

        //One Ingredient can be used in many MenuItemIngredients 
        // (Ingredient 1 - M MenuItemIngredient)
        [JsonIgnore]
        public List<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}
    
