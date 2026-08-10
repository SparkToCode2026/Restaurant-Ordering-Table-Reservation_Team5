using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;
using Team5.Services;

namespace Team5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ProjectContext context;
        private readonly EmailService emailService;

        public ReservationController(ProjectContext _context, EmailService _emailService)
        {
            context = _context;
            emailService = _emailService;
        }

        // 1. Add Reservation
        [HttpPost]
        public IActionResult AddReservation(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Reservations.Add(reservation);
            context.SaveChanges();

            return Ok(reservation);
        }

        // 2. Update Reservation
        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return BadRequest("Reservation ID mismatch.");
            }

            var existingReservation = context.Reservations.FirstOrDefault(r => r.ReservationId == id);
            if (existingReservation == null)
            {
                return NotFound("Reservation not found.");
            }

            existingReservation.ReservationDate = reservation.ReservationDate;
            existingReservation.ReservationTime = reservation.ReservationTime;
            existingReservation.PartySize = reservation.PartySize;
            existingReservation.Status = reservation.Status;
            existingReservation.CreatedAt = reservation.CreatedAt;
            existingReservation.UserId = reservation.UserId;
            existingReservation.TableId = reservation.TableId;

            context.SaveChanges();
            return Ok(existingReservation);
        }

        // 3. Change Reservation Status
        [HttpPut("status/{id}")]
        public IActionResult ChangeReservationStatus(int id, string status)
        {
            var reservation = context.Reservations.FirstOrDefault(r => r.ReservationId == id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            reservation.Status = status;
            context.SaveChanges();

            return Ok(reservation);
        }

        // 4. Delete Reservation
        [HttpDelete("{id}")]
        public IActionResult RemoveReservation(int id)
        {
            var reservation = context.Reservations.FirstOrDefault(r => r.ReservationId == id);
            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            context.Reservations.Remove(reservation);
            context.SaveChanges();

            return Ok("Reservation deleted successfully.");
        }

        // 5. Get All Reservations
        [HttpGet]
        public IActionResult GetAllReservations()
        {
            List<Reservation> reservations = context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .ToList();

            return Ok(reservations);
        }

        // 6. Get Reservation by ID
        [HttpGet("{id}")]
        public IActionResult GetReservation(int id)
        {
            var reservation = context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .FirstOrDefault(r => r.ReservationId == id);

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            return Ok(reservation);
        }

        // 7. Filter Reservations
        [HttpGet("filter")]
        public IActionResult FilterReservations(DateOnly? reservationDate, string? status, int? userId, int? tableId)
        {
            var query = context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .AsQueryable();

            if (reservationDate.HasValue)
            {
                query = query.Where(r => r.ReservationDate == reservationDate.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(r => r.Status.Contains(status));
            }

            if (userId.HasValue)
            {
                query = query.Where(r => r.UserId == userId.Value);
            }

            if (tableId.HasValue)
            {
                query = query.Where(r => r.TableId == tableId.Value);
            }

            return Ok(query.ToList());
        }

        // 8. Sort Reservations
        [HttpGet("sort")]
        public IActionResult SortReservations()
        {
            List<Reservation> reservations = context.Reservations
                .Include(r => r.User)
                .Include(r => r.Table)
                .OrderBy(r => r.ReservationDate)
                .ThenBy(r => r.ReservationTime)
                .ToList();

            return Ok(reservations);
        }
    }
}
