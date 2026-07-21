using System;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.BusinessRules;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Events;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.CitizenAggregate;

/// <summary>
/// Gốc tập hợp (Aggregate Root) đại diện cho công dân.
/// </summary>
public sealed class Citizen : AggregateRoot<CitizenId>
{
    /// <summary>
    /// Họ và tên công dân.
    /// </summary>
    public FullName FullName { get; private set; }

    /// <summary>
    /// Số Căn cước công dân.
    /// </summary>
    public CitizenNumber CitizenNumber { get; private set; }

    /// <summary>
    /// Ngày tháng năm sinh.
    /// </summary>
    public DateTime BirthDate { get; private set; }

    /// <summary>
    /// Giới tính.
    /// </summary>
    public Gender Gender { get; private set; }

    /// <summary>
    /// Số điện thoại liên hệ.
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; }

    /// <summary>
    /// Địa chỉ thường trú.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Địa chỉ Email liên hệ.
    /// </summary>
    public Email Email { get; private set; }

    /// <summary>
    /// Trạng thái của công dân.
    /// </summary>
    public CitizenStatus Status { get; private set; }

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Citizen()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private Citizen(
        CitizenId id,
        FullName fullName,
        CitizenNumber citizenNumber,
        DateTime birthDate,
        Gender gender,
        PhoneNumber phoneNumber,
        Address address,
        Email email,
        CitizenStatus status) : base(id)
    {
        FullName = fullName;
        CitizenNumber = citizenNumber;
        BirthDate = birthDate;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Address = address;
        Email = email;
        Status = status;
    }

    /// <summary>
    /// Khởi tạo một công dân mới.
    /// </summary>
    /// <param name="id">Định danh công dân.</param>
    /// <param name="fullName">Họ và tên.</param>
    /// <param name="citizenNumber">Số Căn cước công dân.</param>
    /// <param name="birthDate">Ngày tháng năm sinh.</param>
    /// <param name="gender">Giới tính.</param>
    /// <param name="phoneNumber">Số điện thoại.</param>
    /// <param name="address">Địa chỉ thường trú.</param>
    /// <param name="email">Email liên hệ.</param>
    /// <returns>Kết quả chứa công dân hoặc lỗi.</returns>
    public static Result<Citizen> Create(
        CitizenId id,
        FullName fullName,
        CitizenNumber citizenNumber,
        DateTime birthDate,
        Gender gender,
        PhoneNumber phoneNumber,
        Address address,
        Email email)
    {
        Guard.Against.Null(id, nameof(id));
        Guard.Against.Null(fullName, nameof(fullName));
        Guard.Against.Null(citizenNumber, nameof(citizenNumber));
        Guard.Against.Null(gender, nameof(gender));
        Guard.Against.Null(phoneNumber, nameof(phoneNumber));
        Guard.Against.Null(address, nameof(address));
        Guard.Against.Null(email, nameof(email));

        CheckRule(new BirthDateMustBeValidRule(birthDate));
        CheckRule(new CitizenNumberMustBeValidRule(citizenNumber.Value));

        var citizen = new Citizen(id, fullName, citizenNumber, birthDate, gender, phoneNumber, address, email, CitizenStatus.Active);

        citizen.RaiseDomainEvent(new CitizenCreatedDomainEvent(citizen.Id));

        return Result.Success(citizen);
    }

    /// <summary>
    /// Thay đổi số điện thoại của công dân.
    /// </summary>
    /// <param name="newPhoneNumber">Số điện thoại mới.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result ChangePhone(PhoneNumber newPhoneNumber)
    {
        Guard.Against.Null(newPhoneNumber, nameof(newPhoneNumber));

        if (PhoneNumber == newPhoneNumber)
        {
            return Result.Success();
        }

        PhoneNumber = newPhoneNumber;
        RaiseDomainEvent(new CitizenPhoneChangedDomainEvent(Id, newPhoneNumber));

        return Result.Success();
    }

    /// <summary>
    /// Thay đổi địa chỉ thường trú của công dân.
    /// </summary>
    /// <param name="newAddress">Địa chỉ mới.</param>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result ChangeAddress(Address newAddress)
    {
        Guard.Against.Null(newAddress, nameof(newAddress));

        if (Address == newAddress)
        {
            return Result.Success();
        }

        Address = newAddress;
        RaiseDomainEvent(new CitizenAddressChangedDomainEvent(Id, newAddress));

        return Result.Success();
    }

    /// <summary>
    /// Kích hoạt công dân.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Activate()
    {
        if (Status == CitizenStatus.Active)
        {
            return Result.Success();
        }

        Status = CitizenStatus.Active;
        return Result.Success();
    }

    /// <summary>
    /// Hủy kích hoạt công dân.
    /// </summary>
    /// <returns>Kết quả thành công hoặc lỗi.</returns>
    public Result Deactivate()
    {
        if (Status == CitizenStatus.Inactive)
        {
            return Result.Success();
        }

        Status = CitizenStatus.Inactive;
        return Result.Success();
    }
}
