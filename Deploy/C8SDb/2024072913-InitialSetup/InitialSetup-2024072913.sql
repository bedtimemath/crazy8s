IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE TABLE [Organizations] (
        [OrganizationId] int NOT NULL IDENTITY,
        [OldSystemCompanyId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [Name] nvarchar(512) NOT NULL,
        [TimeZone] nvarchar(50) NOT NULL,
        [Culture] nvarchar(25) NOT NULL,
        [Type] nvarchar(25) NOT NULL,
        [TypeOther] nvarchar(50) NULL,
        [TaxIdentifier] nvarchar(25) NULL,
        [OldSystemNotes] nvarchar(max) NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Organizations] PRIMARY KEY ([OrganizationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE TABLE [Coaches] (
        [CoachId] int NOT NULL IDENTITY,
        [OldSystemCoachId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [OldSystemUserId] uniqueidentifier NULL,
        [OldSystemCompanyId] uniqueidentifier NULL,
        [FirstName] nvarchar(255) NOT NULL,
        [LastName] nvarchar(255) NOT NULL,
        [Email] nvarchar(255) NOT NULL,
        [TimeZone] nvarchar(50) NOT NULL,
        [Phone] nvarchar(25) NULL,
        [PhoneExt] nvarchar(25) NULL,
        [OldSystemNotes] nvarchar(max) NULL,
        [OrganizationId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Coaches] PRIMARY KEY ([CoachId]),
        CONSTRAINT [FK_Coaches_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE TABLE [Applications] (
        [ApplicationId] int NOT NULL IDENTITY,
        [OldSystemApplicationId] uniqueidentifier NULL,
        [OldSystemLinkedCoachId] uniqueidentifier NULL,
        [OldSystemLinkedOrganizationId] uniqueidentifier NULL,
        [Status] nvarchar(25) NOT NULL,
        [ApplicantType] nvarchar(25) NOT NULL,
        [ApplicantFirstName] nvarchar(255) NOT NULL,
        [ApplicantLastName] nvarchar(255) NOT NULL,
        [ApplicantEmail] nvarchar(255) NOT NULL,
        [ApplicantPhone] nvarchar(25) NOT NULL,
        [ApplicantPhoneExt] nvarchar(25) NOT NULL,
        [ApplicantTimeZone] nvarchar(50) NOT NULL,
        [OrganizationName] nvarchar(512) NULL,
        [OrganizationType] nvarchar(25) NULL,
        [OrganizationTypeOther] nvarchar(50) NULL,
        [OrganizationTaxIdentifier] nvarchar(50) NULL,
        [WorkshopCode] nvarchar(50) NULL,
        [Comments] nvarchar(max) NULL,
        [SubmittedOn] datetimeoffset NOT NULL,
        [OldSystemNotes] nvarchar(max) NULL,
        [LinkedCoachId] int NULL,
        [LinkedOrganizationId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Applications] PRIMARY KEY ([ApplicationId]),
        CONSTRAINT [FK_Applications_Coaches_LinkedCoachId] FOREIGN KEY ([LinkedCoachId]) REFERENCES [Coaches] ([CoachId]),
        CONSTRAINT [FK_Applications_Organizations_LinkedOrganizationId] FOREIGN KEY ([LinkedOrganizationId]) REFERENCES [Organizations] ([OrganizationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE TABLE [ApplicationClubs] (
        [ApplicationClubId] int NOT NULL IDENTITY,
        [OldSystemApplicationClubId] uniqueidentifier NULL,
        [OldSystemApplicationId] uniqueidentifier NULL,
        [AgeLevel] nvarchar(25) NOT NULL,
        [ClubSize] nvarchar(25) NOT NULL,
        [Season] int NOT NULL,
        [StartsOn] date NOT NULL,
        [ApplicationId] int NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_ApplicationClubs] PRIMARY KEY ([ApplicationClubId]),
        CONSTRAINT [FK_ApplicationClubs_Applications_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Applications] ([ApplicationId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_ApplicationClubs_ApplicationId] ON [ApplicationClubs] ([ApplicationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_ApplicationClubs_OldSystemApplicationClubId] ON [ApplicationClubs] ([OldSystemApplicationClubId]) WHERE [OldSystemApplicationClubId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Applications_LinkedCoachId] ON [Applications] ([LinkedCoachId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Applications_LinkedOrganizationId] ON [Applications] ([LinkedOrganizationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Applications_OldSystemApplicationId] ON [Applications] ([OldSystemApplicationId]) WHERE [OldSystemApplicationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Coaches_OldSystemCoachId] ON [Coaches] ([OldSystemCoachId]) WHERE [OldSystemCoachId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Coaches_OrganizationId] ON [Coaches] ([OrganizationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Organizations_OldSystemOrganizationId] ON [Organizations] ([OldSystemOrganizationId]) WHERE [OldSystemOrganizationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240729134718_InitialSetup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240729134718_InitialSetup', N'8.0.7');
END;
GO

COMMIT;
GO

