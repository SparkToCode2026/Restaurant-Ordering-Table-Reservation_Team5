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


    }
}
