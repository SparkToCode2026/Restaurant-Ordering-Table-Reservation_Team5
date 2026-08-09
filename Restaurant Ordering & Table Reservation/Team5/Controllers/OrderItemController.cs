using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Models;
using Team5.Models;

namespace Team5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly ProjectContext context;

        public OrderItemController(ProjectContext _context)
        {
            context = _context;
        }

        // 1. Add Order Item
        [HttpPost]
        public IActionResult AddOrderItem(OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.OrderItems.Add(orderItem);
            context.SaveChanges();

            return Ok(orderItem);
        }

        // 2. Update Order Item
        [HttpPut("{id}")]
        public IActionResult UpdateOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.OrderItemId)
            {
                return BadRequest("Order Item ID mismatch.");
            }

            var existingOrderItem = context.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            if (existingOrderItem == null)
            {
                return NotFound("Order Item not found.");
            }

            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.UnitPrice = orderItem.UnitPrice;
            existingOrderItem.Subtotal = orderItem.Subtotal;
            existingOrderItem.OrderId = orderItem.OrderId;
            existingOrderItem.MenuItemId = orderItem.MenuItemId;

            context.SaveChanges();
            return Ok(existingOrderItem);
        }

        // 3. Change Quantity
        [HttpPut("quantity/{id}")]
        public IActionResult ChangeQuantity(int id, int quantity)
        {
            var orderItem = context.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound("Order Item not found.");
            }

            orderItem.Quantity = quantity;
            orderItem.Subtotal = orderItem.UnitPrice * quantity;
            context.SaveChanges();

            return Ok(orderItem);
        }

        // 4. Delete Order Item
        [HttpDelete("{id}")]
        public IActionResult RemoveOrderItem(int id)
        {
            var orderItem = context.OrderItems.FirstOrDefault(o => o.OrderItemId == id);
            if (orderItem == null)
            {
                return NotFound("Order Item not found.");
            }

            context.OrderItems.Remove(orderItem);
            context.SaveChanges();

            return Ok("Order Item deleted successfully.");
        }

        // 5. Get All Order Items
        [HttpGet]
        public IActionResult GetAllOrderItems()
        {
            List<OrderItem> orderItems = context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.MenuItem)
                .ToList();

            return Ok(orderItems);
        }

        // 6. Get Order Item by ID
        [HttpGet("{id}")]
        public IActionResult GetOrderItem(int id)
        {
            var orderItem = context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.MenuItem)
                .FirstOrDefault(o => o.OrderItemId == id);

            if (orderItem == null)
            {
                return NotFound("Order Item not found.");
            }

            return Ok(orderItem);
        }

        // 7. Filter Order Items
        [HttpGet("filter")]
        public IActionResult FilterOrderItems(int? orderId, int? menuItemId, int? quantity)
        {
            var query = context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.MenuItem)
                .AsQueryable();

            if (orderId.HasValue)
            {
                query = query.Where(o => o.OrderId == orderId.Value);
            }

            if (menuItemId.HasValue)
            {
                query = query.Where(o => o.MenuItemId == menuItemId.Value);
            }

            if (quantity.HasValue)
            {
                query = query.Where(o => o.Quantity >= quantity.Value);
            }

            return Ok(query.ToList());
        }

        // 8. Sort Order Items
        [HttpGet("sort")]
        public IActionResult SortOrderItems()
        {
            List<OrderItem> orderItems = context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.MenuItem)
                .OrderByDescending(o => o.Subtotal)
                .ToList();

            return Ok(orderItems);
        }
    }
}
