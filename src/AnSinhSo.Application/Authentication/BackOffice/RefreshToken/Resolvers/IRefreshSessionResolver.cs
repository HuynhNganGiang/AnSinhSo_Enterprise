using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Authentication.BackOffice.RefreshToken.Resolvers;

public interface IRefreshSessionResolver
{
    Task<Result<RefreshSessionContext>> ResolveAsync(string hashedRefreshToken, CancellationToken cancellationToken = default);
    Task RotateAsync(RefreshSessionContext context, string newHashedRefreshToken, DateTime expiryDate, CancellationToken cancellationToken = default);
}
