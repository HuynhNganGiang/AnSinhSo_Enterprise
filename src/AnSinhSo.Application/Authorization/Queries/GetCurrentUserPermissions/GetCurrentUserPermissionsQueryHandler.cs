using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.Queries.GetUserPermissions;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetCurrentUserPermissions;

internal sealed class GetCurrentUserPermissionsQueryHandler : IRequestHandler<GetCurrentUserPermissionsQuery, Result<List<string>>>
{
    private readonly ISender _sender;
    private readonly ICurrentUser _currentUser;

    public GetCurrentUserPermissionsQueryHandler(ISender sender, ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    public async Task<Result<List<string>>> Handle(GetCurrentUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        var citizenId = Guid.TryParse(_currentUser.UserId, out var parsedId) ? parsedId : Guid.Empty;
        if (citizenId == Guid.Empty)
        {
            return Result.Failure<List<string>>(new Error("Unauthorized", "User is not authenticated.", ErrorType.Validation));
        }

        var result = await _sender.Send(new GetUserPermissionsQuery(citizenId), cancellationToken);
        if (result.IsFailure)
        {
            return Result.Failure<List<string>>(result.Error);
        }

        return Result.Success(new List<string>(result.Value));
    }
}
