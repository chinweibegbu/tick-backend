namespace TemplateTest.Domain.Enum
{
    public enum AccountType
    {
        Settlement = 1,
        Internal = 2,
    }

    public enum AccountStatus
    {
        Active = 1,
        Inactive = 2,
        Frozen = 3,
        Blocked = 4,
    }

    public enum ParticipantStatus
    {
        Active = 1,
        Inactive = 2,
    }

    public enum ParticipantType
    {
        LocalFi = 1,
        TemplateTestInternal = 2,
        SettlementPartner = 3,
    }

    public enum BasicAuthStatus
    {
        Active = 1,
        Inactive = 2,
    }

    public enum RequestType
    {
        New = 1,
        Edit = 2,
        Delete = 3
    }

    // Populate this enum with the list of events that you want to audit
    public enum AuditEventType
    {
        Account = 1,
        Participant = 2,
        BasicUser = 3,
        User = 4,
        LiquidityTransfer = 5
    }

    public enum UserRole
    {
        Administrator = 1,
        BankStaff = 2,
        Settlement = 3
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2
    }
}
