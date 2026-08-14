using AnSinhSo.Application.Payments.DTOs;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Queries.SearchPayments;

public record SearchPaymentsQuery(SearchPaymentRequestDto Request) : IRequest<Result<PagedResult<PaymentDto>>>;
