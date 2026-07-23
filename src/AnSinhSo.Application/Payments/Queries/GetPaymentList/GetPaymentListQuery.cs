using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Payments.Queries.GetPaymentList;

public sealed record GetPaymentListQuery(
    int PageNumber = 1,
    int PageSize = 10,
    Guid? PolicyId = null) : IRequest<Result>;
