using System.ComponentModel.DataAnnotations.Schema;
using ALevel = C8S.Domain.Enums.AgeLevel;

namespace C8S.UtilityApp.Models;

public class OrderClubSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT o.[Id] AS [OldSystemOrderId], c.[Id] AS [OldSystemClubId], c.[OrganizationId] AS [OldSystemOrganizationId], c.[CoachId] AS [OldSystemCoachId], c.[MeetingAddressId] AS [OldSystemMeetingAddressId], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], s.[Ordinal] AS [Season], CAST(c.[Starts] AS DATE) AS [StartsOnDateTime], c.[Notes], CAST(c.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Club] c LEFT JOIN [Crazy8s].[Order] o ON o.[ClubId] = c.[Id] LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = c.[AgeLevelId] LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = c.[ClubSizeId] LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = c.[SeasonId] LEFT JOIN [Bits].[UsaPostal] up ON up.[Id] = c.[MeetingAddressId] WHERE o.[DeletedBy] IS NULL AND c.[DeletedBy] IS NULL";
    #endregion

    #region Public Properties    
    public Guid? OldSystemOrderId { get; set; } = null;

    public Guid? OldSystemClubId { get; set; } = null;

    public Guid? OldSystemOrganizationId { get; set; } = null;

    public Guid? OldSystemCoachId { get; set; } = null;

    public Guid? OldSystemMeetingAddressId { get; set; } = null;

    [NotMapped]
    public string? AgeLevelString { get; set; } = null;

    [NotMapped]
    public string? ClubSizeString { get; set; } = null;

    public int? Season { get; set; } = null;

    [NotMapped]
    public DateTime? StartsOnDateTime { get; set; } = null;

    public string? Notes { get; set; } = null;

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