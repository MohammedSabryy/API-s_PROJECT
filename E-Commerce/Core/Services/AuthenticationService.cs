global using Domain.Entities;
global using Microsoft.AspNetCore.Identity;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AuthenticationService(UserManager<User> UserManager , IOptions<JwtOptions> options ,IMapper mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailExistAsync(string Email)
        {
            var user = await UserManager.FindByEmailAsync(Email);
            return user != null;
        }

        public async Task<AddressDTO> GetUserAddress(string email)
        {
            var user = await UserManager.Users.Include(x => x.Address).FirstOrDefaultAsync(u => u.Email == email) ?? throw new UserNotFoundEception(email);
            return mapper.Map<AddressDTO>(user.Address);
        }

        public async Task<UserResultDTO> GetUserByEmail(string email)
        {
            var user =  await UserManager.FindByEmailAsync(email)??throw new UserNotFoundEception(email);

            return new UserResultDTO(user.DisplayName, user.Email,await CreateTokenAsync(user));
        }

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

        public async Task<AddressDTO> UpdateUserAddressAsync(AddressDTO address , string email)
        {
            var user = await UserManager.Users.Include(x=>x.Address).FirstOrDefaultAsync(u=>u.Email== email)??
                throw new UserNotFoundEception(email); 
            //var userAddress= mapper.Map<Address>(address);
            if (user.Address != null)
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;


            }
            else
            {
                var userAddress = mapper.Map<Address>(address);
                user.Address = userAddress;
            }
            await UserManager.UpdateAsync(user);
            return address;
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
