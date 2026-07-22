using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Application.Policies.Commands.UpdatePolicyAmount;

/// <summary>
/// Handler xử lý lệnh cập nhật số tiền hỗ trợ của chính sách.
/// </summary>
public sealed class UpdatePolicyAmountCommandHandler : IRequestHandler<UpdatePolicyAmountCommand, Result>
{
    private readonly IPolicyRepository _policyRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo UpdatePolicyAmountCommandHandler.
    /// </summary>
    public UpdatePolicyAmountCommandHandler(IPolicyRepository policyRepository, IUnitOfWork unitOfWork)
    {
        _policyRepository = policyRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý lệnh cập nhật số tiền hỗ trợ.
    /// </summary>
    public async Task<Result> Handle(UpdatePolicyAmountCommand request, CancellationToken cancellationToken)
    {
        var policyId = new PolicyId(request.PolicyId);
        var policy = await _policyRepository.GetByIdAsync(policyId, cancellationToken);
        if (policy is null)
        {
            return Result.Failure(Error.NotFound("Policy.NotFound", $"Chính sách {request.PolicyId} không tồn tại."));
        }

        var moneyResult = Money.Create(request.Amount, Currency.VND);
        if (moneyResult.IsFailure)
        {
            return Result.Failure(moneyResult.Error);
        }

        var updateResult = policy.UpdateAmount(moneyResult.Value);
        if (updateResult.IsFailure)
        {
            return Result.Failure(updateResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
