using System;
using System.Linq.Expressions;

namespace AnSinhSo.Domain.Specifications;

/// <summary>
/// Specification sử dụng phép toán logic NOT để đảo ngược một specification khác.
/// </summary>
/// <typeparam name="T">Kiểu thực thể.</typeparam>
public sealed class NotSpecification<T> : Specification<T>
{
    private readonly ISpecification<T> _specification;

    /// <summary>
    /// Khởi tạo một NotSpecification.
    /// </summary>
    /// <param name="specification">Specification cần đảo ngược.</param>
    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    /// <inheritdoc/>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = _specification.ToExpression();
        var notBody = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notBody, expression.Parameters);
    }
}
