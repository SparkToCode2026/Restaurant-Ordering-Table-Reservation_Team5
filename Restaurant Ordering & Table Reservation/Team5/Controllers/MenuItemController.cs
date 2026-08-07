using Microsoft.AspNetCore.Mvc;
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
    }
}
