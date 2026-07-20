namespace AnSinhSo.Domain.SeedWork.Exceptions;

/// <summary>
/// Ngoại lệ xảy ra khi một quy tắc nghiệp vụ (Business Rule) bị vi phạm.
/// </summary>
public class BusinessRuleValidationException : DomainException
{
    /// <summary>
    /// Quy tắc nghiệp vụ bị vi phạm.
    /// </summary>
    public IBusinessRule BrokenRule { get; }

    /// <summary>
    /// Thông điệp chi tiết về vi phạm.
    /// </summary>
    public string Details { get; }

    /// <summary>
    /// Khởi tạo ngoại lệ dựa trên quy tắc bị vi phạm.
    /// </summary>
    /// <param name="brokenRule">Quy tắc bị vi phạm.</param>
    public BusinessRuleValidationException(IBusinessRule brokenRule)
        : base(brokenRule.Message)
    {
        BrokenRule = brokenRule;
        Details = brokenRule.Message;
    }
}
