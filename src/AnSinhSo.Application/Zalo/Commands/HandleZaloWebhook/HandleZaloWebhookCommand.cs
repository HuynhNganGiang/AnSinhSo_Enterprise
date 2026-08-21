using System.Text.Json;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.Zalo.Commands.HandleZaloWebhook;

public sealed record HandleZaloWebhookCommand(
    string EventName,
    string ZaloUserId,
    JsonElement Payload
) : IRequest<Result>;
