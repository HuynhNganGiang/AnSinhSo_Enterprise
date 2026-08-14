using System.Collections.Generic;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.GetPendingPayments;

public record GetPendingPaymentsQuery() : IRequest<Result<IReadOnlyList<PaymentDto>>>;
