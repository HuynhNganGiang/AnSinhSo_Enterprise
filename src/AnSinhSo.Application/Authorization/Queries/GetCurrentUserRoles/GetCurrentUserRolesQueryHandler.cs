using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetUserRoles;
using AnSinhSo.Contracts.Authorization;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetCurrentUserRoles;

internal sealed class GetCurrentUserRolesQueryHandler : IRequestHandler<GetCurrentUserRolesQuery, Result<List<RoleDto>>>
{
    private readonly ISender _sender;
    private readonly ICurrentUser _currentUser;

    public GetCurrentUserRolesQueryHandler(ISender sender, ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetCurrentUserRolesQuery request, CancellationToken cancellationToken)
    {
        var citizenId = Guid.TryParse(_currentUser.UserId, out var parsedId) ? parsedId : Guid.Empty;
        if (citizenId == Guid.Empty)
        {
            return Result.Failure<List<RoleDto>>(new Error("Unauthorized", "User is not authenticated.", ErrorType.Validation));
        }

        var result = await _sender.Send(new GetUserRolesQuery(citizenId), cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure<List<RoleDto>>(result.Error);
        }

        var mappedRoles = result.Value.Select(r => new RoleDto(r.Id, r.Name, r.Description, r.IsSystemRole)).ToList();
        return Result.Success(mappedRoles);
    }
}
