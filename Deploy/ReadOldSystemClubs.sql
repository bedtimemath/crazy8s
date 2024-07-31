SELECT c.[Id] AS [OldSystemClubId], c.[OrganizationId] AS [OldSystemOrganizationId],
	c.[CoachId] AS [OldSystemCoachId], c.[MeetingAddressId] AS [OldSystemMeetingAddressId],
	al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], s.[Ordinal] AS [Season],
	CAST(c.[Starts] AS DATE) AS [StartsOnDateTime], c.[Notes],
	CAST(c.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[Club] c 
LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = c.[AgeLevelId]
LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = c.[ClubSizeId]
LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = c.[SeasonId]
LEFT JOIN [Bits].[UsaPostal] up ON up.[Id] = c.[MeetingAddressId]
WHERE c.[DeletedBy] IS NULL
;

SELECT c.[Id] AS [OldSystemClubId], c.[OrganizationId] AS [OldSystemOrganizationId], c.[CoachId] AS [OldSystemCoachId], c.[MeetingAddressId] AS [OldSystemMeetingAddressId], al.[Name] AS [AgeLevelString], cs.[Name] AS [ClubSizeString], s.[Ordinal] AS [Season], CAST(c.[Starts] AS DATE) AS [StartsOnDateTime], c.[Notes], CAST(c.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Club] c  LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = c.[AgeLevelId] LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = c.[ClubSizeId] LEFT JOIN [Crazy8s].[Season] s ON s.[Id] = c.[SeasonId] LEFT JOIN [Bits].[UsaPostal] up ON up.[Id] = c.[MeetingAddressId] WHERE c.[DeletedBy] IS NULL
;