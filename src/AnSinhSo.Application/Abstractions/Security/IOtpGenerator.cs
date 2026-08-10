namespace AnSinhSo.Application.Abstractions.Security;

public interface IOtpGenerator
{
    string Generate(int length = 6);
}
