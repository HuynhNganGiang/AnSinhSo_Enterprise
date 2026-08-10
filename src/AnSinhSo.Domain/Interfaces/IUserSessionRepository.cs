using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.UserSessionAggregate;

namespace AnSinhSo.Domain.Interfaces;

public interface IUserSessionRepository
{
    Task<UserSession?> GetByTokenHashAsync(string tokenHash, CancellationToken ct = default);
    Task<UserSession?> GetByIdAsync(UserSessionId id, CancellationToken ct = default);
    Task<List<UserSession>> GetActiveSessionsByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<List<UserSession>> GetSessionsByFamilyIdAsync(Guid familyId, CancellationToken ct = default);
    
    Task<bool> ExistsActiveSessionAsync(Guid userId, string deviceId, CancellationToken ct = default);
    Task<int> GetActiveSessionCountAsync(Guid userId, CancellationToken ct = default);

    void Add(UserSession session);
    void Update(UserSession session);
}
