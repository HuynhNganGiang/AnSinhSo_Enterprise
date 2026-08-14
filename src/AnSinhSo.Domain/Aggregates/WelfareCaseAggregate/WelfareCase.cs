using System;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;

public sealed class WelfareCase : AggregateRoot<WelfareCaseId>
{
    public CitizenId CitizenId { get; private set; }
    public HouseholdId? HouseholdId { get; private set; }
    public WelfareProgramId ProgramId { get; private set; }
    
    public CitizenSnapshot CitizenSnapshot { get; private set; }
    public WelfareStatus Status { get; private set; }
    public string? Notes { get; private set; }

    public decimal? BenefitAmount { get; private set; }
    public DateTime? EffectiveFrom { get; private set; }
    public DateTime? EffectiveTo { get; private set; }

#pragma warning disable CS8618
    private WelfareCase() { }
#pragma warning restore CS8618

    private WelfareCase(
        WelfareCaseId id,
        CitizenId citizenId,
        HouseholdId? householdId,
        WelfareProgramId programId,
        CitizenSnapshot snapshot) : base(id)
    {
        CitizenId = citizenId;
        HouseholdId = householdId;
        ProgramId = programId;
        CitizenSnapshot = snapshot;
        Status = WelfareStatus.Draft;
    }

    public static Result<WelfareCase> Create(
        WelfareCaseId id,
        CitizenId citizenId,
        HouseholdId? householdId,
        WelfareProgramId programId,
        CitizenSnapshot snapshot)
    {
        Guard.Against.Null(citizenId, nameof(citizenId));
        Guard.Against.Null(programId, nameof(programId));
        Guard.Against.Null(snapshot, nameof(snapshot));

        var welfareCase = new WelfareCase(id, citizenId, householdId, programId, snapshot);
        
        welfareCase.RaiseDomainEvent(new Events.WelfareCaseCreatedEvent(welfareCase.Id));
        return Result.Success(welfareCase);
    }

    public Result UpdateDetails(string? notes, decimal? benefitAmount, DateTime? effectiveFrom, DateTime? effectiveTo)
    {
        if (Status != WelfareStatus.Draft)
        {
            return Result.Failure(Error.Failure("WelfareCase.CannotUpdate", "Can only update notes/benefits when in Draft status."));
        }

        Notes = notes;
        BenefitAmount = benefitAmount;
        EffectiveFrom = effectiveFrom;
        EffectiveTo = effectiveTo;

        return Result.Success();
    }

    public Result Submit(string? notes)
    {
        if (Status != WelfareStatus.Draft)
        {
            return Result.Failure(Error.Failure("WelfareCase.InvalidStateTransition", "Only Draft cases can be submitted."));
        }

        Status = WelfareStatus.Submitted;
        if (!string.IsNullOrEmpty(notes)) Notes = notes;

        RaiseDomainEvent(new Events.WelfareCaseSubmittedEvent(Id));
        return Result.Success();
    }

    public Result MakeDecision(bool isApproved, string? notes)
    {
        if (Status != WelfareStatus.Submitted && Status != WelfareStatus.UnderReview)
        {
            return Result.Failure(Error.Failure("WelfareCase.InvalidStateTransition", "Can only make decision on Submitted or UnderReview cases."));
        }

        Status = isApproved ? WelfareStatus.Approved : WelfareStatus.Rejected;
        if (!string.IsNullOrEmpty(notes)) Notes = notes;

        RaiseDomainEvent(new Events.WelfareCaseDecisionMadeEvent(Id, isApproved));
        return Result.Success();
    }

    public Result Cancel(string? notes)
    {
        if (Status == WelfareStatus.Closed || Status == WelfareStatus.Rejected || Status == WelfareStatus.Approved)
        {
            return Result.Failure(Error.Failure("WelfareCase.InvalidStateTransition", "Cannot cancel an already completed or approved case."));
        }

        Status = WelfareStatus.Cancelled;
        if (!string.IsNullOrEmpty(notes)) Notes = notes;

        RaiseDomainEvent(new Events.WelfareCaseCancelledEvent(Id));
        return Result.Success();
    }

    public Result Close(string? notes)
    {
        if (Status != WelfareStatus.Approved && Status != WelfareStatus.Rejected)
        {
            return Result.Failure(Error.Failure("WelfareCase.InvalidStateTransition", "Can only close Approved or Rejected cases."));
        }

        Status = WelfareStatus.Closed;
        if (!string.IsNullOrEmpty(notes)) Notes = notes;

        RaiseDomainEvent(new Events.WelfareCaseClosedEvent(Id));
        return Result.Success();
    }
}
