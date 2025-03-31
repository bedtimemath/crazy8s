using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Enums;
using ALevel = C8S.Domain.Enums.AgeLevel;

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

    public string Key { get; set; } = null!;

    public string Name { get; set; } = null!;

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

    public OfferStatus? Status => StatusString switch
    {
        "Draft" => OfferStatus.Draft,
        "Active" => OfferStatus.Active,
        "Inactive" => OfferStatus.Inactive,
        _ => throw new Exception($"Unrecognized: {StatusString}")
    };

    public AgeLevel? AgeLevel => AgeLevelString switch
    {
        "K - 2nd Grade" => ALevel.GradesK2,
        "3rd - 5th Grade" => ALevel.Grades35,
        "Unknown" or null => null,
        _ => throw new Exception($"Unrecognized: {AgeLevelString}")
    };

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}