using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private ProjectContext context;

        public PaymentController(ProjectContext _context)
        {
            context = _context;
        }


        // 1. POST
        [HttpPost]
        public void AddPayment(Payment p)
        {
            context.Payments.Add(p);
            context.SaveChanges();
        }


        // 2. PUT
        [HttpPut("{id}")]
        public void UpdatePayment(int id, Payment p)
        {
            Payment payment = context.Payments.FirstOrDefault(x => x.PaymentId == id);

            if (payment != null)
            {
                payment.Amount = p.Amount;
                payment.PaymentMethod = p.PaymentMethod;
                payment.TransactionRef = p.TransactionRef;

                context.SaveChanges();
            }
        }


        // 3. PUT - Status
        [HttpPut("status/{id}")]
        public void ChangePaymentStatus(int id, string status)
        {
            Payment payment = context.Payments.FirstOrDefault(x => x.PaymentId == id);

            if (payment != null)
            {
                payment.PaymentStatus = status;
                context.SaveChanges();
            }
        }


        // 4. DELETE
        [HttpDelete("{id}")]
        public void RemovePayment(int id)
        {
            Payment payment = context.Payments.FirstOrDefault(x => x.PaymentId == id);

            if (payment != null)
            {
                context.Payments.Remove(payment);
                context.SaveChanges();
            }
        }


        // 5. GET ALL
        [HttpGet]
        public List<Payment> GetALLPayments()
        {
            return context.Payments.Include(p => p.Order).ToList();
        }


        // 6. GET BY ID
        [HttpGet("{id}")]
        public Payment GetPayment(int id)
        {
            return context.Payments.Include(p => p.Order).FirstOrDefault(p => p.PaymentId == id);
        }


        // 7. FILTER
        [HttpGet("filter")]
        public List<Payment> FilterPayments(string status)
        {
            return context.Payments.Where(p => p.PaymentStatus == status).ToList();
        }


        // 8. SORT / AGGREGATE
        [HttpGet("sort")]
        public List<Payment> SortPayments()
        {
            return context.Payments
                .OrderByDescending(p => p.Amount)
                .ToList();
        }
    }
}