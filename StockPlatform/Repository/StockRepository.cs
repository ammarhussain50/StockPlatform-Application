using Microsoft.EntityFrameworkCore;
using StockPlatform.Data;
using StockPlatform.DTOS.Stock;
using StockPlatform.Helpers;
using StockPlatform.Interfaces;
using StockPlatform.Mappers;
using StockPlatform.Models;

namespace StockPlatform.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext context;

        public StockRepository(ApplicationDBContext context)
        {
            this.context = context;
        }
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await context.stocks.AddAsync(stockModel);
            await context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await context.stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            context.stocks.Remove(stockModel);
            await context.SaveChangesAsync();
            return stockModel;
        }




        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            // Ensure valid pagination defaults
            if (query.PageNumber <= 0)
                query.PageNumber = 1;

            if (query.PageSize <= 0)
                query.PageSize = 20;

            // Base query with comments included
            var stocks = context.stocks
                .Include(c => c.Comments)
                .AsQueryable();

            // Case-insensitive filtering for CompanyName
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                var name = query.CompanyName.ToLower();
                stocks = stocks.Where(s => s.CompanyName.ToLower().Contains(name));
            }

            // Case-insensitive filtering for Symbol
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                var symbol = query.Symbol.ToLower();
                stocks = stocks.Where(s => s.Symbol.ToLower().Contains(symbol));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending
                        ? stocks.OrderByDescending(s => s.Symbol)
                        : stocks.OrderBy(s => s.Symbol);
                }
                // Add more sort options if needed
            }
            else
            {
                // Default sort by latest stock
                stocks = stocks.OrderByDescending(s => s.Id);
            }

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks
                .Skip(skipNumber)
                .Take(query.PageSize)
                .ToListAsync();
           

        }






        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await context.stocks.Include(c => c.Comments).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> StockExist(int id)
        {
            return await context.stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateFromCreateDto stockDto)
        {
            var existngStock = context.stocks.FirstOrDefault(s => s.Id == id);
            if (existngStock == null)
            {
                return null;
            }
            existngStock = existngStock.UpdatestockfromDto(stockDto);
            await context.SaveChangesAsync();
            return existngStock;
        }
    }
}
