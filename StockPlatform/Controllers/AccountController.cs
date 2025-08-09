using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockPlatform.Models;

namespace StockPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
    }
}
