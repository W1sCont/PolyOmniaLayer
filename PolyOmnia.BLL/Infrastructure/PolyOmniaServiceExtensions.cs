using Microsoft.Extensions.DependencyInjection;
using PolyOmnia.BLL.Interfaces;
using PolyOmnia.BLL.Services;

namespace PolyOmnia.BLL.Infrastructure
{
    public static class PolyOmniaServiceExtensions
    {
        public static void AddPolyOmniaServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
