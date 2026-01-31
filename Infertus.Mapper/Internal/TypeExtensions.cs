using System.Reflection;

namespace Infertus.Mapper.Internal;

internal static class TypeExtensions
{
    private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Instance;

    public static IEnumerable<PropertyInfo> GetWritableProperties(this Type t, BindingFlags flags = DefaultFlags) =>
        t.GetProperties(flags).Where(p => p.CanWrite);

    public static Dictionary<string, PropertyInfo> GetReadablePropertiesDict(this Type t, BindingFlags flags = DefaultFlags) =>
        t.GetProperties(flags).Where(p => p.CanRead).ToDictionary(p => p.Name);
}
