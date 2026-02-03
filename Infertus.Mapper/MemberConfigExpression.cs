using Infertus.Mapper.Internal.Interfaces;
using Infertus.Mapper.Internal.Services;
using System.Linq.Expressions;

namespace Infertus.Mapper;

public sealed class MemberConfigExpression<TSource, TTarget, TMember>
{
    internal LambdaExpression? SourceExpression { get; private set; }

    public void MapFrom(Expression<Func<TSource, TMember>> source)
    {
        SourceExpression = source;
    }

    public void MapFrom<TSourceMember>(Expression<Func<TSource, TSourceMember>> source)
    {
        SourceExpression = BuildNestedExpression(source);
    }

    private static LambdaExpression BuildNestedExpression<TSourceMember>(
        Expression<Func<TSource, TSourceMember>> source)
    {
        var mapper = MappingsRegistry.Get(typeof(TSourceMember), typeof(TMember));
        if (mapper == null)
            throw new InvalidOperationException(
                $"No mapping registered for {typeof(TSourceMember).Name} -> {typeof(TMember).Name}");

        var srcParam = source.Parameters[0];
        var sourceValue = source.Body;

        var mapMethod = mapper.GetType()
            .GetMethod(nameof(IMap<TSourceMember, TMember>.Map))!;

        var mapCall =
            Expression.Call(
                Expression.Constant(mapper),
                mapMethod,
                sourceValue
            );

        var nullCheck =
            Expression.Equal(
                sourceValue,
                Expression.Constant(null, typeof(TSourceMember))
            );

        var body =
            Expression.Condition(
                nullCheck,
                Expression.Default(typeof(TMember)),
                mapCall
            );

        return Expression.Lambda(body, srcParam);
    }
}
