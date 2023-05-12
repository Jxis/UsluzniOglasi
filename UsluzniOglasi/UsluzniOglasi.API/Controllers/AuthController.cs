using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsluzniOglasi.Application.Models;
using UsluzniOglasi.Application.Services;

namespace UsluzniOglasi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authService.Login(userLoginModel))
                return BadRequest(ModelState);

            return Ok("Logged in successfully!");
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authService.Register(userRegisterModel))
                return BadRequest(ModelState);

            return Ok("Registered successfully!");
        }
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Register()
        {
            if (!await _authService.Logout())
                return BadRequest(ModelState);

            return Ok("Logged out successfully!");
        }
    }
}
