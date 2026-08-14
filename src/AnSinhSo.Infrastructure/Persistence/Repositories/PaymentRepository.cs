using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AnSinhSo.Infrastructure.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AnSinhSoDbContext _context;

    public PaymentRepository(AnSinhSoDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetByIdAsync(PaymentId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Payment>()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(PaymentId id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Payment>()
            .AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Payment> Items, int TotalCount)> SearchAsync(
        string? keyword,
        Guid? citizenId,
        Guid? householdId,
        Guid? welfareCaseId,
        int? statusId,
        int? methodId,
        DateTime? fromDate,
        DateTime? toDate,
        int page,
        int pageSize,
        string? sort,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Set<Payment>().AsQueryable();

        if (citizenId.HasValue)
        {
            var cId = new CitizenId(citizenId.Value);
            query = query.Where(x => x.CitizenId == cId);
        }

        if (householdId.HasValue)
        {
            var hId = new HouseholdId(householdId.Value);
            query = query.Where(x => x.HouseholdId == hId);
        }

        if (welfareCaseId.HasValue)
        {
            var wId = new WelfareCaseId(welfareCaseId.Value);
            query = query.Where(x => x.WelfareCaseId == wId);
        }

        if (statusId.HasValue)
        {
            var status = PaymentStatus.FromId(statusId.Value);
            query = query.Where(x => x.Status == status);
        }

        if (methodId.HasValue)
        {
            var method = PaymentMethod.FromId(methodId.Value);
            query = query.Where(x => x.Method == method);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(x => x.ScheduledDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(x => x.ScheduledDate <= toDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.PaymentNumber.Contains(keyword));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(sort))
        {
            query = query.OrderByDescending(x => x.CreatedAt);
        }
        else
        {
            query = sort.ToLower() switch
            {
                "createdat_asc" => query.OrderBy(x => x.CreatedAt),
                "createdat_desc" => query.OrderByDescending(x => x.CreatedAt),
                "scheduleddate_asc" => query.OrderBy(x => x.ScheduledDate),
                "scheduleddate_desc" => query.OrderByDescending(x => x.ScheduledDate),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };
        }

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Payment>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Payment>()
            .Where(x => x.Status == PaymentStatus.Pending)
            .ToListAsync(cancellationToken);
    }

    public void Add(Payment payment)
    {
        _context.Set<Payment>().Add(payment);
    }

    public void Update(Payment payment)
    {
        _context.Set<Payment>().Update(payment);
    }
}
