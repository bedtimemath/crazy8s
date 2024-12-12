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
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Invoices] (
        [InvoiceId] int NOT NULL IDENTITY,
        [Status] nvarchar(25) NOT NULL,
        [Identifier] nvarchar(255) NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Invoices] PRIMARY KEY ([InvoiceId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Places] (
        [PlaceId] int NOT NULL IDENTITY,
        [OldSystemCompanyId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [OldSystemPostalAddressId] uniqueidentifier NULL,
        [OldSystemUsaPostalId] uniqueidentifier NULL,
        [Name] nvarchar(512) NOT NULL,
        [Type] nvarchar(25) NOT NULL,
        [TypeOther] nvarchar(50) NULL,
        [TaxIdentifier] nvarchar(25) NULL,
        [Line1] nvarchar(255) NOT NULL,
        [Line2] nvarchar(255) NULL,
        [City] nvarchar(50) NOT NULL,
        [State] nvarchar(5) NOT NULL,
        [ZIPCode] nvarchar(25) NOT NULL,
        [IsMilitary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [ParentId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Places] PRIMARY KEY ([PlaceId]),
        CONSTRAINT [FK_Places_Places_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Places] ([PlaceId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Skus] (
        [SkuId] int NOT NULL IDENTITY,
        [OldSystemSkuId] uniqueidentifier NULL,
        [Key] nvarchar(50) NOT NULL,
        [Name] nvarchar(255) NOT NULL,
        [Status] nvarchar(25) NOT NULL,
        [Season] int NULL,
        [AgeLevel] nvarchar(25) NULL,
        [ClubSize] nvarchar(25) NULL,
        [Comments] nvarchar(1024) NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Skus] PRIMARY KEY ([SkuId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [WorkshopCodes] (
        [WorkshopCodeId] int NOT NULL IDENTITY,
        [Key] nvarchar(50) NOT NULL,
        [StartsOn] datetimeoffset NULL,
        [EndsOn] datetimeoffset NULL,
        CONSTRAINT [PK_WorkshopCodes] PRIMARY KEY ([WorkshopCodeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Persons] (
        [PersonId] int NOT NULL IDENTITY,
        [OldSystemCoachId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [OldSystemUserId] uniqueidentifier NULL,
        [OldSystemCompanyId] uniqueidentifier NULL,
        [FirstName] nvarchar(255) NULL,
        [LastName] nvarchar(255) NOT NULL,
        [Email] nvarchar(255) NULL,
        [TimeZone] nvarchar(50) NULL,
        [Phone] nvarchar(255) NULL,
        [JobTitle] nvarchar(25) NULL,
        [JobTitleOther] nvarchar(50) NULL,
        [WordPressUser] nvarchar(255) NULL,
        [PlaceId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Persons] PRIMARY KEY ([PersonId]),
        CONSTRAINT [FK_Persons_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [InvoicePersons] (
        [InvoicePersonId] int NOT NULL IDENTITY,
        [IsPrimary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [PersonId] int NOT NULL,
        [InvoiceId] int NOT NULL,
        CONSTRAINT [PK_InvoicePersons] PRIMARY KEY ([InvoicePersonId]),
        CONSTRAINT [FK_InvoicePersons_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]) ON DELETE CASCADE,
        CONSTRAINT [FK_InvoicePersons_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Permissions] (
        [PermissionId] int NOT NULL IDENTITY,
        [IsPrimary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [PersonId] int NOT NULL,
        [SkuId] int NOT NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([PermissionId]),
        CONSTRAINT [FK_Permissions_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]),
        CONSTRAINT [FK_Permissions_Skus_SkuId] FOREIGN KEY ([SkuId]) REFERENCES [Skus] ([SkuId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Requests] (
        [RequestId] int NOT NULL IDENTITY,
        [OldSystemApplicationId] uniqueidentifier NULL,
        [OldSystemAddressId] uniqueidentifier NULL,
        [OldSystemLinkedCoachId] uniqueidentifier NULL,
        [OldSystemLinkedOrganizationId] uniqueidentifier NULL,
        [Status] nvarchar(25) NOT NULL,
        [PersonType] nvarchar(25) NULL,
        [PersonFirstName] nvarchar(255) NULL,
        [PersonLastName] nvarchar(255) NOT NULL,
        [PersonEmail] nvarchar(255) NOT NULL,
        [PersonPhone] nvarchar(255) NULL,
        [PersonTimeZone] nvarchar(50) NOT NULL,
        [PlaceName] nvarchar(512) NULL,
        [PlaceType] nvarchar(25) NULL,
        [PlaceTypeOther] nvarchar(50) NULL,
        [PlaceTaxIdentifier] nvarchar(50) NULL,
        [WorkshopCode] nvarchar(50) NULL,
        [ReferenceSource] nvarchar(50) NULL,
        [ReferenceSourceOther] nvarchar(512) NULL,
        [Comments] nvarchar(max) NULL,
        [SubmittedOn] datetimeoffset NOT NULL,
        [PersonId] int NULL,
        [PlaceId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Requests] PRIMARY KEY ([RequestId]),
        CONSTRAINT [FK_Requests_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]),
        CONSTRAINT [FK_Requests_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [RequestedClubs] (
        [RequestedClubId] int NOT NULL IDENTITY,
        [OldSystemApplicationClubId] uniqueidentifier NULL,
        [OldSystemApplicationId] uniqueidentifier NULL,
        [OldSystemLinkedClubId] uniqueidentifier NULL,
        [AgeLevel] nvarchar(25) NOT NULL,
        [ClubSize] nvarchar(25) NOT NULL,
        [Season] int NOT NULL,
        [StartsOn] date NOT NULL,
        [ApplicationId] int NOT NULL,
        CONSTRAINT [PK_RequestedClubs] PRIMARY KEY ([RequestedClubId]),
        CONSTRAINT [FK_RequestedClubs_Requests_ApplicationId] FOREIGN KEY ([ApplicationId]) REFERENCES [Requests] ([RequestId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Sales] (
        [SaleId] int NOT NULL IDENTITY,
        [Status] nvarchar(25) NOT NULL,
        [PlaceId] int NOT NULL,
        [RequestId] int NULL,
        [InvoiceId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Sales] PRIMARY KEY ([SaleId]),
        CONSTRAINT [FK_Sales_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]),
        CONSTRAINT [FK_Sales_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId]),
        CONSTRAINT [FK_Sales_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([RequestId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Unfinisheds] (
        [UnfinishedId] int NOT NULL IDENTITY,
        [Code] uniqueidentifier NOT NULL DEFAULT (NEWID()),
        [PersonType] nvarchar(25) NULL,
        [PersonFirstName] nvarchar(255) NULL,
        [PersonLastName] nvarchar(255) NULL,
        [PersonEmail] nvarchar(255) NULL,
        [PersonPhone] nvarchar(255) NULL,
        [PersonTimeZone] nvarchar(50) NULL,
        [HasHostedBefore] bit NULL,
        [PlaceName] nvarchar(512) NULL,
        [PlaceAddress1] nvarchar(255) NULL,
        [PlaceAddress2] nvarchar(255) NULL,
        [PlaceCity] nvarchar(50) NULL,
        [PlaceState] nvarchar(5) NULL,
        [PlacePostalCode] nvarchar(25) NULL,
        [PlaceType] nvarchar(25) NULL,
        [PlaceTypeOther] nvarchar(50) NULL,
        [PlaceTaxIdentifier] nvarchar(50) NULL,
        [ClubsString] nvarchar(1024) NULL,
        [WorkshopCode] nvarchar(50) NULL,
        [ChosenTimeSlot] datetimeoffset NULL,
        [ReferenceSource] nvarchar(50) NULL,
        [ReferenceSourceOther] nvarchar(512) NULL,
        [Comments] nvarchar(max) NULL,
        [EndPart01On] datetimeoffset NULL,
        [EndPart02On] datetimeoffset NULL,
        [EndPart03On] datetimeoffset NULL,
        [EndPart04On] datetimeoffset NULL,
        [SubmittedOn] datetimeoffset NULL,
        [RequestId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Unfinisheds] PRIMARY KEY ([UnfinishedId]),
        CONSTRAINT [FK_Unfinisheds_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([RequestId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Clubs] (
        [ClubId] int NOT NULL IDENTITY,
        [OldSystemClubId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NULL,
        [OldSystemCoachId] uniqueidentifier NULL,
        [OldSystemMeetingAddressId] uniqueidentifier NULL,
        [Status] nvarchar(25) NOT NULL,
        [Season] int NULL,
        [AgeLevel] nvarchar(25) NULL,
        [ClubSize] nvarchar(25) NULL,
        [StartsOn] date NULL,
        [PlaceId] int NOT NULL,
        [SaleId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Clubs] PRIMARY KEY ([ClubId]),
        CONSTRAINT [FK_Clubs_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Clubs_Sales_SaleId] FOREIGN KEY ([SaleId]) REFERENCES [Sales] ([SaleId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [SalePersons] (
        [SalePersonId] int NOT NULL IDENTITY,
        [IsPrimary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [PersonId] int NOT NULL,
        [SaleId] int NOT NULL,
        CONSTRAINT [PK_SalePersons] PRIMARY KEY ([SalePersonId]),
        CONSTRAINT [FK_SalePersons_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]) ON DELETE CASCADE,
        CONSTRAINT [FK_SalePersons_Sales_SaleId] FOREIGN KEY ([SaleId]) REFERENCES [Sales] ([SaleId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [ClubPersons] (
        [ClubPersonId] int NOT NULL IDENTITY,
        [IsPrimary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [PersonId] int NOT NULL,
        [ClubId] int NOT NULL,
        CONSTRAINT [PK_ClubPersons] PRIMARY KEY ([ClubPersonId]),
        CONSTRAINT [FK_ClubPersons_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([ClubId]) ON DELETE CASCADE,
        CONSTRAINT [FK_ClubPersons_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Orders] (
        [OrderId] int NOT NULL IDENTITY,
        [OldSystemOrderId] uniqueidentifier NULL,
        [OldSystemShippingAddressId] uniqueidentifier NULL,
        [OldSystemClubId] uniqueidentifier NULL,
        [Number] int NOT NULL,
        [Status] nvarchar(25) NOT NULL,
        [ContactName] nvarchar(512) NULL,
        [ContactEmail] nvarchar(255) NULL,
        [ContactPhone] nvarchar(255) NULL,
        [Recipient] nvarchar(512) NOT NULL,
        [Line1] nvarchar(255) NOT NULL,
        [Line2] nvarchar(255) NULL,
        [City] nvarchar(50) NOT NULL,
        [State] nvarchar(5) NOT NULL,
        [ZIPCode] nvarchar(25) NOT NULL,
        [IsMilitary] bit NOT NULL DEFAULT CAST(0 AS bit),
        [OrderedOn] datetimeoffset NOT NULL,
        [ArriveBy] date NULL,
        [ShippedOn] datetimeoffset NULL,
        [EmailedOn] datetimeoffset NULL,
        [ClubId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
        CONSTRAINT [FK_Orders_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([ClubId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Notes] (
        [NoteId] int NOT NULL IDENTITY,
        [Reference] nvarchar(25) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [Author] nvarchar(512) NOT NULL,
        [ClubId] int NULL,
        [InvoiceId] int NULL,
        [OrderId] int NULL,
        [PersonId] int NULL,
        [PlaceId] int NULL,
        [RequestId] int NULL,
        [SaleId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Notes] PRIMARY KEY ([NoteId]),
        CONSTRAINT [FK_Notes_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([ClubId]),
        CONSTRAINT [FK_Notes_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]),
        CONSTRAINT [FK_Notes_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]),
        CONSTRAINT [FK_Notes_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]),
        CONSTRAINT [FK_Notes_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId]),
        CONSTRAINT [FK_Notes_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([RequestId]),
        CONSTRAINT [FK_Notes_Sales_SaleId] FOREIGN KEY ([SaleId]) REFERENCES [Sales] ([SaleId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
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
        CONSTRAINT [PK_OrderSkus] PRIMARY KEY ([OrderSkuId]),
        CONSTRAINT [FK_OrderSkus_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderSkus_Skus_SkuId] FOREIGN KEY ([SkuId]) REFERENCES [Skus] ([SkuId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE TABLE [Shipments] (
        [ShipmentId] int NOT NULL IDENTITY,
        [TrackingNumber] nvarchar(50) NOT NULL,
        [ShipMethod] nvarchar(25) NULL,
        [ShipMethodOther] nvarchar(50) NULL,
        [OrderId] int NULL,
        CONSTRAINT [PK_Shipments] PRIMARY KEY ([ShipmentId]),
        CONSTRAINT [FK_Shipments_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_ClubPersons_ClubId] ON [ClubPersons] ([ClubId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_ClubPersons_PersonId] ON [ClubPersons] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Clubs_OldSystemClubId] ON [Clubs] ([OldSystemClubId]) WHERE [OldSystemClubId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_PlaceId] ON [Clubs] ([PlaceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_SaleId] ON [Clubs] ([SaleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_InvoicePersons_InvoiceId] ON [InvoicePersons] ([InvoiceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_InvoicePersons_PersonId] ON [InvoicePersons] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_ClubId] ON [Notes] ([ClubId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_InvoiceId] ON [Notes] ([InvoiceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_OrderId] ON [Notes] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_PersonId] ON [Notes] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_PlaceId] ON [Notes] ([PlaceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_RequestId] ON [Notes] ([RequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_SaleId] ON [Notes] ([SaleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Orders_ClubId] ON [Orders] ([ClubId]) WHERE [ClubId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Orders_OldSystemOrderId] ON [Orders] ([OldSystemOrderId]) WHERE [OldSystemOrderId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_OrderSkus_OldSystemOrderSkuId] ON [OrderSkus] ([OldSystemOrderSkuId]) WHERE [OldSystemOrderSkuId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OrderSkus_OrderId] ON [OrderSkus] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OrderSkus_SkuId] ON [OrderSkus] ([SkuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Permissions_PersonId] ON [Permissions] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Permissions_SkuId] ON [Permissions] ([SkuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Persons_OldSystemCoachId] ON [Persons] ([OldSystemCoachId]) WHERE [OldSystemCoachId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Persons_PlaceId] ON [Persons] ([PlaceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Places_OldSystemOrganizationId] ON [Places] ([OldSystemOrganizationId]) WHERE [OldSystemOrganizationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Places_ParentId] ON [Places] ([ParentId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_RequestedClubs_ApplicationId] ON [RequestedClubs] ([ApplicationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_RequestedClubs_OldSystemApplicationClubId] ON [RequestedClubs] ([OldSystemApplicationClubId]) WHERE [OldSystemApplicationClubId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Requests_OldSystemApplicationId] ON [Requests] ([OldSystemApplicationId]) WHERE [OldSystemApplicationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Requests_PersonId] ON [Requests] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Requests_PlaceId] ON [Requests] ([PlaceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_SalePersons_PersonId] ON [SalePersons] ([PersonId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_SalePersons_SaleId] ON [SalePersons] ([SaleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Sales_InvoiceId] ON [Sales] ([InvoiceId]) WHERE [InvoiceId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Sales_PlaceId] ON [Sales] ([PlaceId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Sales_RequestId] ON [Sales] ([RequestId]) WHERE [RequestId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Shipments_OrderId] ON [Shipments] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Skus_OldSystemSkuId] ON [Skus] ([OldSystemSkuId]) WHERE [OldSystemSkuId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Unfinisheds_Code] ON [Unfinisheds] ([Code]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Unfinisheds_RequestId] ON [Unfinisheds] ([RequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212183855_InitialSetup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241212183855_InitialSetup', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212193304_FixRequestNaming'
)
BEGIN
    ALTER TABLE [RequestedClubs] DROP CONSTRAINT [FK_RequestedClubs_Requests_ApplicationId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212193304_FixRequestNaming'
)
BEGIN
    EXEC sp_rename N'[RequestedClubs].[ApplicationId]', N'RequestId', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212193304_FixRequestNaming'
)
BEGIN
    EXEC sp_rename N'[RequestedClubs].[IX_RequestedClubs_ApplicationId]', N'IX_RequestedClubs_RequestId', N'INDEX';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212193304_FixRequestNaming'
)
BEGIN
    ALTER TABLE [RequestedClubs] ADD CONSTRAINT [FK_RequestedClubs_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([RequestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241212193304_FixRequestNaming'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241212193304_FixRequestNaming', N'8.0.10');
END;
GO

COMMIT;
GO

