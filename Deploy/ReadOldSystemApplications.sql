SELECT a.[Id] AS [OldSystemApplicationId], a.[LinkedCoachId] AS [OldSystemLinkedCoachId], a.[LinkedOrganizationId] AS [OldSystemLinkedOrganizationId],
	aps.[Name] AS [StatusString], ct.[Name] AS [ApplicantTypeString],
	a.[CoachFirstName] AS [ApplicantFirstName], a.[CoachLastName] AS [ApplicantLastName], a.[CoachEmail] AS [ApplicantEmail],
	a.[CoachPhone] AS [ApplicantPhoneString], a.[CoachPhoneExt] AS [ApplicantPhoneExt], a.[CoachTimeZoneId] AS [ApplicantTimeZone],
	a.[NewOrganizationName] AS [OrganizationName], ot.[Name] AS [OrganizationTypeString], a.[NewOrganizationOrganizationTypeOther] AS [OrganizationTypeOther],
	a.[NewOrganizationTaxId] AS [OrganizationTaxIdentifier], a.[WorkshopCode], a.[Comments], a.[Submitted] AS [SubmittedOn], a.[Notes] AS [OldSystemNotes],
	CAST(a.[Created] AS VARCHAR) AS [CreatedOnString]
FROM [Crazy8s].[Application] a 
LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId]
LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId]
LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId]
WHERE a.[DeletedBy] IS NULL
;

SELECT a.[Id] AS [OldSystemApplicationId], a.[LinkedCoachId] AS [OldSystemLinkedCoachId], a.[LinkedOrganizationId] AS [OldSystemLinkedOrganizationId], aps.[Name] AS [StatusString], ct.[Name] AS [ApplicantTypeString], a.[CoachFirstName] AS [ApplicantFirstName], a.[CoachLastName] AS [ApplicantLastName], a.[CoachEmail] AS [ApplicantEmail], a.[CoachPhone] AS [ApplicantPhoneString], a.[CoachPhoneExt] AS [ApplicantPhoneExt], a.[CoachTimeZoneId] AS [ApplicantTimeZone], a.[NewOrganizationName] AS [OrganizationName], ot.[Name] AS [OrganizationTypeString], a.[NewOrganizationOrganizationTypeOther] AS [OrganizationTypeOther], a.[NewOrganizationTaxId] AS [OrganizationTaxIdentifier], a.[WorkshopCode], a.[Comments], a.[Submitted] AS [SubmittedOn], a.[Notes] AS [OldSystemNotes], CAST(a.[Created] AS VARCHAR) AS [CreatedOnString] FROM [Crazy8s].[Application] a  LEFT JOIN [Crazy8s].[ApplicationStatus] aps ON aps.[Id] = a.[ApplicationStatusId] LEFT JOIN [Crazy8s].[CoachType] ct ON ct.[Id] = a.[CoachTypeId] LEFT JOIN [Crazy8s].[OrganizationType] ot ON ot.[Id] = a.[NewOrganizationOrganizationTypeId] WHERE a.[DeletedBy] IS NULL
;
