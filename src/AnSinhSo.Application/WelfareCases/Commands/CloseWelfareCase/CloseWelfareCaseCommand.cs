using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.WelfareCases.Commands.CloseWelfareCase;

public record CloseWelfareCaseCommand(
    Guid WelfareCaseId,
    string? Notes) : IRequest<Result>;

public class CloseWelfareCaseCommandHandler : IRequestHandler<CloseWelfareCaseCommand, Result>
{
    private readonly IWelfareCaseRepository _repository;

    public CloseWelfareCaseCommandHandler(IWelfareCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(CloseWelfareCaseCommand request, CancellationToken cancellationToken)
    {
        var id = new WelfareCaseId(request.WelfareCaseId);
        var welfareCase = await _repository.GetByIdAsync(id, cancellationToken);

        if (welfareCase == null)
        {
            return Result.Failure(Error.NotFound("WelfareCase.NotFound", "The specified welfare case was not found."));
        }

        var result = welfareCase.Close(request.Notes);

        if (result.IsSuccess)
        {
            _repository.Update(welfareCase);
        }

        return result;
    }
}
