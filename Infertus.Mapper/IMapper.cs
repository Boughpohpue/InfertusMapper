namespace Infertus.Mapper;

public interface IMapper<in TSource, out TTarget>
{
    TTarget Map(TSource source);
}
