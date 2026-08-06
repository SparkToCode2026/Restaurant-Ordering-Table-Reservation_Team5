using Microsoft.AspNetCore.Mvc;
using Team5.Models;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ProjectContext _context;

        public UserController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        // Get User By Id
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // Add User
        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        // Update User
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest();

            _context.Users.Update(user);
            _context.SaveChanges();

            return Ok(user);
        }

        // Delete User
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
        }

        // Search User By Name
        [HttpGet("search/{name}")]
        public IActionResult SearchUsers(string name)
        {
            var users = _context.Users
                .Where(u => u.UserName.Contains(name))
                .ToList();

            return Ok(users);
        }

        // Sort Users
        [HttpGet("sort")]
        public IActionResult SortUsers()
        {
            var users = _context.Users
                .OrderBy(u => u.UserName)
                .ToList();

            return Ok(users);
        }

        // Count Users
        [HttpGet("count")]
        public IActionResult CountUsers()
        {
            return Ok(_context.Users.Count());
        }
    }
}