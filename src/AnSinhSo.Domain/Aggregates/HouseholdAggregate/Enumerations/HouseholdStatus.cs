using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;

/// <summary>
/// Enumeration đại diện cho HouseholdStatus.
/// </summary>
public sealed class HouseholdStatus : Enumeration
{
    public static readonly HouseholdStatus Active = new(1, nameof(Active));
    public static readonly HouseholdStatus Inactive = new(2, nameof(Inactive));

    private HouseholdStatus(int id, string name)
        : base(id, name)
    {
    }
}
