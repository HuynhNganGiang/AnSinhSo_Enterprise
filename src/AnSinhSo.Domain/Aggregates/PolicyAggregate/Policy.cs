using System;
using AnSinhSo.Domain.Aggregates.PolicyAggregate.BusinessRules;
using AnSinhSo.Domain.Aggregates.PolicyAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.PolicyAggregate.Events;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PolicyAggregate;

/// <summary>
/// Gốc tập hợp (Aggregate Root) đại diện cho chính sách (Policy).
/// </summary>
public sealed class Policy : AggregateRoot<PolicyId>
{
    /// <summary>
    /// Tên chính sách.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Mô tả chi tiết chính sách.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Số tiền hỗ trợ của chính sách.
    /// </summary>
    public Money Amount { get; private set; }

    /// <summary>
    /// Trạng thái của chính sách.
    /// </summary>
    public PolicyStatus Status { get; private set; }

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Policy()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Policy(
        PolicyId id,
        string name,
        string? description,
        Money amount,
        PolicyStatus status) : base(id)
    {
        Name = name;
        Description = description;
        Amount = amount;
        Status = status;
    }

    /// <summary>
    /// Khởi tạo một Policy mới.
    /// </summary>
    /// <param name="id">Định danh chính sách.</param>
    /// <param name="name">Tên chính sách.</param>
    /// <param name="description">Mô tả chính sách.</param>
    /// <param name="amount">Số tiền của chính sách.</param>
    /// <returns>Kết quả chứa Policy hoặc lỗi.</returns>
    public static Result<Policy> Create(
        PolicyId id,
        string name,
        string? description,
        Money amount)
    {
        Guard.Against.Null(id, nameof(id));
        Guard.Against.Null(name, nameof(name));
        Guard.Against.Empty(name, nameof(name));
        Guard.Against.Null(amount, nameof(amount));

        name = name.Trim();
        Guard.Against.Empty(name, nameof(name));

        description = description?.Trim();

        var policy = new Policy(id, name, description, amount, PolicyStatus.Active);

        return Result.Success(policy);
    }

    /// <summary>
    /// Kích hoạt chính sách.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Activate()
    {
        if (Status == PolicyStatus.Active)
        {
            return Result.Success();
        }

        Status = PolicyStatus.Active;
        RaiseDomainEvent(new PolicyActivatedDomainEvent(Id));
        return Result.Success();
    }

    /// <summary>
    /// Hủy kích hoạt / Đóng chính sách.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Deactivate()
    {
        if (Status == PolicyStatus.Closed)
        {
            return Result.Success();
        }

        Status = PolicyStatus.Closed;
        return Result.Success();
    }

    /// <summary>
    /// Cập nhật số tiền của chính sách.
    /// </summary>
    /// <param name="newAmount">Số tiền mới.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result UpdateAmount(Money newAmount)
    {
        Guard.Against.Null(newAmount, nameof(newAmount));

        CheckRule(new PolicyCannotUpdateWhenClosedRule(Status));

        Amount = newAmount;

        return Result.Success();
    }
}
