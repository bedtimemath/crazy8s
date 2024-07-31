SELECT up.[Id] AS [OldSystemUsaPostalId],
	up.[RecipientName], up.[BusinessName], up.[StreetAddress], up.[City],
	us.[Abbreviation] AS [State], up.[PostalCode], us.[TimeZoneId] AS [TimeZone], us.[IsMilitary]
FROM [Bits].[UsaPostal] up
JOIN [Bits].[UsaState] us ON us.[Id] = up.[StateId]
WHERE up.[DeletedBy] IS NOT NULL;

SELECT up.[Id] AS [OldSystemUsaPostalId], up.[RecipientName], up.[BusinessName], up.[StreetAddress], up.[City], us.[Abbreviation] AS [State], up.[PostalCode], us.[TimeZoneId] AS [TimeZone], us.[IsMilitary] FROM [Bits].[UsaPostal] up JOIN [Bits].[UsaState] us ON us.[Id] = up.[StateId] WHERE up.[DeletedBy] IS NOT NULL
;
