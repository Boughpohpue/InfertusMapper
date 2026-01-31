namespace Infertus.Mapper.Internal;

public interface IMap<in TSource, out TTarget>
{
    TTarget Map(TSource source);
}
