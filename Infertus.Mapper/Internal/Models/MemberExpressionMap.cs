using System.Linq.Expressions;
using System.Reflection;

namespace Infertus.Mapper.Internal.Models;

internal sealed class MemberExpressionMap(MemberInfo target, LambdaExpression source)
{
    public MemberInfo Target { get; set; } = target;
    public LambdaExpression Source { get; set; } = source;
}
