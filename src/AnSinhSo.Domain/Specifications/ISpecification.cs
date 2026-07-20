using System;
using System.Linq.Expressions;

namespace AnSinhSo.Domain.Specifications;

/// <summary>
/// Hợp đồng chung cho Specification Pattern, mô tả các quy tắc nghiệp vụ.
/// </summary>
/// <typeparam name="T">Kiểu thực thể cần kiểm tra điều kiện.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Trả về biểu thức (Expression) mô tả quy tắc/điều kiện nghiệp vụ.
    /// </summary>
    /// <returns>Expression&lt;Func&lt;T, bool&gt;&gt; để thẩm định một thực thể.</returns>
    Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Kiểm tra xem thực thể có thỏa mãn điều kiện quy định không.
    /// </summary>
    /// <param name="entity">Thực thể cần kiểm tra.</param>
    /// <returns>True nếu thỏa mãn, ngược lại False.</returns>
    bool IsSatisfiedBy(T entity);

    /// <summary>
    /// Kết hợp với Specification khác bằng phép logic AND.
    /// </summary>
    ISpecification<T> And(ISpecification<T> specification);

    /// <summary>
    /// Kết hợp với Specification khác bằng phép logic OR.
    /// </summary>
    ISpecification<T> Or(ISpecification<T> specification);

    /// <summary>
    /// Phủ định lại Specification hiện tại (phép logic NOT).
    /// </summary>
    ISpecification<T> Not();
}
