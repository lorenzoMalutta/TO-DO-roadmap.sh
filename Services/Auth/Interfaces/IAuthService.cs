using Todo_List_API.Commands.Requests.Auth;
namespace Todo_List_API.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse> Login(LoginRequest loginRequest);
        Task<ServiceResponse> Register(RegisterRequest registerRequest);
    }
}
