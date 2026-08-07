using Microsoft.AspNetCore.Mvc;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController
    {
        private readonly ProjectContext context;

        public MenuItemController(ProjectContext _context)
        {
            context = _context;
        }

    }
}
