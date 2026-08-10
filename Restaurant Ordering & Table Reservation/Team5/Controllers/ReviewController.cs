using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ReviewController(ProjectContext context)
        {
            _context = context;
        }

        // 1. Create Review
        [HttpPost]
        public IActionResult CreateReview(Review review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Reviews.Add(review);
            _context.SaveChanges();

            return Ok(review);
        }

        // 2. Update Review
        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, Review review)
        {
            var existingReview = _context.Reviews.Find(id);

            if (existingReview == null)
                return NotFound();

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;

            _context.SaveChanges();

            return Ok(existingReview);
        }

        // 3. Update Rating
        [HttpPatch("{id}/rating")]
        public IActionResult UpdateRating(
            int id,
            [FromBody] int rating)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
                return NotFound();

            review.Rating = rating;

            _context.SaveChanges();

            return Ok(review);
        }

        // 4. Delete Review
        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
                return NotFound();

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return Ok("Review deleted successfully");
        }

        // 5. Get All Reviews
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.MenuItem)
                .ToList();

            return Ok(reviews);
        }

        // 6. Get Review By Id
        [HttpGet("{id}")]
        public IActionResult GetReview(int id)
        {
            var review = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.MenuItem)
                .FirstOrDefault(r => r.ReviewId == id);

            if (review == null)
                return NotFound();

            return Ok(review);
        }

        // 7. Filter Reviews By Rating
        [HttpGet("filter")]
        public IActionResult FilterReviews(int rating)
        {
            var reviews = _context.Reviews
                .Where(r => r.Rating == rating)
                .ToList();

            return Ok(reviews);
        }

        // 8. Average Review Rating
        [HttpGet("aggregate")]
        public IActionResult ReviewAggregate()
        {
            var totalReviews = _context.Reviews.Count();

            var averageRating = 0.0;

            if (totalReviews > 0)
            {
                averageRating = _context.Reviews
                    .Average(r => r.Rating);
            }

            return Ok(new
            {
                TotalReviews = totalReviews,
                AverageRating = averageRating
            });
        }
    }
}