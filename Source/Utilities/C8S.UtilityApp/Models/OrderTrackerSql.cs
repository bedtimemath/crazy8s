using System.ComponentModel.DataAnnotations.Schema;
using C8S.Domain.Enums;

namespace C8S.UtilityApp.Models;

public class OrderTrackerSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT ot.[Id] AS [OldSystemOrderTrackerId], ot.[OrderId] AS [OldSystemOrderId], ot.[Code], tt.[Name] AS [TypeString], CAST(ot.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[OrderTracker] ot LEFT JOIN [Crazy8s].[TrackerType] tt ON tt.[Id] = ot.[TypeId] WHERE ot.[DeletedBy] IS NULL";
    #endregion

    #region Id Property
    public int? OrderTrackerId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemOrderTrackerId { get; set; } = null;

    public Guid? OldSystemOrderId { get; set; } = null;
    
    public string Code { get; set; } = default!;

    [NotMapped]
    public string? TypeString { get; set; } = null;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties
    public ShipMethod? Method => TypeString switch
    {
        "FedEx" => ShipMethod.FedEx,
        "UPS" => ShipMethod.UPS,
        "USPS" => ShipMethod.USPS,
        null => null,
        _ => throw new Exception($"Unrecognized: {TypeString}")
    };

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}