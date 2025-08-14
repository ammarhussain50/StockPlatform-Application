using StockPlatform.Models;

namespace StockPlatform.Interfaces
{
    public interface ITokenService
    {
        //create a method to generate a token
        string CreateToken(AppUser user);
    }
}
