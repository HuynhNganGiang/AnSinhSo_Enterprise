using System.Collections.Generic;
using System.Linq;
using AnSinhSo.Domain.Enumerations;

namespace AnSinhSo.Domain.Aggregates.PaymentAggregate;

public class PaymentMethod : Enumeration
{
    public static readonly PaymentMethod Cash = new(1, "Cash");
    public static readonly PaymentMethod BankTransfer = new(2, "BankTransfer");
    public static readonly PaymentMethod Postal = new(3, "Postal");

    private PaymentMethod(int id, string name) : base(id, name)
    {
    }

    public static PaymentMethod FromId(int id)
    {
        return GetAll<PaymentMethod>().Single(x => x.Id == id);
    }
}
