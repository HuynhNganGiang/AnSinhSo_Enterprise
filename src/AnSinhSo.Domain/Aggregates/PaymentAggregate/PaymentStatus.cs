using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

public class PaymentStatus : Enumeration
{
    public static readonly PaymentStatus Draft = new(1, "Draft");
    public static readonly PaymentStatus Pending = new(2, "Pending");
    public static readonly PaymentStatus Approved = new(3, "Approved");
    public static readonly PaymentStatus Processing = new(4, "Processing");
    public static readonly PaymentStatus Paid = new(5, "Paid");
    public static readonly PaymentStatus Failed = new(6, "Failed");
    public static readonly PaymentStatus Cancelled = new(7, "Cancelled");

    private PaymentStatus(int id, string name) : base(id, name)
    {
    }

    public static PaymentStatus FromId(int id)
    {
        return GetAll<PaymentStatus>().Single(x => x.Id == id);
    }
}
