using Microsoft.AspNetCore.Mvc;
using Team5.Models;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ProjectContext _context;

        public PaymentController(ProjectContext context)
        {
            _context = context;
        }

        // Get All Payments
        [HttpGet]
        public IActionResult GetPayments()
        {
            return Ok(_context.Payments.ToList());
        }

        // Get Payment By Id
        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _context.Payments.Find(id);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        // Add Payment
        [HttpPost]
        public IActionResult AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();

            return Ok(payment);
        }

        // Update Payment
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, Payment payment)
        {
            if (id != payment.PaymentId)
                return BadRequest();

            _context.Payments.Update(payment);
            _context.SaveChanges();

            return Ok(payment);
        }

        // Delete Payment
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = _context.Payments.Find(id);

            if (payment == null)
                return NotFound();

            _context.Payments.Remove(payment);
            _context.SaveChanges();

            return Ok();
        }

        // Search Payment By Status
        [HttpGet("search/{status}")]
        public IActionResult SearchPayment(string status)
        {
            var payments = _context.Payments
                .Where(p => p.PaymentStatus.Contains(status))
                .ToList();

            return Ok(payments);
        }

        // Sort Payments
        [HttpGet("sort")]
        public IActionResult SortPayments()
        {
            var payments = _context.Payments
                .OrderByDescending(p => p.Amount)
                .ToList();

            return Ok(payments);
        }

        // Count Payments
        [HttpGet("count")]
        public IActionResult CountPayments()
        {
            return Ok(_context.Payments.Count());
        }
    }
}