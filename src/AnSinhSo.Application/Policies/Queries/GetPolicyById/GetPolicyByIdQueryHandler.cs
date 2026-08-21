using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Application.Common.Errors;

namespace AnSinhSo.Application.Policies.Queries.GetPolicyById;

/// <summary>
/// Handler xử lý truy vấn lấy thông tin chi tiết chính sách theo ID.
/// </summary>
public sealed class GetPolicyByIdQueryHandler : IRequestHandler<GetPolicyByIdQuery, Result>
{
    private readonly IPolicyRepository _policyRepository;

    /// <summary>
    /// Khởi tạo GetPolicyByIdQueryHandler.
    /// </summary>
    public GetPolicyByIdQueryHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    /// <summary>
    /// Xử lý truy vấn lấy thông tin chi tiết chính sách.
    /// </summary>
    public async Task<Result> Handle(GetPolicyByIdQuery request, CancellationToken cancellationToken)
    {
        var policyId = new PolicyId(request.PolicyId);
        var policy = await _policyRepository.GetByIdAsync(policyId, cancellationToken);

        if (policy is null)
        {
            return Result.Failure(Error.NotFound("Policy.NotFound", $"Chính sách {request.PolicyId} không tồn tại."));
        }

        // Pending: Step 17: Return DTO
        return Result.Success();
    }
}
