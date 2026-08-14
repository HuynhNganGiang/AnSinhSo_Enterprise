using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.WelfareCases.Commands.UpdateWelfareCase;

public record UpdateWelfareCaseCommand(
    Guid WelfareCaseId,
    string? Notes,
    decimal? BenefitAmount,
    DateTime? EffectiveFrom,
    DateTime? EffectiveTo) : IRequest<Result>;

public class UpdateWelfareCaseCommandHandler : IRequestHandler<UpdateWelfareCaseCommand, Result>
{
    private readonly IWelfareCaseRepository _repository;

    public UpdateWelfareCaseCommandHandler(IWelfareCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(UpdateWelfareCaseCommand request, CancellationToken cancellationToken)
    {
        var id = new WelfareCaseId(request.WelfareCaseId);
        var welfareCase = await _repository.GetByIdAsync(id, cancellationToken);

        if (welfareCase == null)
        {
            return Result.Failure(Error.NotFound("WelfareCase.NotFound", "The specified welfare case was not found."));
        }

        var result = welfareCase.UpdateDetails(request.Notes, request.BenefitAmount, request.EffectiveFrom, request.EffectiveTo);

        if (result.IsSuccess)
        {
            _repository.Update(welfareCase);
        }

        return result;
    }
}
