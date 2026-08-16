using System;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.PaymentPointAggregate;

public sealed class PaymentPoint : AggregateRoot<PaymentPointId>
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public Address Address { get; private set; }
    public Location? Location { get; private set; }
    public PaymentPointStatus Status { get; private set; }
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }

#pragma warning disable CS8618
    private PaymentPoint() { }
#pragma warning restore CS8618

    private PaymentPoint(
        PaymentPointId id,
        string code,
        string name,
        Address address,
        Location? location,
        string? description,
        int displayOrder) : base(id)
    {
        Code = code;
        Name = name;
        Address = address;
        Location = location;
        Description = description;
        DisplayOrder = displayOrder;
        Status = PaymentPointStatus.Active;
    }

    public static Result<PaymentPoint> Create(
        PaymentPointId id,
        string code,
        string name,
        Address address,
        Location? location = null,
        string? description = null,
        int displayOrder = 0)
    {
        Guard.Against.Null(id, nameof(id));
        if (string.IsNullOrWhiteSpace(code)) return Result.Failure<PaymentPoint>(Error.Validation("PaymentPoint.CodeEmpty", "Mã điểm chi trả không được để trống."));
        if (string.IsNullOrWhiteSpace(name)) return Result.Failure<PaymentPoint>(Error.Validation("PaymentPoint.NameEmpty", "Tên điểm chi trả không được để trống."));
        Guard.Against.Null(address, nameof(address));

        var paymentPoint = new PaymentPoint(id, code, name, address, location, description, displayOrder);
        // Domain events can be added later if needed.
        return Result.Success(paymentPoint);
    }

    public Result UpdateLocation(Location location)
    {
        Guard.Against.Null(location, nameof(location));
        Location = location;
        return Result.Success();
    }
}
