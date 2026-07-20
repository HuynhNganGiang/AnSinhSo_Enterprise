namespace AnSinhSo.Domain.SeedWork.ValueObjects;

/// <summary>
/// Lớp cơ sở (dạng record abstract) cho các định danh có kiểu dữ liệu mạnh (Strongly Typed Ids).
/// </summary>
/// <typeparam name="TValue">Kiểu dữ liệu nguyên thủy của định danh (VD: Guid, int, string).</typeparam>
public abstract record StronglyTypedId<TValue>(TValue Value);
