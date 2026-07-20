namespace AnSinhSo.Domain.SeedWork.Entities;

/// <summary>
/// Lớp cơ sở đánh dấu một thực thể là gốc (Aggregate Root).
/// </summary>
/// <typeparam name="TId">Kiểu dữ liệu của định danh.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
{
    /// <summary>
    /// Hàm khởi tạo bảo vệ.
    /// </summary>
    protected AggregateRoot(TId id) : base(id)
    {
    }

    /// <summary>
    /// Hàm khởi tạo mặc định cho ORM.
    /// </summary>
    protected AggregateRoot()
    {
    }
}
