using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;

namespace AnSinhSo.Application.Policies.Commands.DeactivatePolicy;

/// <summary>
/// Handler xử lý lệnh hủy kích hoạt chính sách.
/// </summary>
public sealed class DeactivatePolicyCommandHandler : IRequestHandler<DeactivatePolicyCommand, Result>
{
    private readonly IPolicyRepository _policyRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo DeactivatePolicyCommandHandler.
    /// </summary>
    public DeactivatePolicyCommandHandler(IPolicyRepository policyRepository, IUnitOfWork unitOfWork)
    {
        _policyRepository = policyRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý hủy kích hoạt chính sách.
    /// </summary>
    public async Task<Result> Handle(DeactivatePolicyCommand request, CancellationToken cancellationToken)
    {
        var policyId = new PolicyId(request.PolicyId);
        var policy = await _policyRepository.GetByIdAsync(policyId, cancellationToken);
        if (policy is null)
        {
            return Result.Failure(Error.NotFound("Policy.NotFound", $"Chính sách {request.PolicyId} không tồn tại."));
        }

        var result = policy.Deactivate();
        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
