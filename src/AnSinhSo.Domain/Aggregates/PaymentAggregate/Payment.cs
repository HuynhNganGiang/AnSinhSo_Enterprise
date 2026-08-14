using System;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.Events.PaymentEvents;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

public sealed class Payment : AggregateRoot<PaymentId>
{
    public string PaymentNumber { get; private set; }
    public CitizenId CitizenId { get; private set; }
    public HouseholdId? HouseholdId { get; private set; }
    public WelfareCaseId WelfareCaseId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public DateTime? ActualPaymentDate { get; private set; }
    public PaymentMethod Method { get; private set; }
    public PaymentStatus Status { get; private set; }
    public string? Notes { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Payment() { }
#pragma warning restore CS8618

    private Payment(
        PaymentId id,
        string paymentNumber,
        CitizenId citizenId,
        HouseholdId? householdId,
        WelfareCaseId welfareCaseId,
        decimal amount,
        DateTime scheduledDate,
        PaymentMethod method,
        string? notes) : base(id)
    {
        PaymentNumber = paymentNumber;
        CitizenId = citizenId;
        HouseholdId = householdId;
        WelfareCaseId = welfareCaseId;
        Amount = amount;
        ScheduledDate = scheduledDate;
        Method = method;
        Notes = notes;
        Status = PaymentStatus.Draft;
    }

    public static Result<Payment> Create(
        PaymentId id,
        string paymentNumber,
        CitizenId citizenId,
        HouseholdId? householdId,
        WelfareCaseId welfareCaseId,
        decimal amount,
        DateTime scheduledDate,
        PaymentMethod method,
        string? notes = null)
    {
        Guard.Against.Null(id, nameof(id));
        if (string.IsNullOrWhiteSpace(paymentNumber)) return Result.Failure<Payment>(Error.Validation("Payment.PaymentNumberEmpty", "Payment number cannot be empty."));
        Guard.Against.Null(citizenId, nameof(citizenId));
        Guard.Against.Null(welfareCaseId, nameof(welfareCaseId));
        if (amount <= 0) return Result.Failure<Payment>(Error.Validation("Payment.AmountInvalid", "Amount must be greater than zero."));

        var payment = new Payment(id, paymentNumber, citizenId, householdId, welfareCaseId, amount, scheduledDate, method, notes);
        payment.RaiseDomainEvent(new PaymentCreatedDomainEvent(payment.Id));

        return Result.Success(payment);
    }

    public Result Submit()
    {
        if (Status != PaymentStatus.Draft)
            return Result.Failure(Error.Failure("Payment.SubmitInvalid", "Only Draft payments can be submitted."));

        Status = PaymentStatus.Pending;
        return Result.Success();
    }

    public Result Approve()
    {
        if (Status != PaymentStatus.Pending)
            return Result.Failure(Error.Failure("Payment.ApproveInvalid", "Only Pending payments can be approved."));

        Status = PaymentStatus.Approved;
        RaiseDomainEvent(new PaymentApprovedDomainEvent(Id));
        return Result.Success();
    }

    public Result StartProcessing()
    {
        if (Status != PaymentStatus.Approved)
            return Result.Failure(Error.Failure("Payment.StartProcessingInvalid", "Only Approved payments can start processing."));

        Status = PaymentStatus.Processing;
        return Result.Success();
    }

    public Result Complete(DateTime actualPaymentDate)
    {
        if (Status != PaymentStatus.Processing)
            return Result.Failure(Error.Failure("Payment.CompleteInvalid", "Only Processing payments can be completed."));

        Status = PaymentStatus.Paid;
        ActualPaymentDate = actualPaymentDate;
        RaiseDomainEvent(new PaymentCompletedDomainEvent(Id));
        return Result.Success();
    }

    public Result Fail(string reason)
    {
        if (Status != PaymentStatus.Processing)
            return Result.Failure(Error.Failure("Payment.FailInvalid", "Only Processing payments can be marked as failed."));

        Status = PaymentStatus.Failed;
        Notes = !string.IsNullOrEmpty(Notes) ? $"{Notes} | Failed Reason: {reason}" : $"Failed Reason: {reason}";
        RaiseDomainEvent(new PaymentFailedDomainEvent(Id));
        return Result.Success();
    }

    public Result Cancel(string reason)
    {
        if (Status == PaymentStatus.Paid || Status == PaymentStatus.Failed || Status == PaymentStatus.Cancelled)
            return Result.Failure(Error.Failure("Payment.CancelInvalid", "Cannot cancel a payment that is already Paid, Failed, or Cancelled."));

        Status = PaymentStatus.Cancelled;
        Notes = !string.IsNullOrEmpty(Notes) ? $"{Notes} | Cancel Reason: {reason}" : $"Cancel Reason: {reason}";
        RaiseDomainEvent(new PaymentCancelledDomainEvent(Id));
        return Result.Success();
    }
}
