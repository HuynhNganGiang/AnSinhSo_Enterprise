using System;
using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.GetPaymentById;

public record GetPaymentByIdQuery(Guid PaymentId) : IRequest<Result<PaymentDto>>;
