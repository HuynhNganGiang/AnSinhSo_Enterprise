using System;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Infrastructure.DataImport.Models;
using AnSinhSo.Infrastructure.DataImport.Normalization;

namespace AnSinhSo.Infrastructure.DataImport.Mappers;

public interface ICitizenMapper : IDataMapper<Stg_DoiTuongAnSinhRecord, Citizen> { }

public class CitizenMapper : ICitizenMapper
{
    public ImportResult<Citizen> Map(ImportContext context, Stg_DoiTuongAnSinhRecord dto)
    {
        if (!Guid.TryParse(dto.MaDoiTuong, out var id))
            return ImportResult<Citizen>.Failure(ImportErrorCode.DATA_TYPE_MISMATCH, "Invalid MaDoiTuong (Guid)");

        var citizenId = new CitizenId(id);

        var (firstName, middleName, lastName) = FullNameParser.Parse(dto.HoTen);
        var fullNameResult = FullName.Create(firstName, middleName, lastName);
        if (!fullNameResult.IsSuccess) 
            return ImportResult<Citizen>.Failure(ImportErrorCode.DOMAIN_RULE, fullNameResult.Error?.Message ?? "Invalid FullName");

        var citizenNumberResult = CitizenNumber.Create(dto.CCCD ?? string.Empty);
        if (!citizenNumberResult.IsSuccess) 
            return ImportResult<Citizen>.Failure(ImportErrorCode.INVALID_CCCD, citizenNumberResult.Error?.Message ?? "Invalid CCCD");

        if (!DateTime.TryParse(dto.NgaySinhISO, out var birthDate))
            return ImportResult<Citizen>.Failure(ImportErrorCode.INVALID_DATE, "Invalid NgaySinhISO");

        var gender = dto.GioiTinh?.Trim().ToLower() switch
        {
            "nam" => Gender.Male,
            "nữ" => Gender.Female,
            _ => Gender.Other
        };

        var phoneResult = PhoneNumber.Create(dto.SoDienThoai ?? string.Empty);
        if (!phoneResult.IsSuccess) 
            return ImportResult<Citizen>.Failure(ImportErrorCode.DOMAIN_RULE, phoneResult.Error?.Message ?? "Invalid PhoneNumber");

        // The real implementation needs postal code if the Domain requires it. If CSV doesn't provide, we must use Deferred or Failure per Architectural Decision #3 (No Dummy Data).
        // Since Address requires PostalCode, and CSV might lack it, we try to create it. If it fails, we fail the record.
        // Wait, if CSV doesn't have postal code, we just pass null/empty string and let Domain fail it.
        // But PostalCode.Create doesn't allow empty string. We pass string.Empty and it fails with Domain Rule.
        var postalCodeResult = PostalCode.Create(string.Empty);
        if (!postalCodeResult.IsSuccess)
            return ImportResult<Citizen>.Failure(ImportErrorCode.MISSING_REQUIRED_FIELD, postalCodeResult.Error?.Message ?? "PostalCode is missing but required by Domain");

        var addressResult = Address.Create(dto.DiaChi ?? string.Empty, string.Empty, string.Empty, string.Empty, postalCodeResult.Value);
        if (!addressResult.IsSuccess) 
            return ImportResult<Citizen>.Failure(ImportErrorCode.DOMAIN_RULE, addressResult.Error?.Message ?? "Invalid Address");

        var emailResult = Email.Create(string.Empty); // Passing empty to trigger failure if Domain requires it, no dummy data.
        if (!emailResult.IsSuccess)
            return ImportResult<Citizen>.Failure(ImportErrorCode.MISSING_REQUIRED_FIELD, emailResult.Error?.Message ?? "Email is missing but required by Domain");

        var result = Citizen.Create(
            citizenId,
            fullNameResult.Value,
            citizenNumberResult.Value,
            birthDate,
            gender,
            phoneResult.Value,
            addressResult.Value,
            emailResult.Value
        );

        if (!result.IsSuccess)
        {
            return ImportResult<Citizen>.Failure(ImportErrorCode.DOMAIN_RULE, result.Error?.Message ?? "Citizen validation failed");
        }

        return ImportResult<Citizen>.Success(result.Value);
    }
}
