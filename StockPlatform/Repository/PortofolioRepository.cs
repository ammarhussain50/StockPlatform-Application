using Microsoft.EntityFrameworkCore;
using StockPlatform.Data;
using StockPlatform.Interfaces;
using StockPlatform.Models;


namespace StockPlaform.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;

        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolio> CreatePortfolioAsync(Portfolio portfolio)
        {
            await _context.portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio;

        }

        public async Task<Portfolio> DeletePortfolioAsync(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());
            if (portfolioModel == null)
            {
                return null;
            }
            _context.portfolios.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;

        }




        public async Task<List<Stock>> GetUserPortfolioAsync(AppUser user)
        {

            // Portfolio table me se wo records dhoondh rahe hain jahan AppUserId == current user ka Id
            return await _context.portfolios
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
        }

    }
}