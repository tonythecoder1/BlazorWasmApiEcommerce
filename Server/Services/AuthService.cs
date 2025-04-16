using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;


namespace Server.Services
{
    public class AuthService : IAuthService
    {

        protected readonly DbContextServer _dbContextServer;
        protected readonly UserManager<MyUser> _userManager;
        protected readonly IConfiguration _iconfiguration;
        public AuthService(DbContextServer contextServer, UserManager<MyUser> userManager, IConfiguration _iconfig)
        {
            this._dbContextServer = contextServer;
            this._userManager = userManager;
            this._iconfiguration = _iconfig;
        }

        public async Task<string?> LoginUser(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);

            if (user == null)
                return null;

            var result = await _userManager.CheckPasswordAsync(user, userLoginDTO.Password);

            if (!result)
                return null;

            return CreateToken(user);
        }


        public string CreateToken(MyUser user)
        {

            List<Claim> claims = new List<Claim>{

            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name,user.Email),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public async Task<int> RegisterUser(RegisterDTO userDto)
        {
            if (await UserExists(userDto.Email))
            {
                return 0;
            }

            var user = new MyUser
            {
                UserName = userDto.Email,
                Email = userDto.Email,
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                return 1;
            }

            return -1;
        }

        public async Task<bool> UserExists(string Email)

        {

            if (await _userManager.FindByEmailAsync(Email) != null)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ChangePassword(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.Succeeded;
        }

    }
}