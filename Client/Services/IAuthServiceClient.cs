using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared;

namespace Client.Services
{
    public interface IAuthServiceClient
    {
        public Task<string> Register(RegisterDTO userDto);
        public Task<string> LoginUser(UserLoginDTO userLoginDTO);
        public Task<bool> ChangePassword(UserChangePassword password);
    }
}