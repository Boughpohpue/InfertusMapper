using Infertus.Mapper.Internal.Services;

namespace Infertus.Mapper
{
    public sealed class MapperConfigExpression
    {
        public void AddProfile<TProfile>() where TProfile : MappingProfile, new()
        {
            var profile = new TProfile();

            MappingsRegistry.Compile();
        }

        public void AddProfile(Type profileType)
        {
            if (!typeof(MappingProfile).IsAssignableFrom(profileType))
                throw new ArgumentException($"{profileType.Name} is not of {nameof(MappingProfile)} type!");

            var profile = (MappingProfile)Activator.CreateInstance(profileType)!;

            MappingsRegistry.Compile();
        }
    }
}
