namespace Infertus.Mapper;

public interface IMapper
{
    TTarget Map<TTarget>(object source);
}
