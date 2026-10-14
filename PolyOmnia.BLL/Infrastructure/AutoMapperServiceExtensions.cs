using Microsoft.Extensions.DependencyInjection;
using PolyOmnia.BLL.Profiles;

namespace PolyOmnia.BLL.Infrastructure;

public static class AutoMapperServiceExtensions
{
    public static void AddPolyOmniaAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<PolyOmniaProfile>();
        });
    }
}
