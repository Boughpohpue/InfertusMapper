using System.Linq.Expressions;

namespace Infertus.Mapper.Internal.Services;

internal sealed class ParameterReplaceVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _from;
    private readonly Expression _to;

    public ParameterReplaceVisitor(ParameterExpression from, Expression to)
    {
        _from = from;
        _to = to;
    }

    protected override Expression VisitParameter(ParameterExpression node)
        => node == _from ? _to : base.VisitParameter(node);
}
