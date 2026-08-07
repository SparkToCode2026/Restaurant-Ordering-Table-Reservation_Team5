using Microsoft.AspNetCore.Mvc;
using Team5.Models;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoryController : ControllerBase
    {
        private readonly ProjectContext context;

        public MenuCategoryController(ProjectContext _context)
        {
            context = _context;
        }

        // Create OR add Menu Category
        [HttpPost("CreateMenuCategory")]
        public IActionResult CreateMenuCategory(MenuCategory menuCategory)
        {
            // Check if the model data is valid according to validation attributes like [Required] and [StringLength]
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.MenuCategories.Add(menuCategory);
            context.SaveChanges();
            return Ok(menuCategory);
        }

        // Update Menu Category
        [HttpPut("UpdateMenuCategory/{id}")]
        public IActionResult UpdateMenuCategory(int id,MenuCategory menuCategory)
        {
            if (id != menuCategory.MenuCategoryId)
            {
                return BadRequest("Menu Category ID mismatch.");
            }
            var existingMenuCategory = context.MenuCategories.Find(id);
            if (existingMenuCategory != null)
            {
                context.MenuCategories.Update(menuCategory);
                context.SaveChanges();
                return Ok(menuCategory);
            }
            return NotFound("Menu Category not found.");
        }

        // Update Menu Category Display Order
        [HttpPatch("UpdateMenuCategoryDisplayOrder/{id}")]
        public IActionResult UpdateMenuCategoryDisplayOrder(int id, int displayOrder)
        {
            MenuCategory existingMenuCategory = context.MenuCategories.FirstOrDefault(c => c.MenuCategoryId == id);
            if (existingMenuCategory != null)
            {
                existingMenuCategory.DisplayOrder = displayOrder;
                context.SaveChanges();
                return Ok(existingMenuCategory);

            }
            else
            {
                return NotFound("Menu Category not found.");
            }
        }

        // Delete Menu Category
        [HttpDelete("DeleteMenuCategory/{id}")]
        public IActionResult DeleteMenuCategory(int id)
        {
            MenuCategory existingMenuCategory = context.MenuCategories.FirstOrDefault(c => c.MenuCategoryId == id);
            if (existingMenuCategory != null)
            {
                context.MenuCategories.Remove(existingMenuCategory);
                context.SaveChanges();
                return Ok("Menu Category deleted successfully.");
            }
            else
            {
                return NotFound("Menu Category not found.");
            }
        }

        // Get All Menu Categories
        [HttpGet("GetAllMenuCategories")]
        public IActionResult GetAllMenuCategories()
        {
            List<MenuCategory> menuCategories = context.MenuCategories.ToList();
            return Ok(menuCategories);
        }

        // Get Menu Category By Id
        [HttpGet("GetMenuCategoryById/{id}")]
        public IActionResult GetMenuCategoryById(int id)
        {
            MenuCategory menuCategory = context.MenuCategories.FirstOrDefault(c => c.MenuCategoryId == id);
            if (menuCategory != null)
            {
                return Ok(menuCategory);
            }
            else
            {
                return NotFound("Menu Category not found.");
            }
        }

        // Get Menu Categories By Filter
        [HttpGet("GetMenuCategoriesByFilter")]
        public IActionResult GetMenuCategoriesByFilter(string? name, int? displayOrder)
        {
            var query = context.MenuCategories.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.MenuCategoryName.Contains(name));
            }
            if (displayOrder != null)
            {
                query = query.Where(c => c.DisplayOrder == displayOrder);
            }
            List<MenuCategory> filteredMenuCategories = query.ToList();
            return Ok(filteredMenuCategories);
        }

        // Get Menu Category Summary
        [HttpGet("GetMenuCategorySummary")]
        public IActionResult GetMenuCategorySummary()
        {
            var summary = context.MenuCategories
                .OrderBy(c => c.DisplayOrder) // Order from lowest to highest display order
                .Select(c => new
                {
                    c.MenuCategoryId,
                    c.MenuCategoryName,
                    c.DisplayOrder,
                    MenuItemCount = c.MenuItems.Count()
                })
                .ToList();

            return Ok(summary);
        }
    }
}
