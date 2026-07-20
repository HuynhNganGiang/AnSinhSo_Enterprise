using System;

namespace AnSinhSo.Domain.SeedWork.Results;

/// <summary>
/// Đại diện cho kết quả của một thao tác nghiệp vụ, có mang theo dữ liệu trả về.
/// </summary>
/// <typeparam name="TValue">Kiểu dữ liệu trả về.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    /// <summary>
    /// Giá trị trả về. Sẽ văng lỗi nếu cố tình truy cập khi kết quả là thất bại.
    /// </summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Không thể truy cập dữ liệu của một kết quả thất bại.");

    /// <summary>
    /// Khởi tạo Result có chứa dữ liệu.
    /// </summary>
    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Hỗ trợ chuyển đổi ngầm định từ giá trị thành Result thành công.
    /// </summary>
    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
}
