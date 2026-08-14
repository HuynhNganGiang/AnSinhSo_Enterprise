using System;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Authorization.Queries.GetCurrentUserProfile;

public sealed record GetCurrentUserProfileQuery() : IRequest<Result<CurrentUserProfileDto>>;

public record CurrentUserProfileDto(Guid Id, string FullName, string IdentityNumber, string PhoneNumber);
