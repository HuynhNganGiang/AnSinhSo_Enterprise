using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;

public interface IRelationshipTypeRepository
{
    Task<RelationshipType?> GetByIdAsync(RelationshipTypeId id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(RelationshipTypeId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RelationshipType>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(RelationshipType relationshipType);
}
