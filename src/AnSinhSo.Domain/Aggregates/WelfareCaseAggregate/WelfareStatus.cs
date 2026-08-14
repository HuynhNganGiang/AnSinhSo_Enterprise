using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.Enumerations;
using System.Linq;

namespace AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;

public class WelfareStatus : Enumeration
{
    public static readonly WelfareStatus Draft = new(1, nameof(Draft));
    public static readonly WelfareStatus Submitted = new(2, nameof(Submitted));
    public static readonly WelfareStatus UnderReview = new(3, nameof(UnderReview));
    public static readonly WelfareStatus Approved = new(4, nameof(Approved));
    public static readonly WelfareStatus Rejected = new(5, nameof(Rejected));
    public static readonly WelfareStatus Cancelled = new(6, nameof(Cancelled));
    public static readonly WelfareStatus Closed = new(7, nameof(Closed));

    private WelfareStatus(int id, string name) : base(id, name)
    {
    }

    public static WelfareStatus FromId(int id)
    {
        return GetAll<WelfareStatus>().FirstOrDefault(x => x.Id == id) 
               ?? throw new System.InvalidOperationException($"WelfareStatus with id {id} not found");
    }
}
