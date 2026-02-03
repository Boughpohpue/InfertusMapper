namespace Infertus.Mapper.DemoConsoleApp;

public static class CollectionPrintExtensions
{
    public static string GetString<T>(this IEnumerable<T> collection, string name = "")
    {
        var s = string.IsNullOrEmpty(name)
            ? $"<{collection.GetCollectionTypeString()}>:\n"
            : $"{name} <{collection.GetCollectionTypeString()}>:\n";

        s += "[\n";
        collection.ToList().ForEach(item => s += $"\t{item.GetCollectionItemString()}\n");
        s += "]";

        return s;
    }

    public static string GetCollectionTypeString<T>(this IEnumerable<T> collection)
    {
        var collectionTypeName = new string(collection.GetType().Name.ToArray()
            .Where(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')).ToArray());

        return $"{collectionTypeName}<{typeof(T).Name}>";
    }

    public static string GetCollectionItemString<T>(this T item)
    {
        return "{ " + string.Join(", ", $"{item}".Split('\n').Where(i => !string.IsNullOrWhiteSpace(i))).Replace(":,", ":") + " },";
    }
}
