using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate.ValueObjects;

namespace AnSinhSo.Domain.Interfaces;

public interface IUserSessionRepository
{
    Task<UserSession?> GetByIdAsync(UserSessionId id, CancellationToken cancellationToken = default);
    Task<UserSession?> GetByRefreshTokenHashAsync(string refreshTokenHash, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserSession>> GetActiveSessionsByCitizenAsync(Guid citizenIdentityId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserSession>> GetFamilySessionsAsync(Guid refreshTokenFamilyId, CancellationToken cancellationToken = default);
    
    void Add(UserSession session);
    void Update(UserSession session);
}
