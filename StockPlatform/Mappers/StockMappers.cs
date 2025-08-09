using StockPlatform.DTOS.Stock;
using StockPlatform.Models;

namespace StockPlatform.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock StockModel)
        {
            if (StockModel == null)
            {
                return null;
            }
            return new StockDto
            {
                Id = StockModel.Id,
                Symbol = StockModel.Symbol,
                CompanyName = StockModel.CompanyName,
                Purchase = StockModel.Purchase,
                LastDiv = StockModel.LastDiv,
                Industry = StockModel.Industry,
                MarketCap = StockModel.MarketCap,
                Comments = StockModel.Comments.Select(c => c.ToCommentDto()).ToList() // Assuming you have a ToCommentDto method in Comments mapper
            };
        }


        // dto -> model // jb data frontend sy a ra ho
        //create new object
        public static Stock ToStockFromCreateDto(this CreateStockDTO dto)
        {
            return new Stock
            {
                Symbol = dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDiv = dto.LastDiv,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap
            };


        }

        public static Stock UpdatestockfromDto(this Stock stock, UpdateFromCreateDto dto)
        {
            if (stock == null || dto == null)
            {
                return stock;
            }
            stock.Symbol = dto.Symbol;
            stock.CompanyName = dto.CompanyName;
            stock.Purchase = dto.Purchase;
            stock.LastDiv = dto.LastDiv;
            stock.Industry = dto.Industry;
            stock.MarketCap = dto.MarketCap;
            return stock;
        }
    }
}
