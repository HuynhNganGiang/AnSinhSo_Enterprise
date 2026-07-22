using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Application.Policies.Commands.CreatePolicy;

/// <summary>
/// Handler xử lý lệnh tạo mới chính sách.
/// </summary>
public sealed class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Result<Guid>>
{
    private readonly IPolicyRepository _policyRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Khởi tạo CreatePolicyCommandHandler.
    /// </summary>
    public CreatePolicyCommandHandler(IPolicyRepository policyRepository, IUnitOfWork unitOfWork)
    {
        _policyRepository = policyRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Xử lý tạo mới chính sách.
    /// </summary>
    public async Task<Result<Guid>> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
    {
        var moneyResult = Money.Create(request.Amount, Currency.VND);
        if (moneyResult.IsFailure)
        {
            return Result.Failure<Guid>(moneyResult.Error);
        }

        var policyId = new PolicyId(Guid.NewGuid());
        var policyResult = Policy.Create(policyId, request.Name, request.Description, moneyResult.Value);
        if (policyResult.IsFailure)
        {
            return Result.Failure<Guid>(policyResult.Error);
        }

        await _policyRepository.AddAsync(policyResult.Value, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(policyResult.Value.Id.Value);
    }
}
