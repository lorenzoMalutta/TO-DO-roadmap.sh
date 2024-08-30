using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using Todo_List_API.Commands.Requests.Auth;
using Todo_List_API.Data;
using Todo_List_API.Data.Entity;
using Todo_List_API.Services.Auth.Interfaces;
using Todo_List_API.Utils.JWT;

namespace Todo_List_API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private UnityOfWork _unityOfWork;
        private readonly JwtUtils _jwtUtils;


        public AuthService(UserManager<User> userManager, UnityOfWork unityOfWork, JwtUtils jwtUtils)
        {
            _userManager = userManager;
            _unityOfWork = unityOfWork;
            _jwtUtils = jwtUtils;
        }

        public async Task<ServiceResponse> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if (user == null)
                return ServiceResponse.Factory(false, "Email not found!", HttpStatusCode.NotFound, null);

            var result = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (!result)
                return ServiceResponse.Factory(false, "Invalid login!", HttpStatusCode.Unauthorized, null);

            _unityOfWork.CommitSecurity();

            var claim = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaims(claim);

            var token = _jwtUtils.GetTokenData(user.Email, claimsIdentity);

            return ServiceResponse.Factory(true, "Login successful!", HttpStatusCode.OK, token);
        }

        public async Task<ServiceResponse> Register(RegisterRequest registerRequest)
        {
            var hasEmail = await _userManager.FindByEmailAsync(registerRequest.Email);

            if (hasEmail != null)
                return ServiceResponse.Factory(false, "Email already exist!", HttpStatusCode.Forbidden, null);

            var user = new User
            {
                Email = registerRequest.Email,
                Name = registerRequest.Name,
                UserName = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
                return ServiceResponse.Factory(false, "Failed to create user!", HttpStatusCode.InternalServerError, null);

            _unityOfWork.CommitSecurity();

            return ServiceResponse.Factory(true, "User created successfully!", HttpStatusCode.Created, null);
        }
    }
}
