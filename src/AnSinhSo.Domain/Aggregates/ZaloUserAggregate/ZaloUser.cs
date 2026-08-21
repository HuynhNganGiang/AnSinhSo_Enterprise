using System;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.ZaloUserAggregate;

public class ZaloUser : AggregateRoot<Guid>
{
    public string ZaloId { get; private set; }
    public string Name { get; private set; }
    public string? Avatar { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Guid? CitizenId { get; private set; }
    public Guid? CitizenIdentityId { get; private set; }
    public bool IsFollowing { get; private set; }

#pragma warning disable CS8618
    protected ZaloUser() { }
#pragma warning restore CS8618

    private ZaloUser(Guid id, string zaloId, string name, string? avatar) : base(id)
    {
        ZaloId = zaloId;
        Name = name;
        Avatar = avatar;
        IsFollowing = true;
    }

    public static ZaloUser Create(string zaloId, string name, string? avatar)
    {
        return new ZaloUser(Guid.NewGuid(), zaloId, name, avatar);
    }

    public void UpdateProfile(string name, string? avatar)
    {
        Name = name;
        Avatar = avatar;
    }

    public void SetFollowingStatus(bool isFollowing)
    {
        IsFollowing = isFollowing;
    }

    public void LinkCitizen(Guid citizenId, Guid citizenIdentityId, string phoneNumber)
    {
        CitizenId = citizenId;
        CitizenIdentityId = citizenIdentityId;
        PhoneNumber = phoneNumber;
    }

    public void UnlinkCitizen()
    {
        CitizenId = null;
        CitizenIdentityId = null;
        PhoneNumber = null;
    }
}
