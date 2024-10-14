namespace C8S.FullSlate.Abstractions;

public static class FullSlateConstants
{
    public static class Endpoints
    {
        public const string Clients = "clients";
        public const string Appointments = "appointments";
        public const string Openings = "openings";
    }
    public static class ErrorCodes
    {
        public const string StatusBooked = "STATUS_BOOKED";
        public const string NoOpening = "NO_OPENING";
    }
    public static class ErrorMessages
    {
        public const string PleaseSendRequestId = " Please send the requestId to the support team.";
    }
    public static class Offerings
    {
        public const int CoachCall = 1;
    }
    public static class UserTypes
    {
        public const string Client = "CLIENT";
    }
}