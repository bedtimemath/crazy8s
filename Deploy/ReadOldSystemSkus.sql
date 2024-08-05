SELECT k.[Id] AS [OldSystemSkuId],
	k.[Key], k.[Name], ss.[Name] AS [StatusString],
	s.[Ordinal] AS [Season], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString],
	k.[PackingListNotes], CAST(k.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[Sku] k 
LEFT JOIN [Crazy8s].[SkuStatus] ss ON ss.[Id] = k.[StatusId]
LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = k.[AgeLevelId]
LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = k.[ClubSizeId]
LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = k.[SeasonId]
WHERE k.[DeletedBy] IS NULL
;

SELECT k.[Id] AS [OldSystemSkuId], k.[Key], k.[Name], ss.[Name] AS [StatusString], s.[Ordinal] AS [Season], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], k.[PackingListNotes], CAST(k.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Sku] k  LEFT JOIN [Crazy8s].[SkuStatus] ss ON ss.[Id] = k.[StatusId] LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = k.[AgeLevelId] LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = k.[ClubSizeId] LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = k.[SeasonId] WHERE k.[DeletedBy] IS NULL
;