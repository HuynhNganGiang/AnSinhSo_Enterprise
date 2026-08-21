using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Enumerations;

/// <summary>
/// Enumeration d?i di?n cho Currency.
/// </summary>
public sealed class Currency : Enumeration
{
    public static readonly Currency VND = new(1, nameof(VND));
    public static readonly Currency USD = new(2, nameof(USD));
    public static readonly Currency EUR = new(3, nameof(EUR));

    private Currency(int id, string name)
        : base(id, name)
    {
    }
}
