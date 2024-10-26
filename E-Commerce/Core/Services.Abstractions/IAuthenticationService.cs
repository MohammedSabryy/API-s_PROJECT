using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthenticationService
    {
        public Task<UserResultDTO> LoginAsync(LoginDTO loginModel);
        public Task<UserResultDTO> RegisterAsync(UserRegisterDTO RegisterModel);

        public Task<bool> CheckEmailExistAsync(string Email);
        public Task<AddressDTO> GetUserAddress(string email);
        public Task<AddressDTO> UpdateUserAddressAsync(AddressDTO address , string email);
        public Task<UserResultDTO> GetUserByEmail(string email);


    }
}
