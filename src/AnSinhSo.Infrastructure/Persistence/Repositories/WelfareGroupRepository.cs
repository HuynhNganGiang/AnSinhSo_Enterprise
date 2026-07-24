using System;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Abstractions.Persistence;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class WelfareGroupRepository : Repository<WelfareGroup, WelfareGroupId>, IWelfareGroupRepository
{
    public WelfareGroupRepository(AnSinhSoDbContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> ExistsAsync(WelfareGroupId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(WelfareGroup entity)
    {
        throw new NotImplementedException();
    }
}
