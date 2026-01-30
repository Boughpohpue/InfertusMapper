using Microsoft.Extensions.DependencyInjection;

namespace Infertus.Mapper;

public static class MappingServiceCollectionExtension
{
    public static IServiceCollection AddMappingProfile<TProfile>(this IServiceCollection services)
        where TProfile : Profile, new()
    {
        services.AddSingleton<Profile>(new TProfile());

        return services;
    }
}
