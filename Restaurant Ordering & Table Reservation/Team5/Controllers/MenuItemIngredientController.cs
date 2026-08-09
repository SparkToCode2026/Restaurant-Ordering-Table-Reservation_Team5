using Microsoft.AspNetCore.Mvc;
using Team5.Models;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemIngredientController : ControllerBase
    {
        private readonly ProjectContext _context;

        public MenuItemIngredientController(ProjectContext context)
        {
            _context = context;
        }

        // Get All Menu Item Ingredients
        [HttpGet]
        public IActionResult GetMenuItemIngredients()
        {
            return Ok(_context.MenuItemIngredients.ToList());
        }

        // Get Menu Item Ingredient By Id
        [HttpGet("{id}")]
        public IActionResult GetMenuItemIngredient(int id)
        {
            var item = _context.MenuItemIngredients.Find(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // Add Menu Item Ingredient
        [HttpPost]
        public IActionResult AddMenuItemIngredient(
            MenuItemIngredient item)
        {
            _context.MenuItemIngredients.Add(item);
            _context.SaveChanges();

            return Ok(item);
        }

        // Update Menu Item Ingredient
        [HttpPut("{id}")]
        public IActionResult UpdateMenuItemIngredient(
            int id,
            MenuItemIngredient item)
        {
            if (id != item.MenuItemIngredientId)
                return BadRequest();

            _context.MenuItemIngredients.Update(item);
            _context.SaveChanges();

            return Ok(item);
        }

        // Delete Menu Item Ingredient
        [HttpDelete("{id}")]
        public IActionResult DeleteMenuItemIngredient(int id)
        {
            var item = _context.MenuItemIngredients.Find(id);

            if (item == null)
                return NotFound();

            _context.MenuItemIngredients.Remove(item);
            _context.SaveChanges();

            return Ok();
        }

        // Search By Quantity
        [HttpGet("search/{quantity}")]
        public IActionResult SearchMenuItemIngredient(decimal quantity)
        {
            var items = _context.MenuItemIngredients
                .Where(i => i.QuantityRequired == quantity)
                .ToList();

            return Ok(items);
        }

        // Sort
        [HttpGet("sort")]
        public IActionResult SortMenuItemIngredients()
        {
            var items = _context.MenuItemIngredients
                .OrderBy(i => i.QuantityRequired)
                .ToList();

            return Ok(items);
        }

        // Count
        [HttpGet("count")]
        public IActionResult CountMenuItemIngredients()
        {
            return Ok(_context.MenuItemIngredients.Count());
        }
    }
}