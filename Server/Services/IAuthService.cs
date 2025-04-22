using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Server.Services
{
    public interface IAuthService
    {
        Task<int> RegisterUser(RegisterDTO user);
        Task<bool> UserExists (string Email);
        Task<string?> LoginUser(UserLoginDTO userLoginDTO);
        Task<bool> ChangePassword(string userId, string newPassword);
        string GetUserId();
    }
}