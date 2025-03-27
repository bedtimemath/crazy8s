using System.ComponentModel.DataAnnotations.Schema;
using ALevel = C8S.Domain.Enums.AgeLevel;

namespace C8S.UtilityApp.Models;

public class ApplicationClubSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT ac.[Id] AS [OldSystemApplicationClubId], ac.[ApplicationId] AS [OldSystemApplicationId], ac.[LinkedClubId] AS [OldSystemLinkedClubId], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], s.[Ordinal] AS [Season], CAST(ac.[Starts] AS DATE) AS [StartsOnDateTime], CAST(ac.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[ApplicationClub] ac  LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = ac.[AgeLevelId] LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = ac.[ClubSizeId] LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = ac.[SeasonId] WHERE ac.[DeletedBy] IS NULL";
    #endregion

    #region Id Property
    public int? ApplicationClubId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemApplicationClubId { get; set; } = null;

    public Guid? OldSystemApplicationId { get; set; } = null;

    public Guid? OldLinkedClubId { get; set; } = null;

    [NotMapped]
    public string? AgeLevelString { get; set; } = null;

    [NotMapped]
    public string? ClubSizeString { get; set; } = null;

    public int? Season { get; set; } = null;

    [NotMapped]
    public DateTime? StartsOnDateTime { get; set; } = null;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties
    public ALevel? AgeLevel => AgeLevelString switch
    {
        "3rd - 5th Grade" => ALevel.GradesK2,
        "K - 2nd Grade" => ALevel.Grades35,
        "Unknown" or null => null,
        _ => throw new Exception($"Unrecognized: {AgeLevelString}")
    };

    public DateOnly? StartsOn =>
        StartsOnDateTime.HasValue ? DateOnly.FromDateTime(StartsOnDateTime.Value) : null;

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}