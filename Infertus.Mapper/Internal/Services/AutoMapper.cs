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
            if (!sourceMembers.TryGetValue(targetProp.Name, out var sourceProp))
                continue;

            var srcType = sourceProp.PropertyType;
            var tgtType = targetProp.PropertyType;

            var srcParam = Expression.Parameter(typeof(TSource), "src");
            var srcAccess = Expression.Property(srcParam, sourceProp);

            if (srcType == tgtType)
            {
                yield return new MemberExpressionMap(
                    targetProp,
                    Expression.Lambda(srcAccess, srcParam)
                );
                continue;
            }

            if (srcType.IsEnumerable(out var srcItem) &&
                tgtType.IsEnumerable(out var tgtItem))
            {
                var mapper = MappingsRegistry.Get(srcItem, tgtItem);
                if (mapper == null)
                    continue;

                var lambda = BuildCollectionMappingLambda(
                    srcAccess,
                    srcItem,
                    tgtItem,
                    srcParam,
                    tgtType
                );

                yield return new MemberExpressionMap(targetProp, lambda);
            }
        }
    }

    private static LambdaExpression BuildCollectionMappingLambda(
        Expression sourceCollection,
        Type sourceItemType,
        Type targetItemType,
        ParameterExpression srcParam,
        Type targetCollectionType)
    {
        var mapper = MappingsRegistry.Get(sourceItemType, targetItemType)!;
        var mapMethod = mapper.GetType().GetMethod("Map")!;

        var itemParam = Expression.Parameter(sourceItemType, "x");

        var mapCall = Expression.Call(
            Expression.Constant(mapper),
            mapMethod,
            itemParam
        );

        var selector = Expression.Lambda(mapCall, itemParam);

        var selectMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "Select" && m.GetParameters().Length == 2)
            .MakeGenericMethod(sourceItemType, targetItemType);

        var selectCall = Expression.Call(
            selectMethod,
            sourceCollection,
            selector
        );

        Expression result = selectCall;

        if (targetCollectionType.IsArray)
        {
            var toArray = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToArray))!
                .MakeGenericMethod(targetItemType);

            result = Expression.Call(toArray, selectCall);
        }
        else if (targetCollectionType.IsGenericType &&
                 targetCollectionType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var toList = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToList))!
                .MakeGenericMethod(targetItemType);

            result = Expression.Call(toList, selectCall);
        }

        var body = Expression.Condition(
            Expression.Equal(
                sourceCollection,
                Expression.Constant(null, sourceCollection.Type)
            ),
            Expression.Default(targetCollectionType),
            result
        );

        return Expression.Lambda(body, srcParam);
    }


}
