SELECT ac.[Id] AS [OldSystemApplicationClubId], ac.[ApplicationId] AS [OldSystemApplicationId], ac.[LinkedClubId] AS [OldSystemLinkedClubId],
	al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], s.[Ordinal] AS [Season],
	CAST(ac.[Starts] AS DATE) AS [StartsOnDateTime],
	CAST(ac.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[ApplicationClub] ac 
LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = ac.[AgeLevelId]
LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = ac.[ClubSizeId]
LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = ac.[SeasonId]
WHERE ac.[DeletedBy] IS NULL
;

SELECT ac.[Id] AS [OldSystemApplicationClubId], ac.[ApplicationId] AS [OldSystemApplicationId], ac.[LinkedClubId] AS [OldSystemLinkedClubId], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], s.[Ordinal] AS [Season], CAST(ac.[Starts] AS DATE) AS [StartsOnDateTime], CAST(ac.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[ApplicationClub] ac  LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = ac.[AgeLevelId] LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = ac.[ClubSizeId] LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = ac.[SeasonId] WHERE ac.[DeletedBy] IS NULL
;
