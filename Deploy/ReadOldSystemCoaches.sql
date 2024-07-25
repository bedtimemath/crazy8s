SELECT CAST(c.[Id] AS NVARCHAR(50)) AS [OldSystemCoachIdString], CAST(c.[OrganizationId] AS NVARCHAR(50)) AS [OldSystemOrganizationIdString], 
	CAST(u.[Id] AS NVARCHAR(50)) AS [OldSystemUserIdString], CAST(u.[CompanyId] AS NVARCHAR(50)) AS [OldSystemCompanyIdString], 
	c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Notes] As [OldSystemNotes], 
	c.[Created] AS [CreatedOn] 
FROM [Crazy8s].[Coach] c 
LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId] 
WHERE c.[DeletedBy] IS NULL AND u.[DeletedBy] IS NULL
ORDER BY c.[Created]
;


SELECT CAST(c.[Id] AS NVARCHAR(50)) AS [OldSystemCoachIdString], CAST(c.[OrganizationId] AS NVARCHAR(50)) AS [OldSystemOrganizationIdString], CAST(u.[Id] AS NVARCHAR(50)) AS [OldSystemUserIdString], CAST(u.[CompanyId] AS NVARCHAR(50)) AS [OldSystemCompanyIdString], c.[FirstName], c.[LastName], c.[Email], c.[TimeZoneId] AS [TimeZone], c.[Phone] AS [PhoneString], c.[PhoneExt], c.[Notes] As [OldSystemNotes], c.[Created] AS [CreatedOn] FROM [Crazy8s].[Coach] c LEFT JOIN [Bits].[User] u ON u.[Id] = c.[UserId] WHERE c.[DeletedBy] IS NULL AND u.[DeletedBy] IS NULL
