namespace Infertus.Mapper;

internal static class MappingsRegistry<TSource, TTarget>
{
    public static IMapper<TSource, TTarget> Instance { get; set; } = default!;
}
