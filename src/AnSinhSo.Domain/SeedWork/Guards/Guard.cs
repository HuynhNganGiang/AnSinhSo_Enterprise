namespace AnSinhSo.Domain.SeedWork.Guards;

/// <summary>
/// Lớp trung tâm (Entry point) cho cơ chế Guard.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Truy cập các phương thức Guard (VD: Guard.Against.Null).
    /// </summary>
    public static IGuardClause Against { get; } = new GuardClause();

    private class GuardClause : IGuardClause
    {
    }
}

/// <summary>
/// Interface đánh dấu (Marker Interface) để viết các phương thức mở rộng (Extension Methods) cho Guard.
/// </summary>
public interface IGuardClause
{
}
