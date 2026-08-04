
using Microsoft.EntityFrameworkCore;
namespace Team5.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        public string Name { get; set; }

        public string UnitOfMeasure { get; set; }

        public decimal QuantityInStock { get; set; }

        public decimal ReorderLevel { get; set; }
    }
}
    
