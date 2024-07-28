SELECT a.[Id] AS [OldSystemApplicationId], o.[Id] AS [OldSystemOrganizationId], 
	c.[Name], c.[TimeZoneId] AS [TimeZone], c.[CultureId] As [Culture], t.[Name] AS [OldSystemType], o.[OrganizationTypeOther] AS [TypeOther], 
	CASE WHEN LEN(o.[TaxId]) = 0 THEN NULL ELSE o.[TaxId] END AS [TaxIdentifier], o.[Notes] As [OldSystemNotes], o.[Created] As [CreatedOn] 
FROM [Crazy8s].[Application] a 
WHERE a.[DeletedBy] IS NULL