using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Households.Commands.CreateHousehold;

/// <summary>
/// Command dùng để khởi tạo một hộ gia đình mới.
/// </summary>
/// <param name="HouseholdCode">Mã hộ gia đình.</param>
/// <param name="AddressId">Định danh của địa chỉ hộ gia đình.</param>
/// <param name="HeadCitizenId">Định danh của công dân làm chủ hộ.</param>
/// <param name="Name">Tên hộ gia đình (thường là họ tên chủ hộ).</param>
/// <param name="Description">Mô tả thêm về hộ gia đình.</param>
public sealed record CreateHouseholdCommand(
    string HouseholdCode,
    Guid AddressId,
    Guid HeadCitizenId,
    string Name,
    string Description) : IRequest<Result<Guid>>;
