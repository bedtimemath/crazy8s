namespace C8S.Common.Interfaces
{
    /// <summary>
    /// An interface for any class that will manipulate the date & time. Particularly useful if you need
    /// to add a way to 'pretend' the date/time is something other than it really is (in unit testing or
    /// when integration testing)
    /// </summary>
    public interface IDateTimeHelper
    {
        /// <summary>Current date/time in local time zone.</summary>
        DateTimeOffset Now { get; }

        /// <summary>Current date/time in UTC.</summary>
        DateTimeOffset UtcNow { get; }

        /// <summary>Current date/time in Mountain Time.</summary>
        DateTimeOffset MountainNow { get; }

        /// <summary>Number of ticks (positive or negative) to offset the real date/time.</summary>
        Int32 OffsetSeconds { get; }

        /// <summary>Reset the date/time to the current, real date/time.</summary>
        void ClearChanges();

        /// <summary>Set the pretend "now" in UTC.</summary>
        void SetUtcNow(DateTimeOffset dateTime);

    }
}