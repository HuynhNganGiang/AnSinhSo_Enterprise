using System;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Infrastructure.DataImport.Models;

namespace AnSinhSo.Infrastructure.DataImport.Mappers;

public interface IHouseholdMapper : IDataMapper<Stg_HoGiaDinhRecord, Household> { }

public class HouseholdMapper : IHouseholdMapper
{
    public ImportResult<Household> Map(ImportContext context, Stg_HoGiaDinhRecord dto)
    {
        if (!Guid.TryParse(dto.MaHo, out var id))
            return ImportResult<Household>.Failure(ImportErrorCode.DATA_TYPE_MISMATCH, "Invalid MaHo (Guid).");

        var postalCodeResult = PostalCode.Create(string.Empty);
        if (!postalCodeResult.IsSuccess)
            return ImportResult<Household>.Failure(ImportErrorCode.MISSING_REQUIRED_FIELD, postalCodeResult.Error?.Message ?? "PostalCode is missing but required by Domain");

        var addressResult = Address.Create(dto.DiaChi ?? string.Empty, string.Empty, string.Empty, string.Empty, postalCodeResult.Value);
        if (!addressResult.IsSuccess) 
            return ImportResult<Household>.Failure(ImportErrorCode.DOMAIN_RULE, addressResult.Error?.Message ?? "Invalid Address");

        var result = Household.Create(
            new HouseholdId(Guid.NewGuid()),
            new HouseholdCode(dto.MaHo ?? string.Empty),
            addressResult.Value
        );

        if (!result.IsSuccess)
            return ImportResult<Household>.Failure(ImportErrorCode.DOMAIN_RULE, result.Error?.Message ?? "Household validation failed.");

        return ImportResult<Household>.Success(result.Value);
    }
}
