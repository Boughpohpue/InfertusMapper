using Infertus.Mapper.Internal.Interfaces;

namespace Infertus.Mapper.Internal.Models;

internal sealed class MappingDelegate<TSource, TTarget>(Func<TSource, TTarget> map) : IMap<TSource, TTarget>
{
    private readonly Func<TSource, TTarget> _map = map ?? throw new ArgumentNullException(nameof(map));

    public TTarget Map(TSource source) => _map(source);
}
