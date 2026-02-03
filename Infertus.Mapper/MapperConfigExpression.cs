using Infertus.Mapper.Internal.Services;

namespace Infertus.Mapper;

public sealed class MapperConfigExpression
{
    internal readonly List<MappingProfile> Profiles = [];

    public void AddProfile<TProfile>() where TProfile : MappingProfile, new()
    {
        Add(new TProfile());
    }

    public void AddProfile(Type profileType)
    {
        if (!typeof(MappingProfile).IsAssignableFrom(profileType))
            throw new ArgumentException($"{profileType.Name} is not of {nameof(MappingProfile)} type!");

        var profile = (MappingProfile)Activator.CreateInstance(profileType)!;

        Add(profile);
    }

    private void Add<TProfile>(TProfile profile) where TProfile : MappingProfile
    {
        Profiles.Add(profile);

        MappingsRegistry.Compile();
    }
}
