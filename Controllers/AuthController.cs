using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo_List_API.Commands.Requests.Auth;
using Todo_List_API.Services.Auth.Interfaces;
using System.Net;

namespace Todo_List_API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.Register(request);

            if (result.HttpResponse != HttpStatusCode.Created) 
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request);

            if (result.HttpResponse != HttpStatusCode.OK)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
