using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Team5.Models
{
    public class MenuItemIngredient
    {
        [Key]
        [JsonIgnore]
        public int MenuItemIngredientId { get; set; }

        public decimal QuantityRequired { get; set; }

        //Relationships 
        
        //One Menu Item can have many MenuItemIngredients 
        //(MenuItem 1 - M MenuItemIngredient)
        
        [ForeignKey("MenuItem")] 
        public int MenuItemId { get; set; }
        [JsonIgnore]
        public MenuItem MenuItem { get; set; } 
        
        //One Ingredient can be used in many MenuItemIngredients 
        //(Ingredient 1 - M MenuItemIngredient)
        
        [ForeignKey("Ingredient")] 
        public int IngredientId { get; set; }
        [JsonIgnore]
        public Ingredient Ingredient { get; set; }
    }
}
