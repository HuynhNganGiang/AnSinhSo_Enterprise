using System;
using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.BusinessRules;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Events;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate;

/// <summary>
/// Gốc tập hợp (Aggregate Root) đại diện cho hộ gia đình.
/// </summary>
public sealed class Household : AggregateRoot<HouseholdId>
{
    private readonly List<HouseholdMember> _members = new();

    /// <summary>
    /// Mã sổ hộ khẩu.
    /// </summary>
    public HouseholdCode HouseholdCode { get; private set; }

    /// <summary>
    /// Địa chỉ thường trú của hộ gia đình.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Trạng thái hiện tại của hộ gia đình.
    /// </summary>
    public HouseholdStatus Status { get; private set; }

    /// <summary>
    /// Toạ độ địa lý trên bản đồ.
    /// </summary>
    public Location? Location { get; private set; }

    /// <summary>
    /// Danh sách các thành viên trong hộ gia đình.
    /// </summary>
    public IReadOnlyCollection<HouseholdMember> Members => _members.AsReadOnly();

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Household()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Household(HouseholdId id, HouseholdCode householdCode, Address address, HouseholdStatus status)
    {
        Id = id;
        HouseholdCode = householdCode;
        Address = address;
        Status = status;
    }

    /// <summary>
    /// Khởi tạo một hộ gia đình mới.
    /// </summary>
    /// <param name="id">Định danh hộ gia đình.</param>
    /// <param name="householdCode">Mã sổ hộ khẩu.</param>
    /// <param name="address">Địa chỉ thường trú.</param>
    /// <returns>Kết quả chứa hộ gia đình hoặc lỗi.</returns>
    public static Result<Household> Create(HouseholdId id, HouseholdCode householdCode, Address address)
    {
        var household = new Household(id, householdCode, address, HouseholdStatus.Active);

        household.RaiseDomainEvent(new HouseholdCreatedDomainEvent(household.Id));

        return Result.Success(household);
    }

    /// <summary>
    /// Thêm thành viên mới vào hộ gia đình.
    /// </summary>
    /// <param name="citizenId">Định danh công dân.</param>
    /// <param name="relationshipTypeId">Loại quan hệ (RelationshipType).</param>
    /// <param name="isHead">Có phải chủ hộ không.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result AddMember(CitizenId citizenId, AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate.RelationshipTypeId relationshipTypeId, bool isHead)
    {
        CheckRule(new CannotAddDuplicateCitizenRule(_members.Any(m => m.CitizenId == citizenId)));

        var headCount = _members.Count(m => m.IsHead) + (isHead ? 1 : 0);
        CheckRule(new HouseholdCannotHaveMultipleHeadsRule(headCount));

        var memberId = new HouseholdMemberId(Guid.NewGuid());
        var member = new HouseholdMember(memberId, citizenId, relationshipTypeId, isHead);

        _members.Add(member);

        RaiseDomainEvent(new HouseholdMemberAddedDomainEvent(Id, citizenId));

        return Result.Success();
    }

    /// <summary>
    /// Xóa thành viên khỏi hộ gia đình.
    /// </summary>
    /// <param name="citizenId">Định danh công dân cần xóa.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result RemoveMember(CitizenId citizenId)
    {
        var member = _members.FirstOrDefault(m => m.CitizenId == citizenId);
        if (member is null)
        {
            return Result.Failure(Error.NotFound("Household.MemberNotFound", "Không tìm thấy thành viên trong hộ gia đình."));
        }

        CheckRule(new CannotRemoveCurrentHeadRule(member.IsHead));

        _members.Remove(member);

        RaiseDomainEvent(new HouseholdMemberRemovedDomainEvent(Id, citizenId));

        return Result.Success();
    }

    /// <summary>
    /// Thay đổi chủ hộ.
    /// </summary>
    /// <param name="newHeadCitizenId">Định danh công dân sẽ làm chủ hộ mới.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result ChangeHead(CitizenId newHeadCitizenId)
    {
        var newHead = _members.FirstOrDefault(m => m.CitizenId == newHeadCitizenId);
        if (newHead is null)
        {
            return Result.Failure(Error.NotFound("Household.MemberNotFound", "Không tìm thấy thành viên trong hộ gia đình để chỉ định làm chủ hộ."));
        }

        if (newHead.IsHead)
        {
            return Result.Success();
        }

        var currentHead = _members.FirstOrDefault(m => m.IsHead);
        if (currentHead is not null)
        {
            currentHead.SetHeadStatus(false);
        }

        newHead.SetHeadStatus(true);
        RaiseDomainEvent(new HouseholdHeadChangedDomainEvent(Id, newHeadCitizenId));

        return Result.Success();
    }

    /// <summary>
    /// Thay đổi địa chỉ hộ gia đình.
    /// </summary>
    /// <param name="newAddress">Địa chỉ mới.</param>
    /// <returns>Kết quả.</returns>
    public Result ChangeAddress(Address newAddress)
    {
        Address = newAddress;
        
        RaiseDomainEvent(new HouseholdAddressChangedDomainEvent(Id));
        return Result.Success();
    }

    /// <summary>
    /// Kích hoạt hộ gia đình.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Activate()
    {
        if (Status == HouseholdStatus.Active)
        {
            return Result.Success();
        }

        Status = HouseholdStatus.Active;
        RaiseDomainEvent(new HouseholdActivatedDomainEvent(Id));
        return Result.Success();
    }

    /// <summary>
    /// Hủy kích hoạt hộ gia đình.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Deactivate()
    {
        if (Status == HouseholdStatus.Inactive)
        {
            return Result.Success();
        }

        Status = HouseholdStatus.Inactive;
        RaiseDomainEvent(new HouseholdDeactivatedDomainEvent(Id));
        return Result.Success();
    }

    /// <summary>
    /// Cập nhật toạ độ địa lý.
    /// </summary>
    public Result UpdateLocation(Location location)
    {
        Guard.Against.Null(location, nameof(location));

        if (Location == location)
        {
            return Result.Success();
        }

        Location = location;
        RaiseDomainEvent(new HouseholdLocationUpdatedDomainEvent(Id, location));
        return Result.Success();
    }
}
