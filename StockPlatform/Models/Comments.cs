using System.ComponentModel.DataAnnotations.Schema;

namespace StockPlatform.Models
{
    [Table("Comments")]
    public class Comments
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }
        // Ye line represent karti hai Foreign Key.
        //StockId ek number hai jo bataata hai ke yeh comment kis stock se related hai.
        //StockId = 1  // This comment is about the stock with Id = 1

        public Stock? Stock { get; set; }
        //        Ye line ek Navigation Property hai.
        //Is se hume poora Stock object mil jata hai jis se yeh comment related hai.
        //        Ye line ek Navigation Property hai.
        //Is se hume poora Stock object mil jata hai jis se yeh comment related hai.
    }
}
