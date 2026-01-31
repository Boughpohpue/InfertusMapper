using Infertus.Mapper.Internal.Models;
using System.Linq.Expressions;

namespace Infertus.Mapper.Internal.Services;

internal static class AutoMapper
{
    public static IEnumerable<MemberExpressionMap> GetMapping<TSource, TTarget>()
    {
        var sourceMembers = typeof(TSource).GetReadablePropertiesDict();
        foreach (var targetProp in typeof(TTarget).GetWritableProperties())
        {
            if (!sourceMembers.TryGetValue(targetProp.Name, out var sourceProp)
                || sourceProp.PropertyType != targetProp.PropertyType)
                continue;

            var srcParam = Expression.Parameter(typeof(TSource), "src");
            var body = Expression.Property(srcParam, sourceProp);
            var lambda = Expression.Lambda(body, srcParam);

            yield return new MemberExpressionMap(targetProp, lambda);
        }
    }
}
