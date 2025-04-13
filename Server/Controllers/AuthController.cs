using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Shared;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        protected IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
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
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO){
            var result = await _authService.LoginUser(userLoginDTO);

            if(result == null){
                return Unauthorized("Email ou senha invalidos");
            }

            return Ok(result);
        }

    }
}