using System.Net.Http.Headers;
using C8S.Database.Abstractions.Base;
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

        var entries = eventData.Context.ChangeTracker.Entries<IAuditable>();

            logger.LogInformation("ASYNC: {@Context}", eventData.Context.ChangeTracker.Entries().Select(e => e.State).ToList());
        
        return ValueTask.FromResult(result);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context == null) return result;

            logger.LogInformation("SYNC: {@Context}", eventData.Context.ChangeTracker.Entries().Select(e => e.State).ToList());
        
        return result;
    }
}