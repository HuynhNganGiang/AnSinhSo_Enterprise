using System;
using System.Collections.Generic;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.GetHouseholdPayments;

public record GetHouseholdPaymentsQuery(Guid HouseholdId) : IRequest<Result<IReadOnlyList<PaymentDto>>>;
