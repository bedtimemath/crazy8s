namespace C8S.Common;


public static class SharedConstants
{
    public static class Claims
    {
        public const string EmailAddress = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        public const string Role = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public const string Name = "name";
        public const string Username = "username";
        public const string EmailVerified = "email_verified";
        public const string Picture = "picture";
        public const string SecurityIdentifier = "sid";
        public const string UpdatedAt = "updated_at";
    }

    public static class Display
    {
        public const string NotSet = "(not set)";
        public const string None = "(none)";
        public const string Anonymous = "(anonymous)";
        public const string Any = "(any)";
        public const string Adult = "(adult)";
        public const string System = "(system)";
        public const string You = "(you)";
        public const string Primary = "(primary)";
    }

    public static class HttpHeaders
    {
        public const string FunctionKey = "x-function-key";
    }

    public static class Icons
    {
        public const string Application = "money-check-pen";
        public const string Coach = "chalkboard-user";
        public const string Empty = "empty-set";
        public const string School = "school";
    }

    public static class KeyboardKeys
    {
        public const string Enter = "Enter";
    }

    public static class MaxLengths
    {
        public const int Tiny = 5;
        public const int Abbreviation = Tiny;

        public const int Short = 25;
        public const int Phone = Short;
        public const int ZIPCode = Short;

        public const int Medium = 50;
        public const int Enumeration = Medium;
        public const int Key = Medium;

        public const int Standard = 255;
        public const int Email = Standard;
        public const int FileName = Standard;
        public const int Name = Standard;
        public const int Password = Standard;
        public const int Title = Standard;
        public const int UserName = Standard;
        public const int Identifier = Standard;

        public const int Long = 512;
        public const int FullName = Long;
        public const int Url = Long;

        public const int XLong = 1024;

        public const int XXLong = 2048;

        public const int XXXLong = 4096;
    }

    public static class MimeTypes
    {
        public const string ApplicationJson = "application/json";
        public const string ApplicationPDF = "application/pdf";
        public const string ImageJpeg = "image/jpeg";
    }

    public static class Templates
    {
        public const string DefaultConsoleLog =
            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";
    }

    public static class UrlRoots
    {
        public const string Identity = "id";
        public const string External = "external";
        public const string SiteInfo = "siteinfo";
        public const string Logging = "logging";
    }

    public static class UrlActions
    {
        public const string Add = "add";
        public const string Read = "read";
        public const string LogIn = "login";
        public const string Reset = "reset";
        public const string LogOut = "logout";
        public const string Self = "self";
        public const string Staff = "staff";
        public const string Callback = "callback";
        public const string Challenge = "challenge";
        public const string Confirm = "confirm";
        public const string Change = "change";
        public const string Register = "register";
    }
}