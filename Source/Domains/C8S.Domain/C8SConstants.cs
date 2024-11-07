namespace C8S.Domain;

public static class C8SConstants
{
    public static class BlobContainers
    {
        public const string Images = "images";
        public const string Labels = "labels";
        public const string QRCodes = "qrcodes";
        public const string ApplicationsUnread = "c8s-applications";
        public const string ApplicationsProcessed = "c8s-applications-processed";
        public const string ApplicationsError = "c8s-applications-error";
    }

    public static class Defaults
    {
        public const string DefaultAdminEmails = "development@bedtimemath.org";
    }

    public static class Icons
    {
        public const string Application = "clipboard-list";
        public const string Refresh = "arrow-rotate-right";
    }
}