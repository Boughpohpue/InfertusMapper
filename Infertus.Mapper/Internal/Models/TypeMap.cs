using System.Reflection;

namespace Infertus.Mapper.Internal.Models;

internal sealed class TypeMap(Type sourceType, Type targetType)
{
    public Type SourceType { get; } = sourceType;
    public Type TargetType { get; } = targetType;
    public List<MemberExpressionMap> Members { get; } = [];
    public HashSet<MemberInfo> IgnoredMembers { get; } = [];
}
