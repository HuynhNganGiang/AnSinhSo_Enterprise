using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Common.Interfaces.Security;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.ValueObjects;
using AnSinhSo.Domain.Errors;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.CitizenIdentities.Commands.RegisterCitizenIdentity;

public sealed class RegisterCitizenIdentityCommandHandler 
    : IRequestHandler<RegisterCitizenIdentityCommand, Result<Guid>>
{
    private readonly ICitizenRepository _citizenRepository;
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISecurityStampGenerator _securityStampGenerator;

    public RegisterCitizenIdentityCommandHandler(
        ICitizenRepository citizenRepository,
        ICitizenIdentityRepository citizenIdentityRepository,
        IUnitOfWork unitOfWork,
        ISecurityStampGenerator securityStampGenerator)
    {
        _citizenRepository = citizenRepository;
        _citizenIdentityRepository = citizenIdentityRepository;
        _unitOfWork = unitOfWork;
        _securityStampGenerator = securityStampGenerator;
    }

    public async Task<Result<Guid>> Handle(RegisterCitizenIdentityCommand request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);

        // AD #33: Citizen is Read-Only here
        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);
        if (citizen is null)
        {
            return Result.Failure<Guid>(IdentityErrors.CitizenNotFound);
        }

        // AD #30: Idempotent check
        var existingIdentity = await _citizenIdentityRepository.GetByCitizenIdAsync(citizenId, cancellationToken);
        if (existingIdentity is not null)
        {
            return Result.Failure<Guid>(IdentityErrors.IdentityAlreadyExists);
        }

        // AD #29: Generate Pure Dependencies
        var identityId = CitizenIdentityId.Create(Guid.NewGuid());
        var securityStamp = _securityStampGenerator.Generate();

        // AD #32 & AD #37: Init with phone number but Status remains PendingVerification
        var identity = CitizenIdentity.Create(
            identityId, 
            citizenId, 
            securityStamp, 
            PhoneNumber.Create(request.PhoneNumber));

        // AD #34: No direct event publishing
        _citizenIdentityRepository.Add(identity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // AD #35: Return Guid
        return Result.Success(identityId.Value);
    }
}
