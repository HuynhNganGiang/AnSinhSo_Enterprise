using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.Notifications.Commands.CreateNotification;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.NotificationAggregate;
using MediatR;


namespace AnSinhSo.Application.Notifications.Commands.BroadcastNotification;

internal sealed class BroadcastNotificationCommandHandler : IRequestHandler<BroadcastNotificationCommand, int>
{
    private readonly ISender _sender;
    private readonly ICitizenRepository _citizenRepository;
    private readonly IHouseholdRepository _householdRepository;

    public BroadcastNotificationCommandHandler(
        ISender sender,
        ICitizenRepository citizenRepository,
        IHouseholdRepository householdRepository)
    {
        _sender = sender;
        _citizenRepository = citizenRepository;
        _householdRepository = householdRepository;
    }

    public async Task<int> Handle(BroadcastNotificationCommand request, CancellationToken cancellationToken)
    {
        var targetCitizenIds = new System.Collections.Generic.HashSet<Guid>();

        switch (request.Filter)
        {
            case RecipientFilter.AllCitizens:
                var allCitizensResult = await _citizenRepository.GetPagedAsync(1, 10000, null, cancellationToken);
                foreach (var c in allCitizensResult.Items) targetCitizenIds.Add(c.Id.Value);
                break;
            case RecipientFilter.PoorHouseholds:
                // To fetch citizens in poor households
                var poorHouseholdsResult = await _householdRepository.GetPagedAsync(1, 10000, null, cancellationToken);
                // Assume poor logic: In a real system we'd join with policies, but for now mock with members
                foreach (var h in poorHouseholdsResult.Items)
                {
                    if (h.HouseholdCode.Value.EndsWith("1") || h.HouseholdCode.Value.EndsWith("2"))
                    {
                        foreach (var member in h.Members) targetCitizenIds.Add(member.CitizenId.Value);
                    }
                }
                break;
            case RecipientFilter.NearPoorHouseholds:
                var nearPoorHouseholdsResult = await _householdRepository.GetPagedAsync(1, 10000, null, cancellationToken);
                foreach (var h in nearPoorHouseholdsResult.Items)
                {
                    if (h.HouseholdCode.Value.EndsWith("3") || h.HouseholdCode.Value.EndsWith("4"))
                    {
                        foreach (var member in h.Members) targetCitizenIds.Add(member.CitizenId.Value);
                    }
                }
                break;
            case RecipientFilter.ReceivingBenefits:
                // Assuming receiving benefits means they are in a household with policy
                // We'll mock it
                break;
            case RecipientFilter.SpecificList:
                if (request.SpecificCitizenIds != null)
                {
                    foreach (var id in request.SpecificCitizenIds) targetCitizenIds.Add(id);
                }
                break;
        }

        int count = 0;
        foreach (var citizenId in targetCitizenIds)
        {
            await _sender.Send(new CreateNotificationCommand(
                request.Title,
                request.Content,
                citizenId,
                request.Channel,
                request.Priority,
                null,
                request.SourceModule,
                request.SourceId
            ), cancellationToken);
            count++;
        }

        return count;
    }
}
