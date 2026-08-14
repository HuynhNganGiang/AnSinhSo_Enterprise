using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.CancelPayment;

public record CancelPaymentCommand(Guid PaymentId, string Reason) : IRequest<Result>;
