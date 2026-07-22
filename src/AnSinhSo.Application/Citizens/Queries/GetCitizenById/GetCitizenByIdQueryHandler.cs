using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.SeedWork.Errors;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenById;

/// <summary>
/// Handler xử lý truy vấn lấy thông tin chi tiết công dân theo ID.
/// </summary>
public sealed class GetCitizenByIdQueryHandler : IRequestHandler<GetCitizenByIdQuery, Result>
{
    private readonly ICitizenRepository _citizenRepository;

    /// <summary>
    /// Khởi tạo GetCitizenByIdQueryHandler.
    /// </summary>
    public GetCitizenByIdQueryHandler(ICitizenRepository citizenRepository)
    {
        _citizenRepository = citizenRepository;
    }

    /// <summary>
    /// Xử lý truy vấn thông tin chi tiết công dân.
    /// </summary>
    public async Task<Result> Handle(GetCitizenByIdQuery request, CancellationToken cancellationToken)
    {
        var citizenId = new CitizenId(request.CitizenId);

        var citizen = await _citizenRepository.GetByIdAsync(citizenId, cancellationToken);

        if (citizen is null)
        {
            return Result.Failure(DomainErrors.NotFound(nameof(Citizen), request.CitizenId));
        }

        // TODO Step 17: Return DTO

        return Result.Success();
    }
}
