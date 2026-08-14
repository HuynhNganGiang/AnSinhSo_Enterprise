using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;
using FluentValidation;

namespace AnSinhSo.Application.WelfareCases.Commands.MakeWelfareCaseDecision;

public record MakeWelfareCaseDecisionCommand(
    Guid WelfareCaseId,
    string Decision,
    string? Reason) : IRequest<Result>;

public class MakeWelfareCaseDecisionCommandValidator : AbstractValidator<MakeWelfareCaseDecisionCommand>
{
    public MakeWelfareCaseDecisionCommandValidator()
    {
        RuleFor(x => x.WelfareCaseId).NotEmpty();
        RuleFor(x => x.Decision)
            .NotEmpty()
            .Must(x => x == "Approved" || x == "Rejected")
            .WithMessage("Decision must be 'Approved' or 'Rejected'.");
    }
}

public class MakeWelfareCaseDecisionCommandHandler : IRequestHandler<MakeWelfareCaseDecisionCommand, Result>
{
    private readonly IWelfareCaseRepository _repository;

    public MakeWelfareCaseDecisionCommandHandler(IWelfareCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(MakeWelfareCaseDecisionCommand request, CancellationToken cancellationToken)
    {
        var id = new WelfareCaseId(request.WelfareCaseId);
        var welfareCase = await _repository.GetByIdAsync(id, cancellationToken);

        if (welfareCase == null)
        {
            return Result.Failure(Error.NotFound("WelfareCase.NotFound", "The specified welfare case was not found."));
        }

        bool isApproved = request.Decision == "Approved";
        var result = welfareCase.MakeDecision(isApproved, request.Reason);

        if (result.IsSuccess)
        {
            _repository.Update(welfareCase);
        }

        return result;
    }
}
