using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.SeedWork.Entities;

/// <summary>
/// Lớp cơ sở (non-generic) chứa thông tin audit và quản lý sự kiện miền.
/// Không chứa định danh.
/// </summary>
public abstract class BaseEntity : IAuditable, IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <inheritdoc/>
    public DateTime CreatedAt { get; protected set; }

    /// <inheritdoc/>
    public string? CreatedBy { get; protected set; }

    /// <inheritdoc/>
    public DateTime? UpdatedAt { get; protected set; }

    /// <inheritdoc/>
    public string? UpdatedBy { get; protected set; }

    /// <inheritdoc/>
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <inheritdoc/>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc/>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Kiểm tra quy tắc nghiệp vụ. Ném ngoại lệ nếu vi phạm.
    /// </summary>
    /// <param name="rule">Quy tắc nghiệp vụ cần kiểm tra.</param>
    protected static void CheckRule(AnSinhSo.Domain.SeedWork.Exceptions.IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new AnSinhSo.Domain.SeedWork.Exceptions.BusinessRuleValidationException(rule);
        }
    }

    /// <summary>
    /// Phát sinh sự kiện miền.
    /// </summary>
    /// <param name="domainEvent">Sự kiện miền.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        AddDomainEvent(domainEvent);
    }
}
