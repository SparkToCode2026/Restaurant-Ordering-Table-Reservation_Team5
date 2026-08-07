using Microsoft.AspNetCore.Mvc;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoryController : ControllerBase
    {
        private readonly ProjectContext _context;

        public MenuCategoryController(ProjectContext context)
        {
            _context = context;
        }


    }
}
