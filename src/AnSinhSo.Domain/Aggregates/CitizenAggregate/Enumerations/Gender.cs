using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;

/// <summary>
/// Enumeration đại diện cho Gender.
/// </summary>
public sealed class Gender : Enumeration
{
    public static readonly Gender Male = new(1, nameof(Male));
    public static readonly Gender Female = new(2, nameof(Female));
    public static readonly Gender Other = new(3, nameof(Other));

    private Gender(int id, string name)
        : base(id, name)
    {
    }
}
