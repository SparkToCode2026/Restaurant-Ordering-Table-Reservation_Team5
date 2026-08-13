using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Team5.Models;

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
    [Key]
    public int OrderItemId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Required]
    public decimal UnitPrice { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    [Required]
    public decimal Subtotal { get; set; }

    //Relationships
    // One Order can contain many Order Items (Order 1 - M OrderItem)
    [ForeignKey("Order")]
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }

    // One Menu Item can appear in many Order Items (MenuItem 1 - M OrderItem)
    [ForeignKey("MenuItem")]
    public int MenuItemId { get; set; }
    [JsonIgnore]
    public MenuItem MenuItem { get; set; }
}
