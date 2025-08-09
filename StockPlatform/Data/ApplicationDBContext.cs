using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockPlatform.Models;

namespace StockPlatform.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions Options) : base(Options)
        {
            
        }
        public DbSet<Stock> stocks { get; set; }
        public DbSet<Comments> comments { get; set; }
    }
}
