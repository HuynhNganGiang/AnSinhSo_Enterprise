using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện repository cho Citizen aggregate root.
/// </summary>
public interface ICitizenRepository : IRepository<Citizen, CitizenId>
{
    /// <summary>
    /// Lấy thông tin công dân theo mã định danh (CCCD/CMND).
    /// </summary>
    /// <param name="citizenNumber">Mã định danh công dân.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về công dân nếu tìm thấy, ngược lại trả về null.</returns>
    Task<Citizen?> GetByCitizenNumberAsync(CitizenNumber citizenNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Kiểm tra xem một mã định danh công dân đã tồn tại hay chưa.
    /// </summary>
    /// <param name="citizenNumber">Mã định danh công dân cần kiểm tra.</param>
    /// <param name="cancellationToken">Token hủy yêu cầu.</param>
    /// <returns>Trả về true nếu đã tồn tại, ngược lại trả về false.</returns>
    Task<bool> ExistsCitizenNumberAsync(CitizenNumber citizenNumber, CancellationToken cancellationToken = default);
}
