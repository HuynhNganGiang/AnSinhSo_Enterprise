using System;
using AnSinhSo.Domain.SeedWork.Events;

namespace AnSinhSo.Domain.Aggregates.WelfareCaseAggregate.Events;

public sealed record WelfareCaseCreatedEvent(WelfareCaseId WelfareCaseId) : IDomainEvent;
public sealed record WelfareCaseSubmittedEvent(WelfareCaseId WelfareCaseId) : IDomainEvent;
public sealed record WelfareCaseDecisionMadeEvent(WelfareCaseId WelfareCaseId, bool IsApproved) : IDomainEvent;
public sealed record WelfareCaseCancelledEvent(WelfareCaseId WelfareCaseId) : IDomainEvent;
public sealed record WelfareCaseClosedEvent(WelfareCaseId WelfareCaseId) : IDomainEvent;
