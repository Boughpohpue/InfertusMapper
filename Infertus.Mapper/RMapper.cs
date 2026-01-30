namespace Infertus.Mapper;

public static class RMapper
{
    public static void Register<TSource, TTarget>(Func<TSource, TTarget> map)
    {
        if (MappingsRuntimeRegistry.Get(typeof(TSource), typeof(TTarget)) != null)
            throw new InvalidOperationException(
                $"Mapper already registered for {typeof(TSource).Name} -> {typeof(TTarget).Name}");

        MappingsRuntimeRegistry.Register(new MapperDelegate<TSource, TTarget>(map));
    }

    public static TTarget Map<TTarget>(object source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var sourceType = source.GetType();
        var targetType = typeof(TTarget);

        var mapper = MappingsRuntimeRegistry.Get(sourceType, targetType);

        return mapper != null
            ? (TTarget)((dynamic)mapper).Map((dynamic)source)
            : throw new InvalidOperationException(
                $"No mapper registered for {sourceType.Name} -> {targetType.Name}");
    }
}
