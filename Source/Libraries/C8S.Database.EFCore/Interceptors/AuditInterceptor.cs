using C8S.Database.Abstractions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace C8S.Database.EFCore.Interceptors;

internal sealed class AuditInterceptor(
    ILogger<AuditInterceptor> logger): SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null) return ValueTask.FromResult(result);
        LogChanges(eventData.Context);

        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;
        LogChanges(eventData.Context);

        return result;
    }
    

    private void LogChanges(DbContext dbContext)
    {
        var entries = dbContext.ChangeTracker.Entries();
        foreach (var entry in entries)
        {
            var state = entry.State;
            var changesList = entry.Properties
                .Where(p => p.IsModified)
                .Select(p => $"{p.Metadata.Name}: {p.OriginalValue} => {p.CurrentValue}")
                .ToList();
            logger.LogInformation("[{State}] {Joined}", state, String.Join("; ", changesList));
        }
    }
}