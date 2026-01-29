namespace Infertus.Mapper;

internal sealed class MapperDelegate<TSource, TTarget> : IMapper<TSource, TTarget>
{
    private readonly Func<TSource, TTarget> _map;

    public MapperDelegate(Func<TSource, TTarget> map)
    {
        _map = map ?? throw new ArgumentNullException(nameof(map));
    }

    public TTarget Map(TSource source) => _map(source);
}
