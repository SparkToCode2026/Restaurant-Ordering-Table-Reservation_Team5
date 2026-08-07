using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Dtos;
using RestaurantApi.Models;
using RestaurantApi.Services;

namespace RestaurantApi.Controllers;

/// <summary>
/// Full CRUD + query API for Reservation.
/// Assigned model — EF Core Code task. Requires any authenticated user
/// (no extra Roles restriction — matches the rest of the team's dining
/// endpoints, which are open to Customer/Staff/Admin alike).
/// </summary>
[ApiController]
[Route("api/reservations")]
[Authorize]
public class ReservationsController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService;
    private readonly ILogger<ReservationsController> _logger;

    public ReservationsController(ApplicationDbContext db, IEmailService emailService, ILogger<ReservationsController> logger)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
    }

    // 1) POST — create + fires the reservation-confirmation email (domain-specific requirement)
    [HttpPost]
    public async Task<ActionResult<Reservation>> Create(ReservationCreateDto dto)
    {
        var table = await _db.Tables.FindAsync(dto.TableId);
        if (table is null || !table.IsActive) return BadRequest(new { message = "TableId does not reference an active table." });
        if (table.Capacity < dto.PartySize) return BadRequest(new { message = $"Table {table.TableNumber} only seats {table.Capacity}." });

        var userExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists) return BadRequest(new { message = "UserId does not reference an existing user." });

        var reservation = new Reservation
        {
            UserId = dto.UserId,
            TableId = dto.TableId,
            ReservationDate = dto.ReservationDate,
            ReservationTime = dto.ReservationTime,
            PartySize = dto.PartySize,
            Status = ReservationStatus.Confirmed
        };
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();

        await _db.Entry(reservation).Reference(r => r.User).LoadAsync();
        await _db.Entry(reservation).Reference(r => r.Table).LoadAsync();

        try
        {
            await _emailService.SendReservationConfirmationAsync(reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reservation #{Id} created but confirmation email failed.", reservation.Id);
        }

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    // 2) PUT — update date/time/party size
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ReservationUpdateDto dto)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation is null) return NotFound();

        reservation.ReservationDate = dto.ReservationDate;
        reservation.ReservationTime = dto.ReservationTime;
        reservation.PartySize = dto.PartySize;
        await _db.SaveChangesAsync();
        return Ok(reservation);
    }

    // 3) PATCH — second, distinct update case: status change (confirm/cancel/complete)
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, ReservationStatusUpdateDto dto)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation is null) return NotFound();

        reservation.Status = dto.Status;
        await _db.SaveChangesAsync();
        return Ok(reservation);
    }

    // 4) DELETE
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _db.Reservations.FindAsync(id);
        if (reservation is null) return NotFound();

        _db.Reservations.Remove(reservation);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // 5) GET (list) — includes User and Table navigation
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAll()
    {
        var reservations = await _db.Reservations.Include(r => r.User).Include(r => r.Table).ToListAsync();
        return Ok(reservations);
    }

    // 6) GET (find)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Reservation>> GetById(int id)
    {
        var reservation = await _db.Reservations.Include(r => r.User).Include(r => r.Table).FirstOrDefaultAsync(r => r.Id == id);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    // 7) GET (filter) — by date range and/or status
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Reservation>>> Filter([FromQuery] DateOnly? from, [FromQuery] DateOnly? to, [FromQuery] ReservationStatus? status)
    {
        var query = _db.Reservations.Include(r => r.User).Include(r => r.Table).AsQueryable();

        if (from.HasValue) query = query.Where(r => r.ReservationDate >= from.Value);
        if (to.HasValue) query = query.Where(r => r.ReservationDate <= to.Value);
        if (status.HasValue) query = query.Where(r => r.Status == status.Value);

        return Ok(await query.OrderBy(r => r.ReservationDate).ToListAsync());
    }

    // 8) GET (sort/aggregate) — reservation counts grouped by status
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _db.Reservations
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();
        return Ok(summary);
    }
}
