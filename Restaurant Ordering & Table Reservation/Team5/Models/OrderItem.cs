<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantApi.Models;

/// <summary>
/// Represents a single line item within an Order (one menu item + quantity).
/// Assigned model — EF Core Code task (OrderItem).
///
/// Relationships:
///   - Many-to-one with Order    (the parent order this line belongs to)
///   - Many-to-one with MenuItem (the menu item being ordered)
/// Both FKs are required (non-nullable) — a line item cannot exist without
/// an order or without a menu item.
///
/// UnitPrice is captured at order time (snapshot of MenuItem.Price) so that
/// later price changes to the menu never retroactively change historical
/// order totals. Subtotal = UnitPrice * Quantity, also stored (not computed
/// on read) so reporting queries don't need to recompute it.
/// </summary>
public class OrderItem
{
    public int Id { get; set; }

    // ---- FK: Order ----
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    // ---- FK: MenuItem ----
    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }
=======
﻿namespace Team5.Models
{
    public class OrderItem
    {
    }
>>>>>>> 4a30804116a8d144f3dcb88f0d0a9566e473d2c3
}
