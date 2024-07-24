namespace C8S.Common;

public static class C8SConstants
{
    public static class BlobContainers
    {
        public const string Images = "images";
        public const string Labels = "labels";
        public const string QRCodes = "qrcodes";
    }
    
    public static class Connections
    {
        public const string AzureStorage = "AzureStorage";
        public const string AppConfig = "AppConfig";
        public const string Database = "Database";
        public const string OldSystem = "OldSystem";
    }

    public static class Defaults
    {
        public const string DefaultAdminEmails = "development@bedtimemath.org";
    }
}