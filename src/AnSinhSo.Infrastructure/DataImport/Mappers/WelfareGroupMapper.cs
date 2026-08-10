using System;
using AnSinhSo.Domain.Aggregates.WelfareGroupAggregate;
using AnSinhSo.Infrastructure.DataImport.Models;

namespace AnSinhSo.Infrastructure.DataImport.Mappers;

public interface IWelfareGroupMapper : IDataMapper<Stg_NhomDoiTuongRecord, WelfareGroup> { }

public class WelfareGroupMapper : IWelfareGroupMapper
{
    public ImportResult<WelfareGroup> Map(ImportContext context, Stg_NhomDoiTuongRecord dto)
    {
        if (!Guid.TryParse(dto.MaNhom, out var id))
            return ImportResult<WelfareGroup>.Failure(ImportErrorCode.DATA_TYPE_MISMATCH, "Invalid or missing MaNhom (Guid).");

        var welfareGroupId = new WelfareGroupId(id);

        if (string.IsNullOrWhiteSpace(dto.TenNhomDoiTuong))
            return ImportResult<WelfareGroup>.Failure(ImportErrorCode.MISSING_REQUIRED_FIELD, "TenNhomDoiTuong is missing.");

        var result = WelfareGroup.Create(
            welfareGroupId,
            dto.TenNhomDoiTuong,
            dto.GhiChu
        );

        if (!result.IsSuccess)
            return ImportResult<WelfareGroup>.Failure(ImportErrorCode.DOMAIN_RULE, result.Error?.Message ?? "Domain validation failed.");

        return ImportResult<WelfareGroup>.Success(result.Value);
    }
}
