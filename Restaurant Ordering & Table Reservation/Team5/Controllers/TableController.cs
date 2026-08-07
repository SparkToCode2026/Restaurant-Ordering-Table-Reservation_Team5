
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

        // 2. PUT - Update the Table
        [HttpPut("UpdateTable")]
        public IActionResult UpdateTable(int id, Table newTable)
        {
            Table table = context.Tables.FirstOrDefault(t => t.TableId == id);

            if (table == null)
            {
                return NotFound("Table Not Found");
            }

            table.TableNumber = newTable.TableNumber;
            table.Capacity = newTable.Capacity;
            table.Location = newTable.Location;
            table.IsActive = newTable.IsActive;

            context.SaveChanges();

            return Ok("Table Updated Successfully");
        }

        // 3. PATCH - Update Table Status
        [HttpPatch("UpdateTableStatus")]
        public IActionResult UpdateTableStatus(int id, bool status)
        {
            Table table = context.Tables.FirstOrDefault(t => t.TableId == id);

            if (table == null)
            {
                return NotFound("Table Not Found");
            }

            table.IsActive = status;

            context.SaveChanges();

            return Ok("Table Status Updated Successfully");
        }

        // 4. DELETE - Remove a Table
        [HttpDelete("RemoveTable")]
        public IActionResult RemoveTable(int id)
        {
            Table table = context.Tables.FirstOrDefault(t => t.TableId == id);

            if (table == null)
            {
                return NotFound("Table Not Found");
            }

            context.Tables.Remove(table);
            context.SaveChanges();

            return Ok("Table Removed Successfully");
        }


        // 5. GET - Get all Tables with related Orders and Reservations
        [HttpGet("GetAllTables")]
        public IActionResult GetAllTables()
        {
            List<Table> tables = context.Tables
                .Include(t => t.Orders)
                .Include(t => t.Reservations)
                .ToList();

            return Ok(tables);
        }

        // 6. GET - Get one Table by ID
        [HttpGet("GetTable")]
        public IActionResult GetTable(int id)
        {
            Table table = context.Tables
                .Include(t => t.Orders)
                .Include(t => t.Reservations)
                .FirstOrDefault(t => t.TableId == id);

            if (table == null)
            {
                return NotFound("Table Not Found");
            }

            return Ok(table);
        }

        // 7. GET - Filter Tables by Capacity
        [HttpGet("GetTablesByCapacity")]
        public IActionResult GetTablesByCapacity(int capacity)
        {
            List<Table> tables = context.Tables
                .Where(t => t.Capacity >= capacity)
                .ToList();

            return Ok(tables);
        }

        // 8. GET - Sort Tables by Capacity
        [HttpGet("SortTablesByCapacity")]
        public IActionResult SortTablesByCapacity()
        {
            List<Table> tables = context.Tables
                .OrderByDescending(t => t.Capacity)
                .ToList();

            return Ok(tables);
        }

    }
}
