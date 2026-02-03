using Infertus.Mapper.Internal.Models;
using Infertus.Mapper.Internal.Services;

namespace Infertus.Mapper;

public abstract class MappingProfile 
{
    protected static void RegisterMapping<TSource, TTarget>(Func<TSource, TTarget> map)
    {
        MappingsRegistry.Register(new MappingDelegate<TSource, TTarget>(map));
    }

    protected static MappingExpression<TSource, TTarget> CreateMap<TSource, TTarget>()
    {
        if (typeof(TTarget).GetConstructor(Type.EmptyTypes) == null)
            throw new ArgumentException($"{typeof(TTarget).Name} must have a parameterless constructor");

        var map = MappingsRegistry.Add<TSource, TTarget>();

        map.Members.AddRange(AutoMapper.GetMapping<TSource, TTarget>());

        return new MappingExpression<TSource, TTarget>(map);
    }
}
