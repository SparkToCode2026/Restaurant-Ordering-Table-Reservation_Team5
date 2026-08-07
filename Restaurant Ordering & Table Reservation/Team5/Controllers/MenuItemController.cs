using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ProjectContext context;

        public MenuItemController(ProjectContext _context)
        {
            context = _context;
        }

        // Create OR add Menu Item
        [HttpPost("CreateMenuItem")]
        public IActionResult CreateMenuItem(MenuItem menuItem)
        {
            // Check if the model data is valid according to validation attributes like [Required] and [StringLength]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.MenuItems.Add(menuItem);
            context.SaveChanges();
            return Ok(menuItem);
        }

        // Update Menu Item
        [HttpPut("UpdateMenuItem/{id}")]
        public IActionResult UpdateMenuItem(int id, MenuItem menuItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingMenuItem = context.MenuItems.Find(id);
            if (existingMenuItem == null)
            {
                return NotFound("Menu Item not found.");
            }

            existingMenuItem.MenuItemName = menuItem.MenuItemName;
            existingMenuItem.MenuItemDescription = menuItem.MenuItemDescription;
            existingMenuItem.Price = menuItem.Price;
            existingMenuItem.ImageUrl = menuItem.ImageUrl;
            existingMenuItem.IsAvailable = menuItem.IsAvailable;
            existingMenuItem.MenuCategoryId = menuItem.MenuCategoryId;

            context.SaveChanges();
            return Ok(existingMenuItem);
        }

        // Update Menu Item Price
        [HttpPatch("UpdateMenuItemPrice/{id}")]
        public IActionResult UpdateMenuItemPrice(int id, [FromBody] decimal newPrice)
        {
            MenuItem existingMenuItem = context.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            if (existingMenuItem == null)
            {
                return NotFound("Menu Item not found.");
            }

            existingMenuItem.Price = newPrice;
            context.SaveChanges();
            return Ok(existingMenuItem);
        }

        // Delete Menu Item
        [HttpDelete("DeleteMenuItem/{id}")]
        public IActionResult DeleteMenuItem(int id)
        {
            MenuItem existingMenuItem = context.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            if (existingMenuItem == null)
            {
                return NotFound("Menu Item not found.");
            }
            context.MenuItems.Remove(existingMenuItem);
            context.SaveChanges();
            return Ok("Menu Item deleted successfully.");
        }

        // Get all Menu Items.
        [HttpGet("GetAllMenuItems")]
        public IActionResult GetAllMenuItems()
        {
            List<MenuItem> menuItems = context.MenuItems.Include(m => m.Category).ToList();
            return Ok(menuItems);
        }

        // Get Menu Item by ID
        [HttpGet("GetMenuItemById/{id}")]
        public IActionResult GetMenuItem(int id) 
        {
            MenuItem menuItem = context.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            if (menuItem != null)
            {
                return Ok(menuItem);
            }
            else
            {
                return NotFound("Menu Item not found.");
            }
        }

        // Get Menu Items by Filter
        [HttpGet("GetMenuItemsByFilter")]
        public IActionResult GetMenuItemsByFilter(string? name, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            var query = context.MenuItems.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.MenuItemName.Contains(name));
            }
            if (minPrice.HasValue)
            {
                query = query.Where(m => m.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(m => m.Price <= maxPrice.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(m => m.MenuCategoryId == categoryId.Value);
            }
            List<MenuItem> filteredMenuItems = query.ToList();
            return Ok(filteredMenuItems);
        }
    }
}
