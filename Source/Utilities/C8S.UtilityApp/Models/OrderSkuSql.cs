﻿using C8S.Database.Abstractions.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace C8S.UtilityApp.Models;

public class OrderSkuSql
{
    #region Constants & ReadOnlys
    public const string SqlGet =
        "SELECT os.[Id] AS [OldSystemOrderSkuId], os.[OrderId] AS [OldSystemOrderId], os.[SkuId] AS [OldSystemSkuId], os.[Ordinal], os.[Quantity], CAST(os.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[OrderSku] os WHERE os.[DeletedBy] IS NULL";
    #endregion

    #region Id Property
    public int? OrderSkuId { get; set; }
    #endregion

    #region Public Properties    
    public Guid? OldSystemOrderSkuId { get; set; } = null;
    
    public Guid? OldSystemOrderId { get; set; } = null;

    public Guid? OldSystemSkuId { get; set; } = null;

    public int Ordinal { get; set; } = default!;

    public short Quantity { get; set; } = default!;

    [NotMapped]
    public string? CreatedOnString { get; set; } = null;
    #endregion

    #region Derived Properties
    public DateTimeOffset CreatedOn => 
        !DateTimeOffset.TryParse(CreatedOnString ?? String.Empty, out var createdOn) ? DateTimeOffset.MinValue : createdOn;
    #endregion
}