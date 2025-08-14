using System.ComponentModel.DataAnnotations.Schema;

namespace StockPlatform.Models
{
    [Table("Portfolio")]
    public class Portfolio
    {
        //  Ye Portfolio ek join table ka role play karti hai:
        // Ek User ke paas multiple Stocks ho sakte hain
        // Aur ek Stock multiple Users ke portfolio mein ho sakta hai
        // Is many-to-many relationship ko EF Core samajhne ke liye beech mein yeh Portfolio entity zaroori hoti hai


        // Foreign key - ye batata hai ke kis user ka ye portfolio item hai
        public string AppUserId { get; set; }

        // Foreign key - ye batata hai ke portfolio kis stock se linked hai
        public int StockId { get; set; }

        // Navigation property - isse Stock object ka full data milega (e.g. CompanyName, Symbol etc.)
        public Stock Stock { get; set; }

        // Navigation property - isse User ka full object milega (AppUser se)
        public AppUser AppUser { get; set; }
    }
}
