using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;

/// <summary>
/// Enumeration đại diện cho CitizenStatus.
/// </summary>
public sealed class CitizenStatus : Enumeration
{
    public static readonly CitizenStatus Active = new(1, nameof(Active));
    public static readonly CitizenStatus Inactive = new(2, nameof(Inactive));

    private CitizenStatus(int id, string name)
        : base(id, name)
    {
    }
}
