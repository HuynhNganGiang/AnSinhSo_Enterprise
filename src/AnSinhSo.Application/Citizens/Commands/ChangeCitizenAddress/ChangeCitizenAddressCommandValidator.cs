using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using FluentValidation;

namespace AnSinhSo.Application.Citizens.Commands.ChangeCitizenAddress;

/// <summary>
/// Validator cho ChangeCitizenAddressCommand.
/// </summary>
public sealed class ChangeCitizenAddressCommandValidator : AbstractValidator<ChangeCitizenAddressCommand>
{
    /// <summary>
    /// Khởi tạo ChangeCitizenAddressCommandValidator.
    /// </summary>
    public ChangeCitizenAddressCommandValidator()
    {
        RuleFor(x => x.CitizenId)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotEmpty();
    }
}
