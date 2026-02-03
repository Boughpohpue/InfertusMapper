using Infertus.Mapper.Internal.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Formats.Tar;

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
        services.AddMappingProfile<TProfile>();

        services.AddMapper();

        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services, params Type[] profileTypes)
    {
        services.AddMappingProfiles(profileTypes);

        services.AddMapper();

        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services, Action<MapperConfigExpression> config)
    {
        config(new MapperConfigExpression());

        services.AddMapper();

        return services;
    }

    public static IServiceCollection AddMappingProfile<TProfile>(this IServiceCollection services)
        where TProfile : MappingProfile, new()
    {
        //services.AddSingleton<MappingProfile>(new TProfile());
        new MapperConfigExpression().AddProfile<TProfile>();

        //MappingsRegistry.Compile();

        return services;
    }

    public static IServiceCollection AddMappingProfiles(this IServiceCollection services, params Type[] profileTypes)
    {
        var config = new MapperConfigExpression();
        foreach (var type in profileTypes)
        {
            config.AddProfile(type);

            //if (!typeof(MappingProfile).IsAssignableFrom(type))
            //    throw new ArgumentException($"{type.Name} is not of {nameof(MappingProfile)} type!");

            //var profile = (MappingProfile)Activator.CreateInstance(type)!;

            //services.AddSingleton(typeof(MappingProfile), profile);
            //MappingsRegistry.Compile();
        }

        return services;
    }
}
