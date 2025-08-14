using StockPlatform.Models;

namespace StockPlatform.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfolio(AppUser appUser);
    }
}
