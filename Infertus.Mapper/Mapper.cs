using Infertus.Mapper.Internal.Services;

namespace Infertus.Mapper;

public class Mapper : IMapper
{
    public TTarget Map<TTarget>(object source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var mapper = MappingsRegistry.Get(source.GetType(), typeof(TTarget));

        return mapper != null
            ? (TTarget)((dynamic)mapper).Map((dynamic)source)
            : throw new InvalidOperationException(
                $"Mapping {source.GetType().Name} -> {typeof(TTarget).Name} not found!");
    }
}
