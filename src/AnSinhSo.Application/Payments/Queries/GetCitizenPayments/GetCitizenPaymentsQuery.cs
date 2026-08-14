using System;
using System.Collections.Generic;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.GetCitizenPayments;

public record GetCitizenPaymentsQuery(Guid CitizenId) : IRequest<Result<IReadOnlyList<PaymentDto>>>;
