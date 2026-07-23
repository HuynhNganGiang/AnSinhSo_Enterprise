using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Common.Errors;

public static class DomainErrors
{
    public static Error NotFound(string objectName, object id)
    {
        return Error.NotFound($"{objectName}.NotFound", $"Không tìm thấy {objectName} với ID '{id}'.");
    }

    public static Error Validation(string code, string message)
    {
        return Error.Validation(code, message);
    }

    public static Error Conflict(string code, string message)
    {
        return Error.Conflict(code, message);
    }
}
