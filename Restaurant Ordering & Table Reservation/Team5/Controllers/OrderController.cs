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

        // 2. PUT - Update the Order
        [HttpPut("UpdateOrder")]
        public IActionResult UpdateOrder(int id, Order newOrder)
        {
            Order order = context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order Not Found");
            }

            order.OrderType = newOrder.OrderType;
            order.Status = newOrder.Status;
            order.OrderDate = newOrder.OrderDate;
            order.TotalAmount = newOrder.TotalAmount;
            order.UserId = newOrder.UserId;
            order.TableId = newOrder.TableId;

            context.SaveChanges();

            return Ok("Order Updated Successfully");
        }

        // 3. PATCH - Update Order Status
        [HttpPatch("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus(int id, string status)
        {
            Order order = context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order Not Found");
            }

            order.Status = status;

            context.SaveChanges();

            return Ok("Order Status Updated Successfully");
        }

        // 4. DELETE - Remove an Order
        [HttpDelete("RemoveOrder")]
        public IActionResult RemoveOrder(int id)
        {
            Order order = context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound("Order Not Found");
            }

            context.Orders.Remove(order);
            context.SaveChanges();

            return Ok("Order Removed Successfully");
        }



    }
}
