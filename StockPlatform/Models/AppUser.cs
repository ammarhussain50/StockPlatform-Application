using Microsoft.AspNetCore.Identity;

namespace StockPlatform.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
        //        Ye line add karne ka main purpose hai:
        //AppUser ko Portfolio se navigation property provide karna — taake jab bhi kisi user ko fetch karo, uske saath related portfolio items(stocks) bhi mil sakein.


    }
}
