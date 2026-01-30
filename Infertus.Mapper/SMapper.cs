namespace Infertus.Mapper;

public static class SMapper
{
    public static void Register<TSource, TTarget>(Func<TSource, TTarget> map)
    {
        if (MappingsRegistry<TSource, TTarget>.Instance != null)
            throw new InvalidOperationException(
                $"Mapper already registered for {typeof(TSource).Name} -> {typeof(TTarget).Name}");

        MappingsRegistry<TSource, TTarget>.Instance = new MapperDelegate<TSource, TTarget>(map);
    }

    public static TTarget Map<TSource, TTarget>(TSource source)
    {
        var mapper = MappingsRegistry<TSource, TTarget>.Instance;

        return mapper != null
            ? mapper.Map(source)
            : throw new InvalidOperationException(
                $"No mapper registered for {typeof(TSource).Name} -> {typeof(TTarget).Name}");
    }
}
