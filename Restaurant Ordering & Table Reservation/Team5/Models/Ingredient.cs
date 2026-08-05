
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Team5.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal QuantityInStock { get; set; }

        public decimal ReorderLevel { get; set; }

        //Relationships 
        
        //One Ingredient can be used in many MenuItemIngredients 
        // (Ingredient 1 - M MenuItemIngredient)
        public List<MenuItemIngredient> MenuItemIngredients { get; set; }
    }
}
    
