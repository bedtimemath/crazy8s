using C8S.Database.Abstractions.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.UtilityApp.Models;

public class OrderSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT o.[Id] AS [OldSystemOrderId], o.[ShippingAddressId] AS [OldSystemShippingAddressId], o.[ClubId] AS [OldSystemClubId], o.[Number], os.[Name] AS [StatusString], o.[ContactEmail], o.[ContactPhone], o.[ContactPhoneExt], CAST(o.[Ordered] AS NVARCHAR(255)) AS [OrderedOnString], CAST(o.[ArriveBy] AS NVARCHAR(255)) AS [ArriveByString],  CAST(o.[Shipped] AS NVARCHAR(255)) AS [ShippedOnString], CAST(o.[Emailed] AS NVARCHAR(255)) AS [EmailedOnString], o.[BatchId] AS [BatchIdentifier], o.[Notes], CAST(o.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Order] o  LEFT JOIN [Crazy8s].[OrderStatus] os ON os.[Id] = o.[StatusId] WHERE o.[DeletedBy] IS NULL";
    #endregion

    #region Id Property
    public int? OrderId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemOrderId { get; set; } = null;
    
    public Guid? OldSystemShippingAddressId { get; set; } = null;

    public Guid? OldSystemClubId { get; set; } = null;

    public int Number { get; set; } = default!;

    [NotMapped]
    public string? StatusString { get; set; } = null;

    public string? ContactEmail { get; set; } = null;

    public string? ContactPhone { get; set; } = null;

    public string? ContactPhoneExt { get; set; } = null;
    
    [NotMapped]
    public string OrderedOnString { get; set; } = default!;
    
    [NotMapped]
    public string ArriveByString { get; set; } = default!;
    
    [NotMapped]
    public string? ShippedOnString { get; set; } = default!;
    
    [NotMapped]
    public string? EmailedOnString { get; set; } = default!;
    
    public Guid? BatchIdentifier { get; set; } = default!;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties
    public OrderStatus? Status => StatusString switch
    {
        "Ordered" => OrderStatus.Ordered,
        "Processing" => OrderStatus.Processing,
        "Shipped" => OrderStatus.Shipped,
        "Canceled" => OrderStatus.Canceled,
        "Returned" => OrderStatus.Returned,
        null => null,
        _ => throw new Exception($"Unrecognized: {StatusString}")
    };

    public DateTimeOffset OrderedOn => 
        !DateTimeOffset.TryParse(OrderedOnString, out var orderedOn) ? DateTimeOffset.MinValue : orderedOn;

    public DateOnly ArriveBy => 
        !DateOnly.TryParse(ArriveByString, out var arriveBy) ? DateOnly.MinValue : arriveBy;

    public DateTimeOffset? ShippedOn => (String.IsNullOrEmpty(ShippedOnString)) ? null :
        (!DateTimeOffset.TryParse(ShippedOnString, out var shippedOn) ? DateTimeOffset.MinValue : shippedOn);

    public DateTimeOffset? EmailedOn => (String.IsNullOrEmpty(EmailedOnString)) ? null :
        (!DateTimeOffset.TryParse(EmailedOnString, out var emailedOn) ? DateTimeOffset.MinValue : emailedOn);

    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}