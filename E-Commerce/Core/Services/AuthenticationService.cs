global using Domain.Entities;
global using Microsoft.AspNetCore.Identity;
using Domain.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AuthenticationService(UserManager<User> UserManager) : IAuthenticationService
    { 
        public async Task<UserResultDTO> LoginAsync(LoginDTO loginModel)
        {
            var user = await UserManager.FindByEmailAsync(loginModel.Email);
            if (user == null) throw new UnAuthorizedException("Email Doesn't Exist");
            var result = await UserManager.CheckPasswordAsync(user,loginModel.Password);
            if(!result) throw new UnAuthorizedException();
            return new UserResultDTO
                (
                user.DisplayName,
                user.Email,
                await CreateTokenAsync(user)
                );
            
        }

        public async Task<UserResultDTO> RegisterAsync(UserRegisterDTO registerModel)
        {
            var user = new User()
            {
                DisplayName = registerModel.DisplayName,
                Email = registerModel.Email,
                PhoneNumber = registerModel.PhoneNumber,
                UserName = registerModel.UserName
            };
            var result = await UserManager.CreateAsync(user,registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e=>e.Description).ToList();
                throw new ValidationException(errors);
            }
            return new UserResultDTO
                (
                user.DisplayName,
                user.Email,
                await CreateTokenAsync(user)
                );
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)

            };
            var roles = await UserManager.GetRolesAsync(user);
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role,role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x+7hPN9lfVvXdTYknzokbkRy7JsLHCe8FeXcF3hQe5U="));
            var signingCreds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                audience: "MyAudience",
                issuer: "https://localhost:5001",
                expires:DateTime.UtcNow.AddDays(30),
                claims:authClaims,
                signingCredentials:signingCreds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
