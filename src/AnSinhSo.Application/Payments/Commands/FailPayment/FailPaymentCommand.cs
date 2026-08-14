using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.FailPayment;

public record FailPaymentCommand(Guid PaymentId, string Reason) : IRequest<Result>;
