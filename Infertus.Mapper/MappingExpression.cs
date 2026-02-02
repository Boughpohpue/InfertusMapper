using Infertus.Mapper.Internal.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace Infertus.Mapper;

public sealed class MappingExpression<TSource, TTarget>
{
    private readonly TypeMap _typeMap;

    internal MappingExpression(TypeMap typeMap)
    {
        _typeMap = typeMap;
    }

    public MappingExpression<TSource, TTarget> ForMember<TMember>(
        Expression<Func<TTarget, TMember>> target, Expression<Func<TSource, TMember>> source)
    {
        if (target.Body is not MemberExpression member)
            throw new ArgumentException($"{nameof(target)} must be a member expression!");

        if (member.Member is PropertyInfo p && !p.CanWrite)
            throw new InvalidOperationException(
                $"Can't map {p.Name} as it's not writable!");

        if (_typeMap.IgnoredMembers.Contains(member.Member))
            throw new InvalidOperationException(
                $"Can't map {member.Member.Name} as it's marked as ignored!");

        _typeMap.Members.RemoveAll(m => m.Target == member.Member);
        _typeMap.Members.Add(new MemberExpressionMap(member.Member, source));

        return this;
    }

    public MappingExpression<TSource, TTarget> ForMember<TMember>(
        Expression<Func<TTarget, TMember>> target,
        Action<MemberConfigExpression<TSource, TTarget, TMember>> configure)
    {
        if (target.Body is not MemberExpression member)
            throw new ArgumentException($"{nameof(target)} must be a member expression!");

        if (member.Member is PropertyInfo p && !p.CanWrite)
            throw new InvalidOperationException(
                $"Can't map {p.Name} as it's not writable!");

        if (_typeMap.IgnoredMembers.Contains(member.Member))
            throw new InvalidOperationException(
                $"Can't map {member.Member.Name} as it's marked as ignored!");

        var config = new MemberConfigExpression<TSource, TTarget, TMember>();
        configure(config);

        if (config.SourceExpression == null)
            throw new InvalidOperationException(
                $"No source mapping configured for {member.Member.Name}");

        _typeMap.Members.RemoveAll(m => m.Target == member.Member);
        _typeMap.Members.Add(new MemberExpressionMap(member.Member, config.SourceExpression));

        return this;
    }


    public MappingExpression<TSource, TTarget> Ignore<TMember>(Expression<Func<TTarget, TMember>> target)
    {
        if (target.Body is not MemberExpression member)
            throw new ArgumentException($"{nameof(target)} must be a member expression!");

        if (!_typeMap.IgnoredMembers.Contains(member.Member))
        {
            _typeMap.Members.RemoveAll(m => m.Target == member.Member);
            _typeMap.IgnoredMembers.Add(member.Member);
        }

        return this;
    }
}
