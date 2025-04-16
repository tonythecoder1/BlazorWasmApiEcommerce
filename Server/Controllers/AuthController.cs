using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        protected IAuthService _authService;
        protected UserManager<MyUser> _userManager;
        public AuthController(IAuthService authService, UserManager<MyUser> userManager)
        {
            this._authService = authService;
            this._userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO userDto)
        {

            var result = await _authService.RegisterUser(userDto);

            if (result == 1)
            {

                return Ok(result);

            }
            else if (result == 0)
            {
                return Conflict(result);
            }
            else
            {
                return BadRequest("Erro ao criar utilizador");
            }

        }

        //api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var result = await _authService.LoginUser(userLoginDTO);

            if (result == null)
            {
                return Unauthorized("Email ou senha invalidos");
            }

            return Ok(result);
        }
        
        [HttpPost("change")]
        public async Task<ActionResult<bool>> ChangePassword([FromBody] UserChangePassword userDto)
        {
            var userId = User.FindFirstValue("nameid");

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _authService.ChangePassword(userId, userDto.Password);

            // Retorne explicitamente como JSON
            return new JsonResult(result) { ContentType = "application/json" };
        }



    }
}