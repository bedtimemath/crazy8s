using System.Diagnostics.CodeAnalysis;
using C8S.Common.Interfaces;

namespace C8S.Common.Helpers.PassThrus;

[ExcludeFromCodeCoverage]
public class PassthruDateTimeHelper : IDateTimeHelper
{
    #region Public Properties
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTimeOffset MountainNow => 
        TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.Now, "Mountain Standard Time");
    public Int32 OffsetSeconds => 0;
    #endregion

    #region IDateTimeHelper Implementation
    public void ClearChanges()
    {
        throw new NotImplementedException();
    }

    public void SetNow(DateTimeOffset dateTime)
    {
        throw new NotImplementedException();
    }

    public void SetUtcNow(DateTimeOffset dateTime)
    {
        throw new NotImplementedException();
    }
    #endregion
}