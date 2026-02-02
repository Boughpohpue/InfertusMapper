namespace Infertus.Mapper.Internal.Interfaces;

public interface IMap<in TSource, out TTarget>
{
    TTarget Map(TSource source);
}
