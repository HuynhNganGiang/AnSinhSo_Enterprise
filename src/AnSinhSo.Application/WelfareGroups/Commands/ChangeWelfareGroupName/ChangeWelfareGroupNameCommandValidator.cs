using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using FluentValidation;

namespace AnSinhSo.Application.WelfareGroups.Commands.ChangeWelfareGroupName;

/// <summary>
/// Validator cho ChangeWelfareGroupNameCommand.
/// </summary>
public sealed class ChangeWelfareGroupNameCommandValidator : AbstractValidator<ChangeWelfareGroupNameCommand>
{
    /// <summary>
    /// Khởi tạo ChangeWelfareGroupNameCommandValidator.
    /// </summary>
    public ChangeWelfareGroupNameCommandValidator()
    {
        RuleFor(x => x.WelfareGroupId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}
