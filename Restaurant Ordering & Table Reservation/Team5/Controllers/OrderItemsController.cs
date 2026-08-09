using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Models;

namespace RestaurantApi.Controllers;

/// <summary>
/// Full CRUD + query API for OrderItem.
/// Assigned model — EF Core Code task. Every write recalculates the parent
/// Order's TotalAmount (line items -> order total is a derived value, so it
/// must stay in sync whenever a line item is added, changed, or removed).
/// </summary>
[ApiController]
[Route("api/order-items")]
[Authorize]
public class OrderItemsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public OrderItemsController(ApplicationDbContext db)
    {
        _db = db;
    }

    private async Task RecalculateOrderTotalAsync(int orderId)
    {
        var order = await _db.Orders.Include(o => o.OrderItems).Include(o => o.Promotion).FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is null) return;

        decimal total = order.OrderItems.Sum(oi => oi.Subtotal);
        if (order.Promotion is { IsActive: true } promo)
        {
            total -= total * (promo.DiscountPercent / 100m);
        }
        order.TotalAmount = Math.Round(total, 2);
        await _db.SaveChangesAsync();
    }

    // 1) POST — add a line item to an existing order
    [HttpPost]
    public async Task<ActionResult<OrderItem>> Create(OrderItemCreateDto dto)
    {
        var order = await _db.Orders.FindAsync(dto.OrderId);
        var menuItem = await _db.MenuItems.FindAsync(dto.MenuItemId);
        if (order is null || menuItem is null) return BadRequest(new { message = "OrderId and/or MenuItemId does not reference an existing record." });

        var item = new OrderItem
        {
            OrderId = dto.OrderId,
            MenuItemId = dto.MenuItemId,
            Quantity = dto.Quantity,
            UnitPrice = menuItem.Price,
            Subtotal = menuItem.Price * dto.Quantity
        };
        _db.OrderItems.Add(item);
        await _db.SaveChangesAsync();
        await RecalculateOrderTotalAsync(dto.OrderId);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    // 2) PUT — change quantity
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, OrderItemUpdateDto dto)
    {
        var item = await _db.OrderItems.FindAsync(id);
        if (item is null) return NotFound();

        item.Quantity = dto.Quantity;
        item.Subtotal = item.UnitPrice * dto.Quantity;
        await _db.SaveChangesAsync();
        await RecalculateOrderTotalAsync(item.OrderId);

        return Ok(item);
    }

    // 3) PATCH — second, distinct update case: swap to a different menu item (update via related FK)
    [HttpPatch("{id:int}/menu-item")]
    public async Task<IActionResult> SwapMenuItem(int id, OrderItemMenuItemChangeDto dto)
    {
        var item = await _db.OrderItems.FindAsync(id);
        if (item is null) return NotFound();

        var menuItem = await _db.MenuItems.FindAsync(dto.MenuItemId);
        if (menuItem is null) return BadRequest(new { message = "MenuItemId does not reference an existing menu item." });

        item.MenuItemId = menuItem.Id;
        item.UnitPrice = menuItem.Price;
        item.Subtotal = menuItem.Price * item.Quantity;
        await _db.SaveChangesAsync();
        await RecalculateOrderTotalAsync(item.OrderId);

        return Ok(item);
    }

    // 4) DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.OrderItems.FindAsync(id);
        if (item is null) return NotFound();

        var orderId = item.OrderId;
        _db.OrderItems.Remove(item);
        await _db.SaveChangesAsync();
        await RecalculateOrderTotalAsync(orderId);

        return NoContent();
    }

    // 5) GET (list) — includes Order and MenuItem navigation
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetAll()
    {
        var items = await _db.OrderItems.Include(oi => oi.Order).Include(oi => oi.MenuItem).ToListAsync();
        return Ok(items);
    }

    // 6) GET (find)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderItem>> GetById(int id)
    {
        var item = await _db.OrderItems.Include(oi => oi.Order).Include(oi => oi.MenuItem).FirstOrDefaultAsync(oi => oi.Id == id);
        return item is null ? NotFound() : Ok(item);
    }

    // 7) GET (filter) — all line items for one order
    [HttpGet("by-order/{orderId:int}")]
    public async Task<ActionResult<IEnumerable<OrderItem>>> GetByOrder(int orderId)
    {
        var items = await _db.OrderItems.Include(oi => oi.MenuItem).Where(oi => oi.OrderId == orderId).ToListAsync();
        return Ok(items);
    }

    // 8) GET (sort/aggregate) — best-selling menu items by total quantity ordered
    [HttpGet("best-sellers")]
    public async Task<IActionResult> GetBestSellers()
    {
        var result = await _db.OrderItems
            .GroupBy(oi => oi.MenuItem!.Name)
            .Select(g => new { MenuItem = g.Key, TotalQuantitySold = g.Sum(oi => oi.Quantity), TotalRevenue = g.Sum(oi => oi.Subtotal) })
            .OrderByDescending(g => g.TotalQuantitySold)
            .ToListAsync();
        return Ok(result);
    }
}
