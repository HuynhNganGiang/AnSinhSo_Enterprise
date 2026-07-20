namespace AnSinhSo.Domain.SeedWork.Exceptions;

/// <summary>
/// Giao diện cho các quy tắc nghiệp vụ (Business Rules).
/// </summary>
public interface IBusinessRule
{
    /// <summary>
    /// Thông điệp lỗi nếu quy tắc bị vi phạm.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Kiểm tra quy tắc có bị vi phạm hay không.
    /// </summary>
    /// <returns>True nếu vi phạm quy tắc, ngược lại False.</returns>
    bool IsBroken();
}
