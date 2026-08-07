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

       
    }
}
