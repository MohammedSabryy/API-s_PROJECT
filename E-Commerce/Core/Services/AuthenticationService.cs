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

        public Task<UserResultDTO> RegisterAsync(UserRegisterDTO RegisterModel)
        {
            throw new NotImplementedException();
        }
    }
}
