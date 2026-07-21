using System;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;

/// <summary>
/// Gốc tập hợp (Aggregate Root) đại diện cho nhóm đối tượng an sinh (Welfare Group).
/// </summary>
public sealed class WelfareGroup : AggregateRoot<WelfareGroupId>
{
    /// <summary>
    /// Tên nhóm đối tượng.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Mô tả chi tiết nhóm đối tượng.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Trạng thái hoạt động.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private WelfareGroup()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private WelfareGroup(
        WelfareGroupId id,
        string name,
        string? description,
        bool isActive) : base(id)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    /// <summary>
    /// Khởi tạo một WelfareGroup mới.
    /// </summary>
    /// <param name="id">Định danh nhóm đối tượng.</param>
    /// <param name="name">Tên nhóm đối tượng.</param>
    /// <param name="description">Mô tả nhóm đối tượng.</param>
    /// <returns>Kết quả chứa WelfareGroup hoặc lỗi.</returns>
    public static Result<WelfareGroup> Create(
        WelfareGroupId id,
        string name,
        string? description)
    {
        Guard.Against.Null(id, nameof(id));
        Guard.Against.Empty(name, nameof(name));

        name = name.Trim();
        Guard.Against.Empty(name, nameof(name));
        description = description?.Trim();

        var welfareGroup = new WelfareGroup(id, name, description, true);

        return Result.Success(welfareGroup);
    }

    /// <summary>
    /// Đổi tên và mô tả của nhóm đối tượng.
    /// </summary>
    /// <param name="newName">Tên mới.</param>
    /// <param name="newDescription">Mô tả mới.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Rename(string newName, string? newDescription)
    {
        Guard.Against.Empty(newName, nameof(newName));

        newName = newName.Trim();
        Guard.Against.Empty(newName, nameof(newName));
        newDescription = newDescription?.Trim();

        if (Name == newName && Description == newDescription)
        {
            return Result.Success();
        }

        Name = newName;
        Description = newDescription;

        return Result.Success();
    }

    /// <summary>
    /// Kích hoạt nhóm đối tượng.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Activate()
    {
        if (IsActive)
        {
            return Result.Success();
        }

        IsActive = true;
        return Result.Success();
    }

    /// <summary>
    /// Hủy kích hoạt nhóm đối tượng.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Deactivate()
    {
        if (!IsActive)
        {
            return Result.Success();
        }

        IsActive = false;
        return Result.Success();
    }
}
