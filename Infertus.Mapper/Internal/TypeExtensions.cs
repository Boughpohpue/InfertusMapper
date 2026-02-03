using System.Reflection;

namespace Infertus.Mapper.Internal;

internal static class TypeExtensions
{
    private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.Instance;

    public static IEnumerable<PropertyInfo> GetWritableProperties(this Type t, BindingFlags flags = DefaultFlags) =>
        t.GetProperties(flags).Where(p => p.CanWrite);

    public static Dictionary<string, PropertyInfo> GetReadablePropertiesDict(this Type t, BindingFlags flags = DefaultFlags) =>
        t.GetProperties(flags).Where(p => p.CanRead).ToDictionary(p => p.Name);

    internal static bool IsEnumerable(this Type type, out Type elementType)
    {
        if (type.IsArray)
        {
            elementType = type.GetElementType()!;
            return true;
        }

        if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
        {
            elementType = type.GetGenericArguments()[0];
            return true;
        }

        var enumerableInterface = type.GetInterfaces()
            .FirstOrDefault(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        if (enumerableInterface != null)
        {
            elementType = enumerableInterface.GetGenericArguments()[0];
            return true;
        }

        elementType = null!;
        return false;
    }
}
