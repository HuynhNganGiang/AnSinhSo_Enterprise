using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Errors;

public static class IdentityErrors
{
    public static readonly Error CitizenNotFound = new(
        "Identity.CitizenNotFound",
        "Citizen profile does not exist in the Core Welfare Domain.",
        ErrorType.NotFound
    );

    public static readonly Error IdentityAlreadyExists = new(
        "Identity.IdentityAlreadyExists",
        "A CitizenIdentity has already been registered for this Citizen.",
        ErrorType.Conflict
    );

    public static readonly Error InvalidPhoneNumber = new(
        "Identity.InvalidPhoneNumber",
        "The provided phone number format is invalid.",
        ErrorType.Validation
    );

    public static readonly Error IdentityNotFound = new(
        "Identity.IdentityNotFound",
        "CitizenIdentity does not exist.",
        ErrorType.NotFound
    );
}
