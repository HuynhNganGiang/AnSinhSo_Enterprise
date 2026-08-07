using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.CreateCitizen;

/// <summary>
/// Lệnh tạo mới công dân.
/// </summary>
/// <param name="CitizenNumber">Số Căn cước công dân.</param>
/// <param name="FullName">Họ và tên công dân.</param>
/// <param name="BirthDate">Ngày tháng năm sinh.</param>
/// <param name="Gender">Giới tính.</param>
/// <param name="PhoneNumber">Số điện thoại liên hệ.</param>
/// <param name="Address">Địa chỉ thường trú.</param>
/// <param name="Email">Địa chỉ Email liên hệ.</param>
public sealed record CreateCitizenCommand(
    string CitizenNumber,
    string FullName,
    DateTime BirthDate,
    int Gender,
    string PhoneNumber,
    string Address,
    string Email) : IRequest<Result<Guid>>;
