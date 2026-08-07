using System;
using MediatR;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Application.Citizens.Commands.RemoveCitizen;

public sealed record RemoveCitizenCommand(Guid CitizenId) : IRequest<Result>;
