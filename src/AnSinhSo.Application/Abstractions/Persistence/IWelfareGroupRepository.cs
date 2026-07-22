using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;

namespace AnSinhSo.Application.Abstractions.Persistence;

/// <summary>
/// Giao diện repository cho WelfareGroup aggregate root.
/// </summary>
public interface IWelfareGroupRepository : IRepository<WelfareGroup, WelfareGroupId>
{
}
