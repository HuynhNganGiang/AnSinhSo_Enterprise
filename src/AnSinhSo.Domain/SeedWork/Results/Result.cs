using System;

namespace AnSinhSo.Domain.SeedWork.Results;

/// <summary>
/// Đại diện cho kết quả của một thao tác nghiệp vụ, không có dữ liệu trả về.
/// </summary>
public class Result
{
    /// <summary>
    /// Kiểm tra kết quả có thành công hay không.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Kiểm tra kết quả có bị lỗi hay không.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Lỗi đi kèm nếu thao tác thất bại.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Khởi tạo một đối tượng Result.
    /// </summary>
    /// <param name="isSuccess">Cờ thành công.</param>
    /// <param name="error">Lỗi đính kèm.</param>
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Kết quả thành công không thể chứa lỗi.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Kết quả thất bại phải chứa một lỗi cụ thể.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>
    /// Trả về một Result thành công.
    /// </summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Trả về một Result thất bại.
    /// </summary>
    /// <param name="error">Lỗi đi kèm.</param>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Trả về Result thành công kèm dữ liệu.
    /// </summary>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    /// <summary>
    /// Trả về Result thất bại kèm kiểu dữ liệu rỗng.
    /// </summary>
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}
