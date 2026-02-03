using Infertus.Mapper.Internal;
using Infertus.Mapper.Internal.Services;
using System.Collections;

namespace Infertus.Mapper;

public class Mapper : IMapper
{
    public TTarget Map<TTarget>(object source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sourceType = source.GetType();
        var targetType = typeof(TTarget);

        var mapper = MappingsRegistry.Get(sourceType, targetType);
        if (mapper != null)
            return (TTarget)((dynamic)mapper).Map((dynamic)source);

        if (TryMapCollection(source, sourceType, targetType, out var result))
            return (TTarget)result!;

        throw new InvalidOperationException(
            $"Mapping {sourceType.Name} -> {targetType.Name} not found!");
    }

    private static bool TryMapCollection(
        object source,
        Type sourceType,
        Type targetType,
        out object? result)
    {
        result = null;

        if (!sourceType.IsEnumerable(out var sourceItemType) ||
            !targetType.IsEnumerable(out var targetItemType))
            return false;

        var itemMapper = MappingsRegistry.Get(sourceItemType, targetItemType);
        if (itemMapper == null)
            return false;

        var sourceEnumerable = (IEnumerable)source;

        var listType = typeof(List<>).MakeGenericType(targetItemType);
        var list = (IList)Activator.CreateInstance(listType)!;

        foreach (var item in sourceEnumerable)
        {
            var mappedItem = ((dynamic)itemMapper).Map((dynamic)item);
            list.Add(mappedItem);
        }

        if (targetType.IsArray)
        {
            var toArray = typeof(Enumerable)
                .GetMethod(nameof(Enumerable.ToArray))!
                .MakeGenericMethod(targetItemType);

            result = toArray.Invoke(null, [list]);
            return true;
        }

        if (targetType.IsAssignableFrom(listType))
        {
            result = list;
            return true;
        }

        var ctor = targetType.GetConstructor([typeof(IEnumerable<>).MakeGenericType(targetItemType)]);
        if (ctor != null)
        {
            result = ctor.Invoke([list]);
            return true;
        }

        return false;
    }

}
