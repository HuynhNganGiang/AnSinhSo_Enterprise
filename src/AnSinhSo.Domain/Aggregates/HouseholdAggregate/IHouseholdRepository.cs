using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;
using AnSinhSo.Domain.Specifications;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate;

public interface IHouseholdRepository
{
    Task<Household?> GetByIdAsync(HouseholdId id, CancellationToken cancellationToken = default);
    Task<Household?> GetByCodeAsync(HouseholdCode code, CancellationToken cancellationToken = default);
    Task<Household?> GetByCitizenIdAsync(CitizenId citizenId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(HouseholdId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(HouseholdCode code, CancellationToken cancellationToken = default);
    Task<bool> IsCitizenInAnyHouseholdAsync(CitizenId citizenId, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Household> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, string? sort, CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Household> Items, int TotalCount)> SearchAsync(string? keyword, HouseholdStatus? status, int page, int pageSize, string? sort, CancellationToken cancellationToken = default);

    void Add(Household household);
    void Update(Household household);
    void Remove(Household household);

    // Specification Pattern readiness
    Task<Household?> FirstOrDefaultAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Household>> ListAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ISpecification<Household> specification, CancellationToken cancellationToken = default);
}
