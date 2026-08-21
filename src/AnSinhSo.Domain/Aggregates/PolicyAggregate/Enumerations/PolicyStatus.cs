using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate.Enumerations;

/// <summary>
/// Enumeration d?i di?n cho PolicyStatus.
/// </summary>
public sealed class PolicyStatus : Enumeration
{
    public static readonly PolicyStatus Active = new(1, nameof(Active));
    public static readonly PolicyStatus Closed = new(2, nameof(Closed));

    private PolicyStatus(int id, string name)
        : base(id, name)
    {
    }
}
