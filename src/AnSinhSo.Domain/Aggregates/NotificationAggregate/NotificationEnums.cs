namespace AnSinhSo.Domain.Aggregates.NotificationAggregate;

public enum NotificationChannel
{
    InApp,
    ZaloOA
}

public enum NotificationStatus
{
    Pending,
    Processing,
    Sent,
    Failed,
    Read
}

public enum NotificationPriority
{
    Low,
    Normal,
    High,
    Urgent
}

public enum SourceModule
{
    Citizen,
    Household,
    Welfare,
    Payment,
    AI,
    System
}

public enum RecipientFilter
{
    AllCitizens,
    PoorHouseholds,
    NearPoorHouseholds,
    ReceivingBenefits,
    SpecificList
}
