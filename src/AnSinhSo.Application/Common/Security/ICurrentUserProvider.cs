namespace AnSinhSo.Application.Common.Security;

public interface ICurrentUserProvider
{
    CurrentUserInfo CurrentUser { get; }
}
