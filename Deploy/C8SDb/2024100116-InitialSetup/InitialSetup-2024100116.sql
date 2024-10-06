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
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Addresses] (
        [AddressId] int NOT NULL IDENTITY,
        [OldSystemUsaPostalId] uniqueidentifier NULL,
        [RecipientName] nvarchar(255) NULL,
        [BusinessName] nvarchar(255) NULL,
        [StreetAddress] nvarchar(255) NOT NULL,
        [City] nvarchar(50) NOT NULL,
        [State] nvarchar(5) NOT NULL,
        [PostalCode] nvarchar(25) NOT NULL,
        [TimeZone] nvarchar(50) NOT NULL,
        [IsMilitary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Addresses] PRIMARY KEY ([AddressId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Skus] (
        [SkuId] int NOT NULL IDENTITY,
        [OldSystemSkuId] uniqueidentifier NULL,
        [Key] nvarchar(50) NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Status] nvarchar(25) NOT NULL,
        [Season] int NOT NULL,
        [AgeLevel] nvarchar(25) NOT NULL,
        [ClubSize] nvarchar(25) NOT NULL,
        [Notes] nvarchar(max) NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Skus] PRIMARY KEY ([SkuId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Unfinisheds] (
        [UnfinishedId] int NOT NULL IDENTITY,
        [Code] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [ApplicantType] nvarchar(25) NULL,
        [ApplicantFirstName] nvarchar(255) NULL,
        [ApplicantLastName] nvarchar(255) NULL,
        [ApplicantEmail] nvarchar(255) NULL,
        [ApplicantPhone] nvarchar(25) NULL,
        [ApplicantTimeZone] nvarchar(50) NULL,
        [OrganizationName] nvarchar(512) NULL,
        [OrganizationAddress1] nvarchar(255) NULL,
        [OrganizationAddress2] nvarchar(255) NULL,
        [OrganizationCity] nvarchar(50) NULL,
        [OrganizationState] nvarchar(5) NULL,
        [OrganizationPostalCode] nvarchar(25) NULL,
        [OrganizationType] nvarchar(25) NULL,
        [OrganizationTypeOther] nvarchar(50) NULL,
        [OrganizationTaxIdentifier] nvarchar(50) NULL,
        [WorkshopCode] nvarchar(50) NULL,
        [ReferenceSource] nvarchar(50) NULL,
        [ReferenceSourceOther] nvarchar(512) NULL,
        [Comments] nvarchar(max) NULL,
        [SubmittedOn] datetimeoffset NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Unfinisheds] PRIMARY KEY ([UnfinishedId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Organizations] (
        [OrganizationId] int NOT NULL IDENTITY,
        [OldSystemCompanyId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [OldSystemPostalAddressId] uniqueidentifier NULL,
        [Name] nvarchar(512) NOT NULL,
        [TimeZone] nvarchar(50) NOT NULL,
        [Culture] nvarchar(25) NOT NULL,
        [Type] nvarchar(25) NOT NULL,
        [TypeOther] nvarchar(50) NULL,
        [TaxIdentifier] nvarchar(25) NULL,
        [Notes] nvarchar(max) NULL,
        [AddressId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Organizations] PRIMARY KEY ([OrganizationId]),
        CONSTRAINT [FK_Organizations_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([AddressId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
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
        [Notes] nvarchar(max) NULL,
        [OrganizationId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Coaches] PRIMARY KEY ([CoachId]),
        CONSTRAINT [FK_Coaches_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Applications] (
        [ApplicationId] int NOT NULL IDENTITY,
        [OldSystemApplicationId] uniqueidentifier NULL,
        [OldSystemAddressId] uniqueidentifier NULL,
        [OldSystemLinkedCoachId] uniqueidentifier NULL,
        [OldSystemLinkedOrganizationId] uniqueidentifier NULL,
        [Status] nvarchar(25) NOT NULL,
        [ApplicantType] nvarchar(25) NULL,
        [ApplicantFirstName] nvarchar(255) NULL,
        [ApplicantLastName] nvarchar(255) NOT NULL,
        [ApplicantEmail] nvarchar(255) NOT NULL,
        [ApplicantPhone] nvarchar(25) NULL,
        [ApplicantPhoneExt] nvarchar(25) NULL,
        [ApplicantTimeZone] nvarchar(50) NOT NULL,
        [OrganizationName] nvarchar(512) NULL,
        [OrganizationType] nvarchar(25) NULL,
        [OrganizationTypeOther] nvarchar(50) NULL,
        [OrganizationTaxIdentifier] nvarchar(50) NULL,
        [WorkshopCode] nvarchar(50) NULL,
        [ReferenceSource] nvarchar(50) NULL,
        [ReferenceSourceOther] nvarchar(512) NULL,
        [Comments] nvarchar(max) NULL,
        [SubmittedOn] datetimeoffset NOT NULL,
        [IsCoachRemoved] bit NOT NULL DEFAULT CAST(0 AS bit),
        [IsOrganizationRemoved] bit NOT NULL DEFAULT CAST(0 AS bit),
        [Notes] nvarchar(max) NULL,
        [AddressId] int NULL,
        [LinkedCoachId] int NULL,
        [LinkedOrganizationId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Applications] PRIMARY KEY ([ApplicationId]),
        CONSTRAINT [FK_Applications_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([AddressId]),
        CONSTRAINT [FK_Applications_Coaches_LinkedCoachId] FOREIGN KEY ([LinkedCoachId]) REFERENCES [Coaches] ([CoachId]),
        CONSTRAINT [FK_Applications_Organizations_LinkedOrganizationId] FOREIGN KEY ([LinkedOrganizationId]) REFERENCES [Organizations] ([OrganizationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Clubs] (
        [ClubId] int NOT NULL IDENTITY,
        [OldSystemClubId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [OldSystemCoachId] uniqueidentifier NULL,
        [OldSystemMeetingAddressId] uniqueidentifier NULL,
        [AgeLevel] nvarchar(25) NOT NULL,
        [ClubSize] nvarchar(25) NOT NULL,
        [Season] int NOT NULL,
        [StartsOn] date NOT NULL,
        [Notes] nvarchar(max) NULL,
        [CoachId] int NOT NULL,
        [OrganizationId] int NOT NULL,
        [AddressId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Clubs] PRIMARY KEY ([ClubId]),
        CONSTRAINT [FK_Clubs_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([AddressId]),
        CONSTRAINT [FK_Clubs_Coaches_CoachId] FOREIGN KEY ([CoachId]) REFERENCES [Coaches] ([CoachId]),
        CONSTRAINT [FK_Clubs_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [Organizations] ([OrganizationId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [ApplicationClubs] (
        [ApplicationClubId] int NOT NULL IDENTITY,
        [OldSystemApplicationClubId] uniqueidentifier NULL,
        [OldSystemApplicationId] uniqueidentifier NULL,
        [OldSystemLinkedClubId] uniqueidentifier NULL,
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
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [Orders] (
        [OrderId] int NOT NULL IDENTITY,
        [OldSystemOrderId] uniqueidentifier NULL,
        [OldSystemShippingAddressId] uniqueidentifier NULL,
        [OldSystemClubId] uniqueidentifier NULL,
        [Number] int NOT NULL,
        [Status] nvarchar(25) NOT NULL,
        [ContactEmail] nvarchar(255) NULL,
        [ContactPhone] nvarchar(25) NULL,
        [ContactPhoneExt] nvarchar(25) NULL,
        [OrderedOn] datetimeoffset NOT NULL,
        [ArriveBy] date NOT NULL,
        [ShippedOn] datetimeoffset NULL,
        [EmailedOn] datetimeoffset NULL,
        [BatchIdentifier] uniqueidentifier NULL,
        [Notes] nvarchar(max) NULL,
        [AddressId] int NOT NULL,
        [ClubId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
        CONSTRAINT [FK_Orders_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([AddressId]),
        CONSTRAINT [FK_Orders_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([ClubId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE TABLE [OrderSkus] (
        [OrderSkuId] int NOT NULL IDENTITY,
        [OldSystemOrderSkuId] uniqueidentifier NULL,
        [OldSystemOrderId] uniqueidentifier NULL,
        [OldSystemSkuId] uniqueidentifier NULL,
        [Ordinal] int NOT NULL,
        [Quantity] smallint NOT NULL,
        [OrderId] int NOT NULL,
        [SkuId] int NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_OrderSkus] PRIMARY KEY ([OrderSkuId]),
        CONSTRAINT [FK_OrderSkus_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderSkus_Skus_SkuId] FOREIGN KEY ([SkuId]) REFERENCES [Skus] ([SkuId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Addresses_OldSystemUsaPostalId] ON [Addresses] ([OldSystemUsaPostalId]) WHERE [OldSystemUsaPostalId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_ApplicationClubs_ApplicationId] ON [ApplicationClubs] ([ApplicationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_ApplicationClubs_OldSystemApplicationClubId] ON [ApplicationClubs] ([OldSystemApplicationClubId]) WHERE [OldSystemApplicationClubId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Applications_AddressId] ON [Applications] ([AddressId]) WHERE [AddressId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Applications_LinkedCoachId] ON [Applications] ([LinkedCoachId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Applications_LinkedOrganizationId] ON [Applications] ([LinkedOrganizationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Applications_OldSystemApplicationId] ON [Applications] ([OldSystemApplicationId]) WHERE [OldSystemApplicationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Clubs_AddressId] ON [Clubs] ([AddressId]) WHERE [AddressId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_CoachId] ON [Clubs] ([CoachId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Clubs_OldSystemClubId] ON [Clubs] ([OldSystemClubId]) WHERE [OldSystemClubId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_OrganizationId] ON [Clubs] ([OrganizationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Coaches_OldSystemCoachId] ON [Coaches] ([OldSystemCoachId]) WHERE [OldSystemCoachId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Coaches_OrganizationId] ON [Coaches] ([OrganizationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Orders_AddressId] ON [Orders] ([AddressId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Orders_ClubId] ON [Orders] ([ClubId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Orders_OldSystemOrderId] ON [Orders] ([OldSystemOrderId]) WHERE [OldSystemOrderId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OrderSkus_OldSystemOrderSkuId] ON [OrderSkus] ([OldSystemOrderSkuId]) WHERE [OldSystemOrderSkuId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OrderSkus_OrderId] ON [OrderSkus] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OrderSkus_SkuId] ON [OrderSkus] ([SkuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Organizations_AddressId] ON [Organizations] ([AddressId]) WHERE [AddressId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Organizations_OldSystemOrganizationId] ON [Organizations] ([OldSystemOrganizationId]) WHERE [OldSystemOrganizationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Skus_OldSystemSkuId] ON [Skus] ([OldSystemSkuId]) WHERE [OldSystemSkuId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Unfinisheds_Code] ON [Unfinisheds] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241001163517_InitialSetup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241001163517_InitialSetup', N'8.0.8');
END;
GO

COMMIT;
GO

