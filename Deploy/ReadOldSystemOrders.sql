SELECT o.[Id] AS [OldSystemOrderId], o.[ShippingAddressId] AS [OldSystemShippingAddressId], o.[ClubId] AS [OldSystemClubId],
	o.[Number], os.[Name] AS [StatusString],
	o.[ContactEmail], o.[ContactPhone], o.[ContactPhoneExt],
	o.[Ordered] AS [OrderedOnString], o.[ArriveBy] AS [ArriveByString], o.[Shipped] AS [ShippedOnString], o.[Emailed] AS [EmailedOnString],
	o.[BatchId] AS [BatchIdentifier], o.[Notes],
	CAST(o.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[Order] o 
LEFT JOIN [Crazy8s].[OrderStatus] os ON os.[Id] = o.[StatusId]
WHERE o.[DeletedBy] IS NULL
;

SELECT a.[Id] AS [OldSystemApplicationId], a.[NewOrganizationAddressId] AS [OldSystemAddressId], a.[LinkedCoachId] AS [OldSystemLinkedCoachId], a.[LinkedOrganizationId] AS [OldSystemLinkedOrganizationId], aps.[Name] AS [StatusString], ct.[Name] AS [ApplicantTypeString], a.[CoachFirstName] AS [ApplicantFirstName], a.[CoachLastName] AS [ApplicantLastName], a.[CoachEmail] AS [ApplicantEmail], a.[CoachPhone] AS [ApplicantPhoneString], a.[CoachPhoneExt] AS [ApplicantPhoneExt], a.[CoachTimeZoneId] AS [ApplicantTimeZone], a.[NewOrganizationName] AS [OrganizationName], ot.[Name] AS [OrganizationTypeString], a.[NewOrganizationOrganizationTypeOther] AS [OrganizationTypeOther], a.[NewOrganizationTaxId] AS [OrganizationTaxIdentifier], a.[WorkshopCode], a.[Comments], a.[Submitted] AS [SubmittedOn], a.[Notes], CAST(a.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Application] a  LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId] LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId] LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId] WHERE a.[DeletedBy] IS NULL
;
