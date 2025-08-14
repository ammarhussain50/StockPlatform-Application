using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPlatform.DTOS.Account;
using StockPlatform.Interfaces;
using StockPlatform.Models;
using System;
using System.Threading.Tasks;

namespace StockPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _Siginmanager;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService , SignInManager<AppUser> siginmanager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _Siginmanager = siginmanager;
        }

       

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // User ko database se fetch karo (username lowercase compare karo for case-insensitive matching)
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.UserName.ToLower() == login.UserName.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var Result = await _Siginmanager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!Result.Succeeded)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = register.UserName,
                    Email = register.Email
                };

                var createUserResult = await _userManager.CreateAsync(appUser, register.Password);

                if (!createUserResult.Succeeded)
                    return StatusCode(500, createUserResult.Errors);

                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                if (!roleResult.Succeeded)
                    return StatusCode(500, roleResult.Errors);

                return Ok(new NewUserDto
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = _tokenService.CreateToken(appUser)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}

