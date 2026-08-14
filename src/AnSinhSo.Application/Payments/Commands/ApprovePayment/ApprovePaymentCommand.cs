using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.ApprovePayment;

public record ApprovePaymentCommand(Guid PaymentId) : IRequest<Result>;
