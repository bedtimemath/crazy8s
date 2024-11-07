using System.Text.Json;
using System.Text.Json.Serialization;
using C8S.Database.Abstractions.Base;
using C8S.Database.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SC.Audit.EFCore.Contexts;
using SC.Audit.EFCore.Models;
using SC.Common.Models;

namespace C8S.Database.EFCore.Interceptors;

internal sealed class AuditInterceptor(
    ILogger<AuditInterceptor> logger,
    IDbContextFactory<SCAuditContext> dbContextFactory) : SaveChangesInterceptor
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private readonly List<DataChangeDb> _auditEntries = new();

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null) return result;

        try
        {
            await using var auditContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            foreach (var entry in eventData.Context.ChangeTracker.Entries()
                         .Where(e => e.State != EntityState.Detached &&
                                     e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        var deletedProps = entry.Properties
                            .Select(p => p.ToChangeData()).ToList();
                        // deleted entities don't come through on save, so we'll note them now
                        auditContext.DataChanges.Add(DataChangeFromEntry(entry, deletedProps));
                        break;

                    case EntityState.Modified:
                        var modifiedProps = entry.Properties
                            .Where(p => p.IsModified)
                            .Select(p => p.ToChangeData(includeOriginal: true)).ToList();
                        _auditEntries.Add(DataChangeFromEntry(entry, modifiedProps));
                        break;

                    case EntityState.Added:
                        var addedProps = entry.Properties
                            .Select(p => p.ToChangeData()).ToList();
                        _auditEntries.Add(DataChangeFromEntry(entry, addedProps));
                        break;

                    case EntityState.Detached:
                    case EntityState.Unchanged:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            await auditContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // failing here could cause the whole chain to fail
            logger.LogError(ex, "Could not save audit information (async)");
        }

        return result;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result) =>
        throw new NotImplementedException(); // always use async

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null) return result;

        try
        {
            await using var auditContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                var uniqueId = (entry.Entity as IBaseDb)?.UniqueId;
                var dataChange = _auditEntries.FirstOrDefault(a => a.Identifier == uniqueId);
                if (dataChange == null) continue;

                // if it's been added, we have to update the id & properties
                if (dataChange.EntityState == EntityState.Added)
                {
                    dataChange.EntityId = (entry.Entity as IBaseDb)?.Id ?? 0;
                    dataChange.PropertiesJson = SerializeProperties(
                        entry.Properties
                            .Select(p => new PropertyChangeData()
                            {
                                Name = p.Metadata.Name,
                                Value = p.CurrentValue
                            }
                        ).ToList());
                }

                await auditContext.AddAsync(dataChange, cancellationToken);
            }
            await auditContext.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            // failing here could cause the whole chain to fail
            logger.LogError(ex, "Could not save audit information (async)");
        }

        return result;
    }

    public override int SavedChanges(
        SaveChangesCompletedEventData eventData,
        int result) =>
        throw new NotImplementedException(); // always use async


    private static DataChangeDb DataChangeFromEntry(EntityEntry entry, List<PropertyChangeData> properties)
    {
        var description = $"{entry.State} {(entry.Entity as IBaseDb)?.Display}";
        if (entry.State == EntityState.Modified)
            description += $" ({(String.Join(",", properties.Select(p => p.Name)))})";
        var dataChange = new DataChangeDb()
        {
            Identifier = (entry.Entity as IBaseDb)?.UniqueId ?? Guid.Empty,
            EntityId = (entry.Entity as IBaseDb)?.Id ?? 0,
            EntityName = entry.Metadata.ClrType.Name,
            EntityState = entry.State,
            PropertiesJson = SerializeProperties(properties),
            Description = description,
            CreatedOn = DateTimeOffset.UtcNow
        };
        return dataChange;
    }

    private static string SerializeProperties(List<PropertyChangeData> properties) =>
        JsonSerializer.Serialize(properties, SerializerOptions);
}