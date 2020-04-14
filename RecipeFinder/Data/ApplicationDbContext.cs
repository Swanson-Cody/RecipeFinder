using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeFinder.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<RecipeFinder.Models.Recipe> Recipe { get; set; }
        public DbSet<RecipeFinder.Models.Ingredient> Ingredient { get; set; }
    }
}
