namespace Infertus.Mapper;

public static class Mapper
{
    public static void Register<TSource, TTarget>(Func<TSource, TTarget> map)
    {
        if (MapperRegistry<TSource, TTarget>.Instance != null)
            throw new InvalidOperationException(
                $"Mapper already registered for {typeof(TSource).Name} -> {typeof(TTarget).Name}");

        MapperRegistry<TSource, TTarget>.Instance = new MapperDelegate<TSource, TTarget>(map);
    }

    public static TTarget Map<TSource, TTarget>(TSource source)
    {
        var mapper = MapperRegistry<TSource, TTarget>.Instance;

        if (mapper == null)
            throw new InvalidOperationException(
                $"No mapper registered for {typeof(TSource).Name} -> {typeof(TTarget).Name}");

        return mapper.Map(source);
    }
}
