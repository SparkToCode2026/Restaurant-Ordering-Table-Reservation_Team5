using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Team5.Models;

namespace Team5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private ProjectContext context;

        public ReviewController(ProjectContext _context)
        {
            context = _context;
        }

        // 1. Add Review
        [HttpPost]
        public void AddReview(Review r)
        {
            context.Reviews.Add(r);
            context.SaveChanges();
        }

        // 2. Update Review
        [HttpPut("{id}")]
        public void UpdateReview(int id, Review r)
        {
            Review review = context.Reviews
                .FirstOrDefault(x => x.ReviewId == id);

            if (review != null)
            {
                review.Rating = r.Rating;
                review.Comment = r.Comment;

                context.SaveChanges();
            }
        }

        // 3. Change Rating
        [HttpPut("rating/{id}")]
        public void ChangeRating(int id, int rating)
        {
            Review review = context.Reviews
                .FirstOrDefault(x => x.ReviewId == id);

            if (review != null)
            {
                review.Rating = rating;
                context.SaveChanges();
            }
        }

        // 4. Delete Review
        [HttpDelete("{id}")]
        public void RemoveReview(int id)
        {
            Review review = context.Reviews
                .FirstOrDefault(x => x.ReviewId == id);

            if (review != null)
            {
                context.Reviews.Remove(review);
                context.SaveChanges();
            }
        }

        // 5. Get All Reviews
        [HttpGet]
        public List<Review> GetALLReviews()
        {
            return context.Reviews
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.MenuItem)
                .ToList();
        }

        // 6. Get Review by ID
        [HttpGet("{id}")]
        public Review GetReview(int id)
        {
            return context.Reviews
                .Include(r => r.User)
                .Include(r => r.Order)
                .Include(r => r.MenuItem)
                .FirstOrDefault(r => r.ReviewId == id);
        }

        // 7. Filter Reviews
        [HttpGet("filter")]
        public List<Review> FilterReviews(int rating)
        {
            return context.Reviews
                .Where(r => r.Rating >= rating)
                .ToList();
        }

        // 8. Sort Reviews
        [HttpGet("sort")]
        public List<Review> SortReviews()
        {
            return context.Reviews
                .OrderByDescending(r => r.Rating)
                .ToList();
        }
    }
}