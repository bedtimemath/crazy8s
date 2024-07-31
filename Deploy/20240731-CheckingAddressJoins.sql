SELECT -- c.[Id] AS [ClubAddress], o.[Id] AS [OrgAddress], r.[Id] AS [OrderAddress], a.[Id] AS [ApplicationAddress], 
	up.[Id], COUNT(*)
FROM [Bits].[UsaPostal] up
LEFT JOIN [Crazy8s].[Organization] o ON o.[PostalAddressId] = up.[Id] AND o.[DeletedBy] IS NULL
LEFT JOIN [Crazy8s].[Club] c ON c.[MeetingAddressId] = up.[Id] AND c.[DeletedBy] IS NULL
LEFT JOIN [Crazy8s].[Order] r ON r.[ShippingAddressId] = up.[Id] AND r.[DeletedBy] IS NULL
LEFT JOIN [Crazy8s].[Application] a ON a.[NewOrganizationAddressId] = up.[Id] AND a.[DeletedBy] IS NULL
WHERE up.[DeletedBy] IS NULL
	AND (c.[Id] IS NOT NULL OR o.[Id] IS NOT NULL OR r.[Id] IS NOT NULL OR a.[Id] IS NOT NULL)
GROUP BY up.[Id]
HAVING COUNT(*) != 1