using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fridge_BackEnd.Data;
using Fridge_BackEnd.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Fridge_BackEnd.Features.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly FridgeContext _db;

        public TokenController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration, FridgeContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginViewModel model)
        {
            var errorMessage = "Invalid credentials";

            if (!ModelState.IsValid)
                return BadRequest(errorMessage);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return BadRequest(errorMessage);

            if (await _userManager.IsLockedOutAsync(user))
                return BadRequest(errorMessage);

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (!result.Succeeded)
                return BadRequest(errorMessage);

            var token = GenerateToken(user);
            Console.WriteLine(token);
            return Ok(token);
        }

        private TokenViewModel GenerateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Authentication:JwtExpireDays"]));

            var token = new JwtSecurityToken(
              _configuration["Authentication:JwtIssuer"],
              _configuration["Authentication:JwtAudience"],
              claims,
              expires: expires,
              signingCredentials: creds
            );

            return new TokenViewModel
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                AccessTokenExpiration = expires,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Email = user.Email
            };
        }
    }
}
