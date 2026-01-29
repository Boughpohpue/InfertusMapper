namespace Infertus.Mapper;

public static class RTMapper
{
    public static void Register<TSource, TTarget>(Func<TSource, TTarget> map)
    {
        if (MapperRuntimeRegistry.Get(typeof(TSource), typeof(TTarget)) != null)
            throw new InvalidOperationException(
                $"Mapper already registered for {typeof(TSource).Name} -> {typeof(TTarget).Name}");

        MapperRuntimeRegistry.Register(new MapperDelegate<TSource, TTarget>(map));
    }

    public static TTarget Map<TTarget>(object source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var sourceType = source.GetType();
        var targetType = typeof(TTarget);

        var mapper = MapperRuntimeRegistry.Get(sourceType, targetType);

        if (mapper == null)
            throw new InvalidOperationException(
                $"No mapper registered for {sourceType.Name} -> {targetType.Name}");

        return ((dynamic)mapper).Map((dynamic)source);
    }
}
