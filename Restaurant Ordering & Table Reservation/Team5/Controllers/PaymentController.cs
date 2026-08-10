using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;
using Team5.Services;

namespace Team5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly EmailService emailService;

        public PaymentController(ProjectContext context, EmailService _emailService)
        {
            _context = context;
            emailService = _emailService;
        }

        // 1. Add Payment
        [HttpPost]
        public IActionResult AddPayment(Payment payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Payments.Add(payment);
            _context.SaveChanges();
            // Send email notification to the user
            var order = _context.Orders.Find(payment.OrderId);
            if (order != null)
            {
                var user = _context.Users.Find(order.UserId);
                if (user != null)
                {
                    emailService.SendEmail(
                        user.UserEmail,
                        user.UserName,
                        "Order Receipt - Payment Confirmed",
                        $"<h3>Hi {user.UserName},</h3><p>Your payment of {payment.Amount:C} for Order #{order.OrderId} has been confirmed.</p><p>Payment Method: {payment.PaymentMethod}</p><p>Transaction Ref: {payment.TransactionRef}</p><p>Date: {payment.PaymentDate}</p>"
                    );
                }
            }

            return Ok(payment);
        }

        // 2. Update Payment
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, Payment payment)
        {
            if (id != payment.PaymentId)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingPayment = _context.Payments.Find(id);

            if (existingPayment == null)
                return NotFound();

            existingPayment.Amount = payment.Amount;
            existingPayment.PaymentStatus = payment.PaymentStatus;

            _context.SaveChanges();

            return Ok(existingPayment);
        }

        // 3. Update Payment Status
        [HttpPatch("{id}/status")]
        public IActionResult UpdatePaymentStatus(
            int id,
            [FromBody] string status)
        {
            var payment = _context.Payments.Find(id);

            if (payment == null)
                return NotFound();

            payment.PaymentStatus = status;

            _context.SaveChanges();

            return Ok(payment);
        }

        // 4. Delete Payment
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = _context.Payments.Find(id);

            if (payment == null)
                return NotFound();

            _context.Payments.Remove(payment);
            _context.SaveChanges();

            return Ok(payment);
        }

        // 5. Get All Payments
        [HttpGet]
        public IActionResult GetPayments()
        {
            var payments = _context.Payments
                .Include(p => p.Order)
                .ToList();

            return Ok(payments);
        }

        // 6. Get Payment By Id
        [HttpGet("{id}")]
        public IActionResult GetPayment(int id)
        {
            var payment = _context.Payments
                .Include(p => p.Order)
                .FirstOrDefault(p => p.PaymentId == id);

            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        // 7. Search Payment By Status
        [HttpGet("search/{status}")]
        public IActionResult SearchPayment(string status)
        {
            var payments = _context.Payments
                .Where(p => p.PaymentStatus.Contains(status))
                .ToList();

            return Ok(payments);
        }

        // 8. Sort Payments By Amount
        [HttpGet("sort")]
        public IActionResult SortPayments()
        {
            var payments = _context.Payments
                .OrderByDescending(p => p.Amount)
                .ToList();

            return Ok(payments);
        }

        // 8. Count Payments
        [HttpGet("count")]
        public IActionResult CountPayments()
        {
            var count = _context.Payments.Count();

            return Ok(count);
        }
    }
}
