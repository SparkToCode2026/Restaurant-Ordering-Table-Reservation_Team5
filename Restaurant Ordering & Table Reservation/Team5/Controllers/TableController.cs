
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Controllers
{
    [ApiController]
    [Route("Table")]
    public class TableController : ControllerBase
    {
        private ProjectContext context;

        public TableController(ProjectContext _context)
        {
            context = _context;
        }

        // 1. POST - Add a new Table
        [HttpPost("AddTable")]
        public IActionResult AddTable(Table table)
        {
            context.Tables.Add(table);
            context.SaveChanges();

            return Ok(table.TableId);
        }



    }
}
