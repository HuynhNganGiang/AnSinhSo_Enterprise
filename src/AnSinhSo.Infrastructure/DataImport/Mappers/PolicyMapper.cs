using System;
using AnSinhSo.Domain.Aggregates.PolicyAggregate;
using AnSinhSo.Domain.Enumerations;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Infrastructure.DataImport.Models;

namespace AnSinhSo.Infrastructure.DataImport.Mappers;

public interface IPolicyMapper : IDataMapper<Stg_ChinhSachTroCapRecord, Policy> { }

public class PolicyMapper : IPolicyMapper
{
    public ImportResult<Policy> Map(ImportContext context, Stg_ChinhSachTroCapRecord dto)
    {
        if (!Guid.TryParse(dto.MaChinhSach, out var id))
            return ImportResult<Policy>.Failure(ImportErrorCode.DATA_TYPE_MISMATCH, "Invalid MaChinhSach (Guid).");

        var policyId = new PolicyId(id);

        if (!decimal.TryParse(dto.MucTroCapMacDinh, out var amountValue))
            return ImportResult<Policy>.Failure(ImportErrorCode.DATA_TYPE_MISMATCH, "Invalid MucTroCapMacDinh (decimal).");

        var moneyResult = Money.Create(amountValue, Currency.VND);
        if (!moneyResult.IsSuccess)
            return ImportResult<Policy>.Failure(ImportErrorCode.DOMAIN_RULE, moneyResult.Error?.Message ?? "Invalid Money.");

        var result = Policy.Create(
            policyId,
            dto.TenChinhSach ?? string.Empty,
            dto.GhiChu ?? string.Empty,
            moneyResult.Value
        );

        if (!result.IsSuccess)
            return ImportResult<Policy>.Failure(ImportErrorCode.DOMAIN_RULE, result.Error?.Message ?? "Policy validation failed.");

        return ImportResult<Policy>.Success(result.Value);
    }
}
