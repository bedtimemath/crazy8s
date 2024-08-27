namespace C8S.Applications;

// These should *exactly* match the responses in the WPForms form on WordPress
public static class CoachAppConstants
{
    public static class OrganizationTypes
    {
        public const string School = "School";
        public const string Library = "Library";
        public const string HomeSchool = "Home School Co-Op";
        public const string BoysGirlsClub = "Boys & Girls Club";
        public const string YMCA = "YMCA";
        public const string Other = "Other";
    }

    public static class HasHostedBefore
    {
        public const string FirstTime = "This is our first time";
        public const string HostedBefore = "We've hosted before";
    }

    public static class IsSupervisor
    {
        public const string Coach = "I'm a coach";
        public const string Supervisor = "I'm a supervisor";
    }

    public static class HasWorkshopCode
    {
        public const string No = "No";
        public const string Yes = "Yes, I have a workshop code";
    }
}