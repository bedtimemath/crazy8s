using C8S.Database.Abstractions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SC.Common.Helpers.PassThrus;
using SC.Common.Interfaces;

namespace C8S.Database.EFCore.Interceptors;

internal sealed class CreateModifyInterceptor(
    ILogger<CreateModifyInterceptor> logger,
    IDateTimeHelper? dateTimeHelper): SaveChangesInterceptor
{
    private readonly IDateTimeHelper _dateTimeHelper = 
        dateTimeHelper ?? new PassthruDateTimeHelper();

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null) return ValueTask.FromResult(result);
        SetEntryValues(eventData.Context, _dateTimeHelper);

        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;
        SetEntryValues(eventData.Context, _dateTimeHelper);

        return result;
    }

    private static void SetEntryValues(DbContext context, 
        IDateTimeHelper dateTimeHelper)
    {
        var entries = context.ChangeTracker.Entries<IAuditable>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(entry, 
                    nameof(IAuditable.CreatedOn), dateTimeHelper.UtcNow);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(entry, 
                    nameof(IAuditable.ModifiedOn), dateTimeHelper.UtcNow);
            }
        }
    }
    
    private static void SetCurrentPropertyValue(
        EntityEntry entry,
        string propertyName,
        DateTimeOffset utcNow) =>
        entry.Property(propertyName).CurrentValue = utcNow;
}