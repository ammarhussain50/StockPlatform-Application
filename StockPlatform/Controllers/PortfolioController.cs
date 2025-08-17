using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockPlaform.Extensions;
using StockPlatform.Interfaces;
using StockPlatform.Models;

namespace StockPlatform.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(

            UserManager<AppUser> userManager,
            IStockRepository stockRepository,
            IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepository;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {

            //  JWT token me se username nikalna using custom ClaimsExtension method
            var username = User.GetUsername();

            //  Identity system ka UserManager use karke user ko database se find karna
            var appUser = await _userManager.FindByNameAsync(username);

            //FindByNameAsync yeh method database me AspNetUsers table (ya jo Identity ke users ka table ho) me UserName = 'shahzeel123' ko search karega.

            var UserPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            return Ok(UserPortfolio);


        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            //find user from JWT token claims
            var username = User.GetUsername();


            //  Identity system ka UserManager use karke user ko database se find karna
            var appUser = await _userManager.FindByNameAsync(username);

            // find stock by symbol
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null)
            {
                return NotFound("Stock not found");
            }

            // Get the user's portfolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);

            // Check if the stock already exists in the user's portfolio
            if (userPortfolio.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already exists in your portfolio can't add same stock");
            }
            // Create a new portfolio item
            var portfolioModel = new Portfolio
            {
                AppUserId = appUser.Id,
                StockId = stock.Id,
            };
            // Save the portfolio item to the database
            await _portfolioRepo.CreatePortfolioAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return BadRequest("Failed to add stock to portfolio");
            }
            else
            {
                return Created(); // 201 Created response
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            // Get the username from JWT token claims
            var username = User.GetUsername();

            var appUser = await _userManager.FindByNameAsync(username);

            // get user portfolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolioAsync(appUser);


            //filter the user's portfolio to find the stock with the specified symbol

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1)
            // Agar sirf 1 match mila to delete karein, warna error return karein
            // Ye check is liye hai taake duplicate ya missing stocks ko avoid kiya ja sake
            {
                await _portfolioRepo.DeletePortfolioAsync(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not found in portfolio.");
            }
            return Ok();
        }












    }
}