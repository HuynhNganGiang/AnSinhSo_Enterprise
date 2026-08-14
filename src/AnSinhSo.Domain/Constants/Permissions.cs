namespace AnSinhSo.Domain.Constants;

public static class Permissions
{
    public static class Roles
    {
        public const string View = "roles.view";
        public const string Create = "roles.create";
        public const string Update = "roles.update";
        public const string Delete = "roles.delete";
    }

    public static class PermissionsModule
    {
        public const string View = "permissions.view";
        public const string Manage = "permissions.manage"; // Cho role-permissions
    }

    public static class UserRoles
    {
        public const string View = "userroles.view";
        public const string Manage = "userroles.manage";
    }

    public static class Citizens
    {
        public const string View = "citizens.view";
        public const string Create = "citizens.create";
        public const string Update = "citizens.update";
        public const string Activate = "citizens.activate";
        public const string Deactivate = "citizens.deactivate";
        public const string Delete = "citizens.delete";
    }

    public static class Households
    {
        public const string Read = "households.read";
        public const string Create = "households.create";
        public const string Update = "households.update";
        public const string Delete = "households.delete";
    }

    public static class Payments
    {
        public const string View = "payments.view";
        public const string Create = "payments.create";
        public const string Approve = "payments.approve";
        public const string Complete = "payments.complete";
        public const string Cancel = "payments.cancel";
    }

    public static class GIS
    {
        public const string Read = "gis.read";
        public const string Create = "gis.create";
        public const string Update = "gis.update";
        public const string Delete = "gis.delete";
    }

    public static class Notifications
    {
        public const string Read = "notifications.read";
        public const string Create = "notifications.create";
        public const string Update = "notifications.update";
        public const string Delete = "notifications.delete";
    }

    public static class AI
    {
        public const string Read = "ai.read";
        public const string Create = "ai.create";
        public const string Update = "ai.update";
        public const string Delete = "ai.delete";
    }

    public static class WelfarePrograms
    {
        public const string View = "welfareprograms.view";
        public const string Create = "welfareprograms.create";
        public const string Update = "welfareprograms.update";
    }

    public static class WelfareCases
    {
        public const string View = "welfarecases.view";
        public const string Create = "welfarecases.create";
        public const string Update = "welfarecases.update";
        public const string Decide = "welfarecases.decide";
        public const string Cancel = "welfarecases.cancel";
        public const string Close = "welfarecases.close";
    }
}
