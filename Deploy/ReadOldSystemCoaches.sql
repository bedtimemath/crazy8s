SELECT c.[Id] AS [OldSystemCoachId], c.[OrganizationId] AS [OldSystemOrganizationId], u.[Id] AS [OldSystemUserId], u.[CompanyId] AS [OldSystemCompanyId], 
	c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Notes] As [OldSystemNotes],
	CAST(c.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[Coach] c 
LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId] 
WHERE c.[DeletedBy] IS NULL AND u.[DeletedBy] IS NULL
ORDER BY c.[Created]
;


SELECT c.[Id] AS [OldSystemCoachId], c.[OrganizationId] AS [OldSystemOrganizationId], u.[Id] AS [OldSystemUserId], u.[CompanyId] AS [OldSystemCompanyId], c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Notes] As [OldSystemNotes], CAST(c.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Coach] c  LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId]  WHERE c.[DeletedBy] IS NULL AND u.[DeletedBy] IS NULL
;
