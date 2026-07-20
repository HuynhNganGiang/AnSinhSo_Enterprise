using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Domain.Interfaces;

/// <summary>
/// Hợp đồng chung cho Repository Pattern, sử dụng chuyên biệt cho Aggregate Root.
/// Tuyệt đối không phục thuộc vào Entity Framework Core hay Database.
/// </summary>
/// <typeparam name="TAggregate">Kiểu thực thể Aggregate Root.</typeparam>
/// <typeparam name="TId">Kiểu định danh của Aggregate Root.</typeparam>
public interface IRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    /// <summary>
    /// Lấy một đối tượng dựa vào định danh duy nhất (Id).
    /// </summary>
    /// <param name="id">Định danh cần tìm.</param>
    /// <param name="cancellationToken">Token hủy tác vụ bất đồng bộ.</param>
    /// <returns>Đối tượng AggregateRoot hoặc Null nếu không tìm thấy.</returns>
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy một đối tượng đầu tiên thỏa mãn điều kiện quy định bởi Specification.
    /// </summary>
    /// <param name="specification">Quy tắc tìm kiếm (Domain Rule).</param>
    /// <param name="cancellationToken">Token hủy tác vụ bất đồng bộ.</param>
    /// <returns>Đối tượng AggregateRoot hoặc Null nếu không thỏa mãn.</returns>
    Task<TAggregate?> FirstOrDefaultAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lấy danh sách các đối tượng thỏa mãn điều kiện quy định bởi Specification.
    /// </summary>
    /// <param name="specification">Quy tắc tìm kiếm (Domain Rule).</param>
    /// <param name="cancellationToken">Token hủy tác vụ bất đồng bộ.</param>
    /// <returns>Danh sách IReadOnlyList chứa các đối tượng.</returns>
    Task<IReadOnlyList<TAggregate>> ListAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đếm số lượng đối tượng thỏa mãn điều kiện quy định bởi Specification.
    /// </summary>
    /// <param name="specification">Quy tắc tìm kiếm (Domain Rule).</param>
    /// <param name="cancellationToken">Token hủy tác vụ bất đồng bộ.</param>
    /// <returns>Số lượng phần tử.</returns>
    Task<int> CountAsync(ISpecification<TAggregate> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Thêm một đối tượng mới vào Repository.
    /// </summary>
    /// <param name="entity">Đối tượng cần thêm.</param>
    /// <param name="cancellationToken">Token hủy tác vụ bất đồng bộ.</param>
    Task AddAsync(TAggregate entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Đánh dấu xóa đối tượng khỏi Repository.
    /// Việc xóa thực sự diễn ra khi gọi SaveChangesAsync ở IUnitOfWork.
    /// </summary>
    /// <param name="entity">Đối tượng cần xóa.</param>
    void Remove(TAggregate entity);
}
