using Microsoft.Extensions.DependencyInjection;

namespace Infertus.Mapper;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddSingleton<IMapper, Mapper>();

        return services;
    }

    public static IServiceCollection AddMapper<TProfile>(this IServiceCollection services)
        where TProfile : MappingProfile, new()
    {
        services.AddMapper();
        services.AddMappingProfile<TProfile>();
        
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services, params Type[] profileTypes)
    {
        services.AddMapper();
        services.AddMappingProfiles(profileTypes);

        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services, Action<MapperConfigExpression> config)
    {
        services.AddMapper();

        config(new MapperConfigExpression());

        return services;
    }

    public static IServiceCollection AddMappingProfile<TProfile>(this IServiceCollection services)
        where TProfile : MappingProfile, new()
    {
        new MapperConfigExpression().AddProfile<TProfile>();

        return services;
    }

    public static IServiceCollection AddMappingProfiles(this IServiceCollection services, params Type[] profileTypes)
    {
        var config = new MapperConfigExpression();

        foreach (var type in profileTypes)
            config.AddProfile(type);

        return services;
    }
}
