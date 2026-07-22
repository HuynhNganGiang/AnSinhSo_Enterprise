using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Queries.GetCitizenList;

/// <summary>
/// Handler xử lý truy vấn danh sách công dân.
/// </summary>
public sealed class GetCitizenListQueryHandler : IRequestHandler<GetCitizenListQuery, Result>
{
    private readonly ICitizenRepository _citizenRepository;

    /// <summary>
    /// Khởi tạo GetCitizenListQueryHandler.
    /// </summary>
    public GetCitizenListQueryHandler(ICitizenRepository citizenRepository)
    {
        _citizenRepository = citizenRepository;
    }

    /// <summary>
    /// Xử lý truy vấn danh sách công dân.
    /// </summary>
    public async Task<Result> Handle(GetCitizenListQuery request, CancellationToken cancellationToken)
    {
        // TODO Step 17: Repository call and Mapping to DTO
        return await Task.FromResult(Result.Success());
    }
}
