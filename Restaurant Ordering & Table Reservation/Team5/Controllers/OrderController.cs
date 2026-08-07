using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Controllers
{
    [ApiController]
    [Route("Order")]
    public class OrderController : ControllerBase
    {
        private ProjectContext context;

        public OrderController(ProjectContext _context)
        {
            context = _context;
        }


        // 1. POST - Add a new Order
        [HttpPost("AddOrder")]
        public IActionResult AddOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();

            return Ok(order.OrderId);
        }
    }
}
