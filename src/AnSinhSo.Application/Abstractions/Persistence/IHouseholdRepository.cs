using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện repository cho Household aggregate root.
/// </summary>
public interface IHouseholdRepository : IRepository<Household, HouseholdId>
{
    /// <summary>
    /// Lấy thông tin hộ gia đình kèm theo danh sách thành viên.
    /// </summary>
    /// <param name="id">Định danh hộ gia đình.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về hộ gia đình kèm thành viên nếu tìm thấy, ngược lại trả về null.</returns>
    Task<Household?> GetWithMembersAsync(HouseholdId id, CancellationToken cancellationToken = default);
}
