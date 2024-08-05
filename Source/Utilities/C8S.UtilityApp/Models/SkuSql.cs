using C8S.Database.Abstractions.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;
using ALevel = C8S.Database.Abstractions.Enumerations.AgeLevel;
using CSize = C8S.Database.Abstractions.Enumerations.ClubSize;

namespace C8S.UtilityApp.Models;

public class SkuSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT k.[Id] AS [OldSystemSkuId], k.[Key], k.[Name], ss.[Name] AS [StatusString], s.[Ordinal] AS [Season], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], k.[PackingListNotes], CAST(k.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Sku] k  LEFT JOIN [Crazy8s].[SkuStatus] ss ON ss.[Id] = k.[StatusId] LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = k.[AgeLevelId] LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = k.[ClubSizeId] LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = k.[SeasonId] WHERE k.[DeletedBy] IS NULL";
    #endregion

    #region Id Property
    public int? SkuId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemSkuId { get; set; } = null;

    public string Key { get; set; } = default!;

    public string Name { get; set; } = default!;

    [NotMapped]
    public string? StatusString { get; set; } = null;

    public int? Season { get; set; } = null;

    [NotMapped]
    public string? AgeLevelString { get; set; } = null;

    [NotMapped]
    public string? ClubSizeString { get; set; } = null;

    public string? Notes { get; set; } = null;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties

    public SkuStatus? Status => StatusString switch
    {
        "Draft" => SkuStatus.Draft,
        "Active" => SkuStatus.Active,
        "Inactive" => SkuStatus.Inactive,
        _ => throw new Exception($"Unrecognized: {StatusString}")
    };

    public AgeLevel? AgeLevel => AgeLevelString switch
    {
        "3rd - 5th Grade" => ALevel.GradesK2,
        "K - 2nd Grade" => ALevel.Grades35,
        "Unknown" or null => null,
        _ => throw new Exception($"Unrecognized: {AgeLevelString}")
    };

    public ClubSize? ClubSize => ClubSizeString switch
    {
        "Extra A4" => CSize.ExtraA4,
        "Extra A4a" => CSize.ExtraA4a,
        "Extra A4b" => CSize.ExtraA4b,
        "Extra A8" => CSize.ExtraA8,
        "Size 12" => CSize.Size12,
        "Size 16" => CSize.Size16,
        "Size 20" => CSize.Size20,
        "Unknown" or null => null,
        _ => throw new Exception($"Unrecognized: {ClubSizeString}")
    };

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}