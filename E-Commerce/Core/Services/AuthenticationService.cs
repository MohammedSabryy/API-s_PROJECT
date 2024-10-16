﻿global using Domain.Entities;
global using Microsoft.AspNetCore.Identity;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
    internal class AuthenticationService(UserManager<User> UserManager , IOptions<JwtOptions> options) : IAuthenticationService
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
            var jwtOptions = options.Value; 
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)

            };
            var roles = await UserManager.GetRolesAsync(user);
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role,role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var signingCreds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                audience: jwtOptions.Audience,
                issuer: jwtOptions.Issure,
                expires:DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                claims:authClaims,
                signingCredentials:signingCreds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
