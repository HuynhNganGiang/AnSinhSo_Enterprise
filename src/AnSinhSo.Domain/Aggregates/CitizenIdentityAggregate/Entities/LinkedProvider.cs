using System;
using AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Enumerations;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.CitizenIdentityAggregate.Entities;

public sealed class LinkedProvider : Entity<Guid>
{
    public ProviderType ProviderType { get; private set; }
    public string SubjectId { get; private set; } = string.Empty;
    public DateTime LinkedAt { get; private set; }

    private LinkedProvider() { } // ORM

    internal LinkedProvider(Guid id, ProviderType providerType, string subjectId, DateTime linkedAt)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(subjectId))
        {
            throw new ArgumentException("SubjectId cannot be empty.", nameof(subjectId));
        }

        ProviderType = providerType;
        SubjectId = subjectId;
        LinkedAt = linkedAt;
    }
}
