using Microsoft.AspNetCore.Identity;
using Todo_List_API.Data;
using Todo_List_API.Data.Contexts;
using Todo_List_API.Data.Entity;
using Todo_List_API.Services.Auth;
using Todo_List_API.Services.Auth.Interfaces;
using Todo_List_API.Utils.JWT;

namespace Todo_List_API.Infra
{
    public static class DependencyInjection
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            //Data
            services.AddScoped<SecurityDbContext>();
            services.AddScoped<AppDbContext>();

            //Services
            services.AddScoped<IAuthService, AuthService>();
            
            //Configs
            services.AddScoped<UserManager<User>>();
            services.AddScoped<UnityOfWork>();
            services.AddScoped<JwtUtils>();

            return services;
        }
    }
}
