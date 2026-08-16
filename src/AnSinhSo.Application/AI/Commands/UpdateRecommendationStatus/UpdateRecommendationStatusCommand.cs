using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Interfaces;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Commands.UpdateRecommendationStatus;

public record UpdateRecommendationStatusCommand(Guid Id, AiRecommendationStatus Status, string ReviewedBy) : IRequest<Result>;

public class UpdateRecommendationStatusCommandHandler : IRequestHandler<UpdateRecommendationStatusCommand, Result>
{
    private readonly IAiRecommendationRepository _repository;

    public UpdateRecommendationStatusCommandHandler(IAiRecommendationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(UpdateRecommendationStatusCommand request, CancellationToken cancellationToken)
    {
        var id = AiRecommendationId.Create(request.Id);
        var recommendation = await _repository.GetByIdAsync(id, cancellationToken);

        if (recommendation == null)
            return Result.Failure(Error.NotFound("AiRecommendation.NotFound", "Không tìm thấy kết quả phân tích AI."));

        if (request.Status == AiRecommendationStatus.Reviewed)
        {
            recommendation.MarkAsReviewed(request.ReviewedBy);
        }
        else if (request.Status == AiRecommendationStatus.Dismissed)
        {
            recommendation.Dismiss(request.ReviewedBy);
        }
        else
        {
            return Result.Failure(Error.Validation("AiRecommendation.InvalidStatus", "Trạng thái cập nhật không hợp lệ."));
        }

        _repository.Update(recommendation);
        // SaveChanges is handled by UnitOfWork Pipeline Behavior usually

        return Result.Success();
    }
}
