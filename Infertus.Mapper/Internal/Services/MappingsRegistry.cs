using Infertus.Mapper.Internal.Interfaces;
using Infertus.Mapper.Internal.Models;
using System.Reflection;

namespace Infertus.Mapper.Internal.Services;

internal static class MappingsRegistry
{
    private static readonly List<TypeMap> _maps = [];
    private static readonly Dictionary<(Type Source, Type Target), object> _mappings = [];

    public static object? Get(Type sourceType, Type targetType)
    {
        _mappings.TryGetValue((sourceType, targetType), out var mapper);

        return mapper;
    }

    public static TypeMap Add<TSource, TTarget>()
    {
        var map = new TypeMap(typeof(TSource), typeof(TTarget));

        _maps.Add(map);

        return map;
    }

    public static void Register<TSource, TTarget>(IMap<TSource, TTarget> mapping)
    {
        if (Get(typeof(TSource), typeof(TTarget)) != null)
            throw new InvalidOperationException(
                $"Mapping {typeof(TSource).Name} -> {typeof(TTarget).Name} already registered!");

        _mappings[(typeof(TSource), typeof(TTarget))] = mapping;
    }

    internal static void Compile()
    {
        foreach (var map in _maps)
            CompileAndRegister(map);
    }

    private static void RegisterNotExistingMappings<TSource, TTarget>(IMap<TSource, TTarget> mapping)
    {
        if (Get(typeof(TSource), typeof(TTarget)) == null)
            _mappings[(typeof(TSource), typeof(TTarget))] = mapping;
    }

    private static void CompileGeneric<TSource, TTarget>(TypeMap map)
    {
        RegisterNotExistingMappings(
            new MappingDelegate<TSource, TTarget>(
                MappingCompiler.Compile<TSource, TTarget>(map)));
    }

    private static void CompileAndRegister(TypeMap map)
    {
        var method = typeof(MappingsRegistry)
            .GetMethod(nameof(CompileGeneric), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(map.SourceType, map.TargetType);
        method.Invoke(null, [map]);
    }
}
