SELECT c.[Id] AS [OldSystemCompanyId], o.[Id] AS [OldSystemOrganizationid],
	c.[Name], c.[TimeZoneId] AS [TimeZone], c.[CultureId] As [Culture],
	t.[Name] AS [Type], o.[OrganizationTypeOther] AS [TypeOther],
	CASE WHEN LEN(o.[TaxId]) = 0 THEN NULL ELSE o.[TaxId] END AS [TaxIdentifier], 
	o.[Notes] As [OldSystemNotes],
	o.[Created] As [CreatedOn]
FROM [Crazy8s].[Organization] o
LEFT JOIN [Crazy8s].[OrganizationType] t ON t.[Id] = o.[OrganizationTypeId]
LEFT JOIN [Bits].[Company] c ON o.[CompanyId] = c.[Id]
WHERE c.[DeletedBy] IS NULL AND o.[DeletedBy] IS NULL
;

SELECT c.[Id] AS [OldSystemCompanyId], o.[Id] AS [OldSystemOrganizationid], c.[Name], c.[TimeZoneId] AS [TimeZone], c.[CultureId] As [Culture], t.[Name] AS [Type], o.[OrganizationTypeOther] AS [TypeOther], CASE WHEN LEN(o.[TaxId]) = 0 THEN NULL ELSE o.[TaxId] END AS [TaxIdentifier],  o.[Notes] As [OldSystemNotes], o.[Created] As [CreatedOn] FROM [Crazy8s].[Organization] o LEFT JOIN [Crazy8s].[OrganizationType] t ON t.[Id] = o.[OrganizationTypeId] LEFT JOIN [Bits].[Company] c ON o.[CompanyId] = c.[Id] WHERE c.[DeletedBy] IS NULL AND o.[DeletedBy] IS NULL