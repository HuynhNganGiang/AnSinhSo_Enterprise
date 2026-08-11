namespace AnSinhSo.Application.Abstractions.Authentication;

public interface ITokenGenerator
{
    string GenerateRefreshToken();
}
