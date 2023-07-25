using CodePulse.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        //on classname, ctrl+. for creating ctor
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
