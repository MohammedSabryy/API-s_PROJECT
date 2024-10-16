global using Domain.Entities;
global using Microsoft.AspNetCore.Identity;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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
                "Token"
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
                "Token"
                );
        }
    }
}
