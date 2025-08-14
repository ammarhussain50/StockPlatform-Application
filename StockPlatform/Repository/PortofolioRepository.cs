using Microsoft.EntityFrameworkCore;
using StockPlatform.Data;
using StockPlatform.Interfaces;
using StockPlatform.Models;

namespace StockPlatform.Repository
{
    public class PortofolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext context;

        public PortofolioRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            // Portfolio table me se wo records dhoondh rahe hain jahan AppUserId == current user ka Id
            return await context.portfolios
                .Where(u => u.AppUserId == user.Id)

                // Har matching portfolio record ka related Stock object nikaal rahe hain
                // Aur us stock ka sirf specific data select kar ke naya Stock object bana rahe hain
                .Select(stock => new Stock
                {
                    Id = stock.Stock.Id,                       // Stock ID
                    Symbol = stock.Stock.Symbol,               // Stock symbol (e.g. AAPL)
                    CompanyName = stock.Stock.CompanyName,     // Company name
                    Purchase = stock.Stock.Purchase,           // Purchase price
                    LastDiv = stock.Stock.LastDiv,             // Last dividend
                    Industry = stock.Stock.Industry,           // Industry type
                    MarketCap = stock.Stock.MarketCap          // Market capitalization
                })

                // List mein convert kar rahe hain aur async tarike se DB se fetch kar rahe hain
                .ToListAsync();

            // Select ka kaam hai://

           // Har portfolio record ke andar jo Stock object linked hai, uska data nikalna.
            //Aur us data se ek naya Stock object banana jisme sirf tumhe chahiye wale fields rakho(Id, Symbol, CompanyName, etc.).
        }
    }
}
