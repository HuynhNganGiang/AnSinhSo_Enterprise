using System;
using System.Linq.Expressions;

namespace AnSinhSo.Domain.Specifications;

/// <summary>
/// Lớp cơ sở trừu tượng cho Specification Pattern, cung cấp các thao tác logic AND/OR/NOT.
/// </summary>
/// <typeparam name="T">Kiểu thực thể.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <inheritdoc/>
    public abstract Expression<Func<T, bool>> ToExpression();

    private Func<T, bool>? _compiledExpression;

    /// <inheritdoc/>
    public bool IsSatisfiedBy(T entity)
    {
        _compiledExpression ??= ToExpression().Compile();
        return _compiledExpression(entity);
    }

    /// <inheritdoc/>
    public ISpecification<T> And(ISpecification<T> specification)
    {
        return new AndSpecification<T>(this, specification);
    }

    /// <inheritdoc/>
    public ISpecification<T> Or(ISpecification<T> specification)
    {
        return new OrSpecification<T>(this, specification);
    }

    /// <inheritdoc/>
    public ISpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}
