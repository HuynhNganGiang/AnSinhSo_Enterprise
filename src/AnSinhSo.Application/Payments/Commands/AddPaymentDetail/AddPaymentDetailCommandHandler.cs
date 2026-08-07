using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Application.Common.Errors;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Enumerations;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.ValueObjects;

namespace AnSinhSo.Application.Payments.Commands.AddPaymentDetail;

public sealed class AddPaymentDetailCommandHandler : IRequestHandler<AddPaymentDetailCommand, Result>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPaymentDetailCommandHandler(
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddPaymentDetailCommand request, CancellationToken cancellationToken)
    {
        var paymentId = new PaymentId(request.PaymentId);
        var payment = await _paymentRepository.GetWithDetailsAsync(paymentId, cancellationToken);
        if (payment is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Payment), request.PaymentId));
        }

        var detailId = new PaymentDetailId(request.PaymentDetailId);
        var citizenId = new CitizenId(request.CitizenId);

        var moneyResult = Money.Create(request.Amount, Currency.VND);
        if (moneyResult.IsFailure)
        {
            return Result.Failure(moneyResult.Error);
        }

        var detailResult = PaymentDetail.Create(detailId, citizenId, moneyResult.Value);
        if (detailResult.IsFailure)
        {
            return Result.Failure(detailResult.Error);
        }

        var addDetailResult = payment.AddDetail(detailResult.Value);
        if (addDetailResult.IsFailure)
        {
            return Result.Failure(addDetailResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
