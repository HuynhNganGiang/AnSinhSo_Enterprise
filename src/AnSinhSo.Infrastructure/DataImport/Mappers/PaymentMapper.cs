using System;
using AnSinhSo.Domain.Aggregates.PaymentAggregate;
using AnSinhSo.Infrastructure.DataImport.Models;

namespace AnSinhSo.Infrastructure.DataImport.Mappers;

public interface IPaymentMapper : IDataMapper<Stg_DotChiTraRecord, Payment> { }

public class PaymentMapper : IPaymentMapper
{
    public ImportResult<Payment> Map(ImportContext context, Stg_DotChiTraRecord dto)
    {
        if (!Guid.TryParse(dto.MaDotChiTra, out var id))
            return ImportResult<Payment>.Failure(ImportErrorCode.DATA_TYPE_MISMATCH, "Invalid MaDotChiTra (Guid).");

        var paymentId = new PaymentId(id);

        // The domain requires WelfareCaseId but Stg_DotChiTraRecord doesn't have it.
        // Architectural Decision #3 states: no dummy data, return Failure.
        return ImportResult<Payment>.Failure(ImportErrorCode.MISSING_REQUIRED_FIELD, "WelfareCaseId is required by Domain but missing in Stg_DotChiTra.");
    }
}
