using System;

namespace AnSinhSo.Domain.SeedWork.Exceptions;

/// <summary>
/// Ngoại lệ cơ sở cho toàn bộ tầng Domain.
/// Chỉ kế thừa từ System.Exception, không phụ thuộc Infrastructure hay HTTP.
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary>
    /// Khởi tạo ngoại lệ Domain mặc định.
    /// </summary>
    protected DomainException()
    {
    }

    /// <summary>
    /// Khởi tạo ngoại lệ Domain kèm thông báo.
    /// </summary>
    /// <param name="message">Thông điệp lỗi nghiệp vụ.</param>
    protected DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Khởi tạo ngoại lệ Domain kèm thông báo và ngoại lệ gốc.
    /// </summary>
    /// <param name="message">Thông điệp lỗi nghiệp vụ.</param>
    /// <param name="innerException">Ngoại lệ nội bộ.</param>
    protected DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
