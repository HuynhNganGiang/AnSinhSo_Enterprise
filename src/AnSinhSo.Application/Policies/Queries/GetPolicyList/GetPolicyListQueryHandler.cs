using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Policies.Queries.GetPolicyList;

/// <summary>
/// Handler xử lý truy vấn danh sách chính sách.
/// </summary>
public sealed class GetPolicyListQueryHandler : IRequestHandler<GetPolicyListQuery, Result>
{
    private readonly IPolicyRepository _policyRepository;

    /// <summary>
    /// Khởi tạo GetPolicyListQueryHandler.
    /// </summary>
    public GetPolicyListQueryHandler(IPolicyRepository policyRepository)
    {
        _policyRepository = policyRepository;
    }

    /// <summary>
    /// Xử lý truy vấn danh sách chính sách.
    /// </summary>
    public async Task<Result> Handle(GetPolicyListQuery request, CancellationToken cancellationToken)
    {
        // Pending: Step 17: Repository call and Mapping to DTO
        return await Task.FromResult(Result.Success());
    }
}
