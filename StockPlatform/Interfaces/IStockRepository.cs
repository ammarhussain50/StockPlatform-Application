using StockPlatform.DTOS.Stock;
using StockPlatform.Helpers;
using StockPlatform.Models;

namespace StockPlatform.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateFromCreateDto stockDto);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> StockExist(int id);

    }
}
