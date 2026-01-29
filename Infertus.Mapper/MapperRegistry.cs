namespace Infertus.Mapper;

internal static class MapperRegistry<TSource, TTarget>
{
    public static IMapper<TSource, TTarget> Instance { get; set; } = default!;
}
