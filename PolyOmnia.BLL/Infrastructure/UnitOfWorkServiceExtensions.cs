using Microsoft.Extensions.DependencyInjection;
using PolyOmnia.DAL.Interfaces;
using PolyOmnia.DAL.Repositories;

namespace PolyOmnia.BLL.Infrastructure
{
    public static class UnitOfWorkServiceExtensions
    {
        public static void AddUnitOfWorkService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }
    }
}
