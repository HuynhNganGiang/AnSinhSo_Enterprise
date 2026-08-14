using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Contracts.Common;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Payments.Commands.CreatePayment;

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Result<Guid>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IWelfareCaseRepository _welfareCaseRepository;

    public CreatePaymentCommandHandler(IPaymentRepository paymentRepository, IWelfareCaseRepository welfareCaseRepository)
    {
        _paymentRepository = paymentRepository;
        _welfareCaseRepository = welfareCaseRepository;
    }

    public async Task<Result<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var welfareCaseId = new WelfareCaseId(request.WelfareCaseId);
        var welfareCase = await _welfareCaseRepository.GetByIdAsync(welfareCaseId, cancellationToken);
        if (welfareCase is null)
            return Result.Failure<Guid>(Error.NotFound("WelfareCase.NotFound", "WelfareCase not found."));

        if (welfareCase.Status != WelfareStatus.Approved)
            return Result.Failure<Guid>(Error.Validation("WelfareCase.NotApproved", "Only approved welfare cases can be used to create payments."));

        // Generation of PaymentNumber (PAY-YYMMDD-XXXX)
        // Here we use a random string for XXXX just for simplicity, but in a real system it could be a sequence
        var randomStr = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        var paymentNumber = $"PAY-{DateTime.UtcNow:yyMMdd}-{randomStr}";

        var paymentId = PaymentId.New();
        var citizenId = new CitizenId(request.CitizenId);
        var householdId = request.HouseholdId.HasValue ? new HouseholdId(request.HouseholdId.Value) : (HouseholdId?)null;
        var method = PaymentMethod.FromId(request.MethodId);

        var paymentResult = Payment.Create(
            paymentId,
            paymentNumber,
            citizenId,
            householdId,
            welfareCaseId,
            request.Amount,
            request.ScheduledDate,
            method,
            request.Notes);

        if (paymentResult.IsFailure)
            return Result.Failure<Guid>(paymentResult.Error);

        _paymentRepository.Add(paymentResult.Value);
        return Result.Success(paymentId.Value);
    }
}
