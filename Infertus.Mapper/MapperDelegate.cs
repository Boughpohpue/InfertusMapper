namespace Infertus.Mapper;

internal sealed class MapperDelegate<TSource, TTarget>(Func<TSource, TTarget> map) : IMapper<TSource, TTarget>
{
    private readonly Func<TSource, TTarget> _map = map ?? throw new ArgumentNullException(nameof(map));

    public TTarget Map(TSource source) => _map(source);
}
