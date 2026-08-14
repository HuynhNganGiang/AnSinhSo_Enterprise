using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Authorization.DTOs;
using AnSinhSo.Domain.Interfaces.Authorization;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetRoles;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Result<IReadOnlyCollection<RoleDto>>>
{
    private readonly IRoleRepository _repository;

    public GetRolesQueryHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyCollection<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _repository.GetAllAsync(cancellationToken);

        var roleDtos = roles.Select(r => new RoleDto(
            r.Id.Value,
            r.Name,
            r.Description,
            r.IsSystemRole)).ToList();

        return Result.Success<IReadOnlyCollection<RoleDto>>(roleDtos);
    }
}
