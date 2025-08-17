using StockPlatform.DTOS.Stock;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace StockPlatform.Models
{
    [Table("Stock")]
    public class Stock
    {
        public int Id { get; set; }

        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty; //not null 

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        [Column(TypeName = "decimal(18,2)")] // it accepts total  max 18 digits, with 2 decimal (16 before . 2 after .) places eg: 100.00,
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

        public List<Comments> Comments { get; set; } = new List<Comments>(); // // This list stores objects of the Comment class

        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>(); // This list stores objects of the Portfolio class
        //internal void UpdateFromCreateDto(UpdateFromCreateDto dto)
        //{
        //    throw new NotImplementedException();
        //}

        //internal object ToStockDto()
        //{
        //    throw new NotImplementedException();
        //}
        //one to many relationship a Stock has many comments 



    }
}
