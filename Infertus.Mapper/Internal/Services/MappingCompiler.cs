using Infertus.Mapper.Internal.Models;
using System.Linq.Expressions;

namespace Infertus.Mapper.Internal.Services;

internal static class MappingCompiler
{
    public static Func<TSource, TTarget> Compile<TSource, TTarget>(TypeMap map)
    {
        var source = Expression.Parameter(typeof(TSource), "src");

        var bindings = map.Members
            .Where(m => !map.IgnoredMembers.Contains(m.Target))
            .Select(m =>
                Expression.Bind(
                    m.Target,
                    Expression.Invoke(m.Source, source)
                )
            );

        var body = Expression.MemberInit(
            Expression.New(typeof(TTarget)),
            bindings
        );

        return Expression
            .Lambda<Func<TSource, TTarget>>(body, source)
            .Compile();
    }
}
