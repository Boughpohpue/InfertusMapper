namespace Infertus.Mapper;

internal static class MappingsRuntimeRegistry
{
    private static readonly Dictionary<(Type Source, Type Target), object> _mappers = [];

    public static void Register<TSource, TTarget>(IMapper<TSource, TTarget> mapper)
    {
        _mappers[(typeof(TSource), typeof(TTarget))] = mapper;
    }

    public static object? Get(Type sourceType, Type targetType)
    {
        _mappers.TryGetValue((sourceType, targetType), out var mapper);
        return mapper;
    }
}
