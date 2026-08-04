using Team5.Models;
using Microsoft.EntityFrameworkCore;
namespace Team5
{
    public class ProjectContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

    }
}
