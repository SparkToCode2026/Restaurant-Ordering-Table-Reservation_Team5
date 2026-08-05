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

        //Register the DbSet for the Order 
        public DbSet<Order> Orders { get; set; }
        //Register the DbSet for the Table 
        public DbSet<Table> Tables { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {

        }

    }
}
