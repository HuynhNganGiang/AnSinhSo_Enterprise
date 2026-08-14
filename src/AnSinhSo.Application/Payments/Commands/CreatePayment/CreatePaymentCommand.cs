using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.CreatePayment;

public record CreatePaymentCommand(
    Guid CitizenId,
    Guid? HouseholdId,
    Guid WelfareCaseId,
    decimal Amount,
    DateTime ScheduledDate,
    int MethodId,
    string? Notes
) : IRequest<Result<Guid>>;
