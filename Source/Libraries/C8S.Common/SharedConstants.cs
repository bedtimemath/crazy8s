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
        public const string Activate = "<i class=\"fa-regular fa-wave-pulse\"></i>";
        public const string Actions = "<i class=\"fa-regular fa-clipboard-check\"></i>";
        public const string Add = "<i class=\"fa-regular fa-plus\"></i>";
        public const string Backward = "<i class=\"fa-regular fa-backward\"></i>";
        public const string Barcode = "<i class=\"fa-regular fa-barcode-read\"></i>";
        public const string Call = "<i class=\"fa-regular fa-phone-rotary\"></i>";
        public const string Camera = "<i class=\"fa-regular fa-camera\"></i>";
        public const string Cancel = "<i class=\"fa-regular fa-ban\"></i>";
        public const string CheckIn = "<i class=\"fa-regular fa-arrow-down-to-square\"></i>";
        public const string Check = "<i class=\"fa-regular fa-check\"></i>";
        public const string CheckOut = "<i class=\"fa-regular fa-cash-register\"></i>";
        public const string Clear = "<i class=\"fa-regular fa-broom-wide\"></i>";
        public const string Compare = "<i class=\"fa-regular fa-code-compare\"></i>";
        public const string Delete = "<i class=\"fa-regular fa-trash\"></i>";
        public const string Donation = "<i class=\"fa-regular fa-gift\"></i>";
        public const string Download = "<i class=\"fa-regular fa-download\"></i>";
        public const string Edit = "<i class=\"fa-regular fa-pencil\"></i>";
        public const string Family = "<i class=\"fa-regular fa-family\"></i>";
        public const string Forward = "<i class=\"fa-regular fa-forward\"></i>";
        public const string Flag = "<i class=\"fa-regular fa-flag\"></i>";
        public const string Game = "<i class=\"fa-kit fa-meeple-outline\"></i>";
        public const string Home = "<i class=\"fa-regular fa-house\"></i>";
        public const string Knock = "<i class=\"fa-regular fa-hand-fist\"></i>";
        public const string Label = "<i class=\"fa-regular fa-tag\"></i>";
        public const string Lego = "<i class=\"fa-regular fa-block-brick\"></i>";
        public const string Link = "<i class=\"fa-regular fa-link\"></i>";
        public const string Linked = "<i class=\"fa-regular fa-link\"></i>";
        public const string Loan = "<i class=\"fa-regular fa-hand-holding-box\"></i>";
        public const string Location = "<i class=\"fa-regular fa-location-dot\"></i>";
        public const string LogIn = "<i class=\"fa-regular fa-right-to-bracket\"></i>";
        public const string LogOut = "<i class=\"fa-regular fa-right-from-bracket\"></i>";
        public const string Menu = "<i class=\"fa-regular fa-ellipsis-vertical\"></i>";
        public const string Move = "<i class=\"fa-regular fa-arrow-right-to-line\"></i>";
        public const string Note = "<i class=\"fa-regular fa-note\"></i>";
        public const string Other = "<i class=\"fa-regular fa-square-question\"></i>";
        public const string Part = "<i class=\"fa-regular fa-puzzle\"></i>";
        public const string Person = "<i class=\"fa-regular fa-person\"></i>";
        public const string PostCard = "<i class=\"fa-regular fa-envelopes-bulk\"></i>";
        public const string Primary = "<i class=\"fa-regular fa-circle-1\"></i>";
        public const string Print = "<i class=\"fa-regular fa-print\"></i>";
        public const string Puzzle = "<i class=\"fa-regular fa-puzzle-piece\"></i>";
        public const string Profile = "<i class=\"fa-regular fa-user\"></i>";
        public const string Register = "<i class=\"fa-regular fa-user-plus\"></i>";
        public const string Reload = "<i class=\"fa-regular fa-arrow-rotate-right\"></i>";
        public const string Renew = "<i class=\"fa-regular fa-arrows-rotate\"></i>";
        public const string Repeat = "<i class=\"fa-regular fa-repeat\"></i>";
        public const string Save = "<i class=\"fa-regular fa-floppy-disk\"></i>";
        public const string Skip = "<i class=\"fa-regular fa-forward-step\"></i>";
        public const string SaveNew = "<i class=\"fa-regular fa-floppy-disk-circle-arrow-right\"></i>";
        public const string Search = "<i class=\"fa-regular fa-magnifying-glass\"></i>";
        public const string Select = "<i class=\"fa-regular fa-ballot-check\"></i>";
        public const string Submit = "<i class=\"fa-regular fa-arrow-turn-down-left\"></i>";
        public const string Statistics = "<i class=\"fa-regular fa-chart-line-up\"></i>";
        public const string Swap = "<i class=\"fa-regular fa-swap\"></i>";
        public const string Tags = "<i class=\"fa-regular fa-tags\"></i>";
        public const string Toy = "<i class=\"fa-regular fa-dreidel\"></i>";
        public const string Unlinked = "<i class=\"fa-regular fa-link-slash\"></i>";
        public const string Upload = "<i class=\"fa-regular fa-upload\"></i>";
        public const string Valuation = "<i class=\"fa-regular fa-money-bill\"></i>";
        public const string Verify = "<i class=\"fa-regular fa-badge-check\"></i>";
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