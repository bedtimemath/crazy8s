SELECT os.[Id] AS [OldSystemOrderSkuId], os.[OrderId] AS [OldSystemOrderId], os.[SkuId] AS [OldSystemSkuId],
	os.[Ordinal], os.[Quantity],
	CAST(os.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[OrderSku] os
WHERE os.[DeletedBy] IS NULL
;

SELECT os.[Id] AS [OldSystemOrderSkuId], os.[OrderId] AS [OldSystemOrderId], os.[SkuId] AS [OldSystemSkuId], os.[Ordinal], os.[Quantity], CAST(os.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[OrderSku] os WHERE os.[DeletedBy] IS NULL
;
