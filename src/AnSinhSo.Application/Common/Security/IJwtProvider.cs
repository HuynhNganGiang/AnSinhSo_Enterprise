using AnSinhSo.Domain.Aggregates.UserAggregate;

namespace AnSinhSo.Application.Common.Security;

public interface IJwtProvider
{
    TokenResult Generate(User user);
}
