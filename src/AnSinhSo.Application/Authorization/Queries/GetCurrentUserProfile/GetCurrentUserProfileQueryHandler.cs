using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile;

internal sealed class GetCurrentUserProfileQueryHandler : IRequestHandler<GetCurrentUserProfileQuery, Result<CurrentUserProfileDto>>
{
    private readonly ICitizenIdentityRepository _citizenIdentityRepository;
    private readonly ICitizenRepository _citizenRepository;
    private readonly ICurrentUser _currentUser;

    public GetCurrentUserProfileQueryHandler(
        ICitizenIdentityRepository citizenIdentityRepository, 
        ICitizenRepository citizenRepository, 
        ICurrentUser currentUser)
    {
        _citizenIdentityRepository = citizenIdentityRepository;
        _citizenRepository = citizenRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<CurrentUserProfileDto>> Handle(GetCurrentUserProfileQuery request, CancellationToken cancellationToken)
    {
        var citizenIdentityId = Guid.TryParse(_currentUser.UserId, out var parsedId) ? parsedId : Guid.Empty;
        if (citizenIdentityId == Guid.Empty)
        {
            return Result.Failure<CurrentUserProfileDto>(new Error("Unauthorized", "User is not authenticated.", ErrorType.Validation));
        }

        var identity = await _citizenIdentityRepository.GetByIdAsync(new CitizenIdentityId(citizenIdentityId), cancellationToken);
        if (identity == null)
        {
            return Result.Failure<CurrentUserProfileDto>(new Error("NotFound", "User identity not found.", ErrorType.NotFound));
        }

        var citizen = await _citizenRepository.GetByIdAsync(identity.CitizenId, cancellationToken);
        if (citizen == null)
        {
            return Result.Failure<CurrentUserProfileDto>(new Error("NotFound", "User profile not found.", ErrorType.NotFound));
        }

        return Result.Success(new CurrentUserProfileDto(
            citizen.Id.Value,
            $"{citizen.FullName.FirstName} {citizen.FullName.MiddleName} {citizen.FullName.LastName}".Replace("  ", " ").Trim(),
            citizen.CitizenNumber.Value,
            identity.PrimaryPhone?.Value ?? string.Empty
        ));
    }
}
