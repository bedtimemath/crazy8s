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

    public static class Entities
    {
        public const string Note = "Note";
    }

    public static class Icons
    {
        public const string Add = "plus";
        public const string Clear = "broom-wide";
        public const string Close = "circle-xmark";
        public const string Coach = "user-hair-long";
        public const string Link = "link";
        public const string Order = "cart-shopping";
        public const string Organization = "school";
        public const string Refresh = "arrow-rotate-right";
        public const string Request = "clipboard-list";
        public const string Sku = "barcode";
        public const string Settings = "gear";
        public const string Unlink = "link-slash";
    }

    public static class LogTables
    {
        public const string FunctionsLog = "FunctionsLog";
        public const string AdminLog = "AdminLog";
    }
}