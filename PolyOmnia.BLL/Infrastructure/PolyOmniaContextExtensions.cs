using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PolyOmnia.DAL.EF;

namespace PolyOmnia.BLL.Infrastructure
{
    public static class PolyOmniaContextExtensions
    {
        public static void AddPolyOmniaContext(this IServiceCollection services, string connection) =>
        services.AddDbContext<PolyOmniaContext>(options => options.UseSqlServer(connection));
    }
}
