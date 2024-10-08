SELECT DISTINCT aps.[Name]
FROM [Crazy8s].[Application] a 
LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId]
LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId]
LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId]
WHERE a.[DeletedBy] IS NULL;

SELECT DISTINCT ct.[Name]
FROM [Crazy8s].[Application] a 
LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId]
LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId]
LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId]
WHERE a.[DeletedBy] IS NULL;

SELECT DISTINCT ot.[Name]
FROM [Crazy8s].[Application] a 
LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId]
LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId]
LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId]
WHERE a.[DeletedBy] IS NULL;

SELECT DISTINCT al.[Name]
FROM [Crazy8s].[ApplicationClub] ac
LEFT JOIN [Crazy8s].[AgeLevel] al ON al.[Id] = ac.[AgeLevelId]
WHERE ac.DeletedBy IS NULL
ORDER BY al.[Name];

SELECT DISTINCT cs.[Name]
FROM [Crazy8s].[ApplicationClub] ac
LEFT JOIN [Crazy8s].[ClubSize] cs ON cs.[Id] = ac.[ClubSizeId]
WHERE ac.DeletedBy IS NULL
ORDER BY cs.[Name];

SELECT DISTINCT os.[Name]
FROM [Crazy8s].[Order] o
LEFT JOIN [Crazy8s].[OrderStatus] os ON os.[Id] = o.[StatusId]
WHERE o.DeletedBy IS NULL
ORDER BY os.[Name];

SELECT DISTINCT ss.[Name]
FROM [Crazy8s].[Sku] s
LEFT JOIN [Crazy8s].[SkuStatus] ss ON ss.[Id] = s.[StatusId]
WHERE s.DeletedBy IS NULL
ORDER BY ss.[Name];
