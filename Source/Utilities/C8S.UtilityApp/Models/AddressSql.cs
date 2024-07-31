using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.UtilityApp.Models;

public class AddressSql
{
    #region Constants & ReadOnlys
    public const string SqlGet = 
        "SELECT up.[Id] AS [OldSystemUsaPostalId], up.[RecipientName], up.[BusinessName], up.[StreetAddress], up.[City], us.[Abbreviation] AS [State], up.[PostalCode], us.[TimeZoneId] AS [TimeZone], us.[IsMilitary] FROM [Bits].[UsaPostal] up JOIN [Bits].[UsaState] us ON us.[Id] = up.[StateId] WHERE up.[DeletedBy] IS NOT NULL";
    #endregion

    #region Id Property
    public int? AddressId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemUsaPostalId { get; set; } = null;

    public string? RecipientName { get; set; } = null;

    public string? BusinessName { get; set; } = default!;

    public string? StreetAddress { get; set; } = default!;

    public string? City { get; set; } = default!;

    public string? State { get; set; } = default!;

    public string? PostalCode { get; set; } = default!;

    public string? TimeZone { get; set; } = default!;

    public bool IsMilitary { get; set; } = default!;
    #endregion
}