using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestV1Vibe.Application.Services.Mapper;

namespace TestV1Vibe.Application;

public static class DependencyinjectionExtesion
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddScoped(options => new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new ApplicationMappingProfile());
        }).CreateMapper());
    }
}
