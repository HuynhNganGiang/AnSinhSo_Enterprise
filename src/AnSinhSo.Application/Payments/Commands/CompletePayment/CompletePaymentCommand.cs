using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.CompletePayment;

public record CompletePaymentCommand(Guid PaymentId, DateTime ActualPaymentDate) : IRequest<Result>;
