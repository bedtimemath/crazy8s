using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SC.Common.Base;
using SC.Common.Helpers.PassThrus;
using SC.Common.Interfaces;

namespace C8S.Domain.EFCore.Interceptors;

internal sealed class CreateModifyInterceptor(
    //ILogger<CreateModifyInterceptor> logger,
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
        var entries = context.ChangeTracker.Entries<ICoreDb>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                // we may set the created on value when migrating data; in which case, don't set it here
                if (entry.CurrentValues.TryGetValue(nameof(BaseCoreDb.CreatedOn), out DateTimeOffset createdOn))
                    if (createdOn != DateTimeOffset.MinValue) continue;
                SetCurrentPropertyValue(entry, 
                    nameof(ICoreDb.CreatedOn), dateTimeHelper.UtcNow);
            }
            else if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(entry, 
                    nameof(ICoreDb.ModifiedOn), dateTimeHelper.UtcNow);
            }
        }
    }
    
    private static void SetCurrentPropertyValue(
        EntityEntry entry,
        string propertyName,
        DateTimeOffset utcNow) =>
        entry.Property(propertyName).CurrentValue = utcNow;
}