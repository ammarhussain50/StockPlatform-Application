using Microsoft.AspNetCore.Identity;
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
        public DbSet<Portfolio> portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));
            // Portfolio ek *join table* hai jo AppUser (user) aur Stock ke darmiyan many-to-many relationship ko represent karti hai.
            // Ek user multiple stocks own kar sakta hai, aur ek stock multiple users ke paas ho sakta hai — isliye beech mein ek separate table chahiye jo yeh mapping store kare.

            // HasKey(p => new { p.AppUserId, p.StockId }); line ka matlab hai:
            // Humne `Portfolio` class ke do columns (AppUserId & StockId) milake ek *composite primary key* define ki hai.
            // Iska faida yeh hota hai ke:
            // 1. Har user-stock combination unique rahega (e.g., aik user aik hi stock ko do dafa own nahi kar sakta).
            // 2. EF Core ko clearly pata hota hai ke yeh table sirf join ka kaam kar rahi hai aur kis columns se related hai.

            // Agar yeh composite key define na karo to EF Core confusion mein reh sakta hai ya default auto-incremented ID create karega,
            // jisse many-to-many ka purpose fail ho jaata hai. Isliye yeh step zaroori hai.
            builder.Entity<Portfolio>()
    .HasOne(u => u.AppUser)                  // Har Portfolio ka aik AppUser hota hai (many-to-one)
    .WithMany(u => u.Portfolios)            // Ek AppUser ke paas multiple Portfolios ho sakte hain (one-to-many)
    .HasForeignKey(p => p.AppUserId); // Foreign key Portfolio table mein AppUserId hai



            // Portfolio entity ka relation ek AppUser ke sath hai — har Portfolio sirf ek user ka ho sakta hai
            // HasOne(): Portfolio side se relation batata hai (ek user ka reference hoga har Portfolio mein)
            // WithMany(): User side se batata hai ke us user ke kai Portfolios ho sakte hain
            // HasForeignKey(): Portfolio table mein kis field ke zariye user ko reference karna hai — yahan AppUserId









            builder.Entity<Portfolio>()
                .HasOne(u => u.Stock)                 // Har Portfolio ka ek Stock hota hai
                .WithMany(u => u.Portfolios)         // Ek Stock ke multiple Portfolios ho sakte hain
                .HasForeignKey(p => p.StockId);      // Foreign key Portfolio table mein hai: StockId



            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "b5d83d8d-1a8d-4a4b-8c2d-2f05f9a77e61", // Static GUID
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "b5d83d8d-1a8d-4a4b-8c2d-2f05f9a77e61" // Match Id for simplicity
                },
                new IdentityRole
                {
                    Id = "8d88f06d-c6d3-4d42-9f7e-fbc72704e2d1", // Static GUID
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "8d88f06d-c6d3-4d42-9f7e-fbc72704e2d1" // Match Id for simplicity
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
