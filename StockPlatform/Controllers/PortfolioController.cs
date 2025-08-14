using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockPlatform.Extensions;
using StockPlatform.Interfaces;
using StockPlatform.Models;

namespace StockPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> usermanager;
        private readonly IStockRepository stockrepo;
        private readonly IPortfolioRepository portfoliorepo;

        public PortfolioController(UserManager<AppUser> usermanager , IStockRepository stockrepo , IPortfolioRepository portfoliorepo)
        {
            this.usermanager = usermanager;
            this.stockrepo = stockrepo;
            this.portfoliorepo = portfoliorepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            //  JWT token me se username nikalna using custom ClaimsExtension method
            var username = User.GetUsername();

            //  Identity system ka UserManager use karke user ko database se find karna
            var appUser = await usermanager.FindByNameAsync(username);

            //FindByNameAsync yeh method database me AspNetUsers table (ya jo Identity ke users ka table ho) me UserName = 'shahzeel123' ko search karega.

            var UserPortfolio = await portfoliorepo.GetUserPortfolio(appUser);
            if (UserPortfolio == null) { 
                return NotFound();
                     }

            return Ok(UserPortfolio);

        }

    }
}
