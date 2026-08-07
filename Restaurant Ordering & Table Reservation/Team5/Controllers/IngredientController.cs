using Microsoft.AspNetCore.Mvc;
using Team5.Models;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly ProjectContext _context;

        public IngredientController(ProjectContext context)
        {
            _context = context;
        }

        // Get All Ingredients
        [HttpGet]
        public IActionResult GetIngredients()
        {
            return Ok(_context.Ingredients.ToList());
        }

        // Get Ingredient By Id
        [HttpGet("{id}")]
        public IActionResult GetIngredient(int id)
        {
            var ingredient = _context.Ingredients.Find(id);

            if (ingredient == null)
                return NotFound();

            return Ok(ingredient);
        }

        // Add Ingredient
        [HttpPost]
        public IActionResult AddIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();

            return Ok(ingredient);
        }

        // Update Ingredient
        [HttpPut("{id}")]
        public IActionResult UpdateIngredient(int id, Ingredient ingredient)
        {
            if (id != ingredient.IngredientId)
                return BadRequest();

            _context.Ingredients.Update(ingredient);
            _context.SaveChanges();

            return Ok(ingredient);
        }

        // Delete Ingredient
        [HttpDelete("{id}")]
        public IActionResult DeleteIngredient(int id)
        {
            var ingredient = _context.Ingredients.Find(id);

            if (ingredient == null)
                return NotFound();

            _context.Ingredients.Remove(ingredient);
            _context.SaveChanges();

            return Ok();
        }

        // Search Ingredient By Name
        [HttpGet("search/{name}")]
        public IActionResult SearchIngredient(string name)
        {
            var ingredients = _context.Ingredients
                .Where(i => i.IngredientName.Contains(name))
                .ToList();

            return Ok(ingredients);
        }

        // Sort Ingredients
        [HttpGet("sort")]
        public IActionResult SortIngredients()
        {
            var ingredients = _context.Ingredients
                .OrderBy(i => i.IngredientName)
                .ToList();

            return Ok(ingredients);
        }

        // Count Ingredients
        [HttpGet("count")]
        public IActionResult CountIngredients()
        {
            return Ok(_context.Ingredients.Count());
        }
    }
}