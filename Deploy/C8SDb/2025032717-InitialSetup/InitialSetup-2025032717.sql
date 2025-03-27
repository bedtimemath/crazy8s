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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [KitPages] (
        [KitPageId] int NOT NULL IDENTITY,
        [Status] nvarchar(25) NOT NULL,
        [Url] nvarchar(512) NOT NULL,
        [Title] nvarchar(255) NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_KitPages] PRIMARY KEY ([KitPageId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Offers] (
        [OfferId] int NOT NULL IDENTITY,
        [FulcoId] nvarchar(50) NOT NULL,
        [Title] nvarchar(255) NOT NULL,
        [Status] nvarchar(25) NOT NULL,
        [Description] nvarchar(max) NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Offers] PRIMARY KEY ([OfferId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [OldNews] (
        [OldNewId] int NOT NULL IDENTITY,
        [OldTableName] nvarchar(25) NOT NULL,
        [OldId] uniqueidentifier NOT NULL,
        [NewTableName] nvarchar(25) NOT NULL,
        [NewId] int NOT NULL,
        CONSTRAINT [PK_OldNews] PRIMARY KEY ([OldNewId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Places] (
        [PlaceId] int NOT NULL IDENTITY,
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Requests] (
        [RequestId] int NOT NULL IDENTITY,
        [WorkshopCode] nvarchar(50) NULL,
        [AppointmentId] bigint NULL,
        [AppointmentStartsOn] datetimeoffset NULL,
        [ReferenceSource] nvarchar(50) NULL,
        [ReferenceSourceOther] nvarchar(512) NULL,
        [Comments] nvarchar(max) NULL,
        [SubmittedOn] datetimeoffset NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Requests] PRIMARY KEY ([RequestId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
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
        [AddressHasChanged] bit NULL,
        [PlaceName] nvarchar(512) NULL,
        [PlaceAddress1] nvarchar(255) NULL,
        [PlaceAddress2] nvarchar(255) NULL,
        [PlaceCity] nvarchar(50) NULL,
        [PlaceState] nvarchar(5) NULL,
        [PlacePostalCode] nvarchar(25) NULL,
        [PlaceType] nvarchar(25) NULL,
        [PlaceTypeOther] nvarchar(50) NULL,
        [PlaceTaxIdentifier] nvarchar(50) NULL,
        [ClubsString] nvarchar(max) NULL,
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
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Unfinisheds] PRIMARY KEY ([UnfinishedId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Orders] (
        [OrderId] int NOT NULL IDENTITY,
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
        [Comments] nvarchar(max) NULL,
        [OrderedOn] datetimeoffset NOT NULL,
        [ArriveBy] date NULL,
        [ShippedOn] datetimeoffset NULL,
        [EmailedOn] datetimeoffset NULL,
        [InvoiceId] int NOT NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
        CONSTRAINT [FK_Orders_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Kits] (
        [KitId] int NOT NULL IDENTITY,
        [Status] nvarchar(25) NOT NULL,
        [Year] nvarchar(25) NOT NULL,
        [Season] int NOT NULL,
        [AgeLevel] nvarchar(25) NOT NULL,
        [Version] nvarchar(25) NULL,
        [Comments] nvarchar(max) NULL,
        [OfferId] int NOT NULL,
        [KitPageId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Kits] PRIMARY KEY ([KitId]),
        CONSTRAINT [FK_Kits_KitPages_KitPageId] FOREIGN KEY ([KitPageId]) REFERENCES [KitPages] ([KitPageId]),
        CONSTRAINT [FK_Kits_Offers_OfferId] FOREIGN KEY ([OfferId]) REFERENCES [Offers] ([OfferId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Persons] (
        [PersonId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(255) NULL,
        [LastName] nvarchar(255) NOT NULL,
        [Email] nvarchar(255) NULL,
        [TimeZone] nvarchar(50) NULL,
        [Phone] nvarchar(255) NULL,
        [JobTitle] nvarchar(25) NULL,
        [JobTitleOther] nvarchar(50) NULL,
        [WordPressId] int NULL,
        [PlaceId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Persons] PRIMARY KEY ([PersonId]),
        CONSTRAINT [FK_Persons_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Tickets] (
        [TicketId] int NOT NULL IDENTITY,
        [Status] nvarchar(25) NOT NULL,
        [PlaceId] int NOT NULL,
        [RequestId] int NOT NULL,
        [InvoiceId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Tickets] PRIMARY KEY ([TicketId]),
        CONSTRAINT [FK_Tickets_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]),
        CONSTRAINT [FK_Tickets_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId]),
        CONSTRAINT [FK_Tickets_Requests_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Requests] ([RequestId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [InvoicePersons] (
        [InvoicePersonId] int NOT NULL IDENTITY,
        [Ordinal] int NOT NULL DEFAULT 0,
        [PersonId] int NOT NULL,
        [InvoiceId] int NOT NULL,
        CONSTRAINT [PK_InvoicePersons] PRIMARY KEY ([InvoicePersonId]),
        CONSTRAINT [FK_InvoicePersons_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]) ON DELETE CASCADE,
        CONSTRAINT [FK_InvoicePersons_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Permissions] (
        [PermissionId] int NOT NULL IDENTITY,
        [ExpiresOn] datetimeoffset NULL,
        [PersonId] int NOT NULL,
        [KitPageId] int NOT NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([PermissionId]),
        CONSTRAINT [FK_Permissions_KitPages_KitPageId] FOREIGN KEY ([KitPageId]) REFERENCES [KitPages] ([KitPageId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Permissions_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [Clubs] (
        [ClubId] int NOT NULL IDENTITY,
        [Status] nvarchar(25) NOT NULL,
        [StartsOn] date NULL,
        [KitId] int NOT NULL,
        [PlaceId] int NOT NULL,
        [TicketId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Clubs] PRIMARY KEY ([ClubId]),
        CONSTRAINT [FK_Clubs_Kits_KitId] FOREIGN KEY ([KitId]) REFERENCES [Kits] ([KitId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Clubs_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId]) ON DELETE CASCADE,
        CONSTRAINT [FK_Clubs_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([TicketId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [TicketPersons] (
        [SalePersonId] int NOT NULL IDENTITY,
        [Ordinal] int NOT NULL,
        [PersonId] int NOT NULL,
        [TicketId] int NOT NULL,
        CONSTRAINT [PK_TicketPersons] PRIMARY KEY ([SalePersonId]),
        CONSTRAINT [FK_TicketPersons_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]) ON DELETE CASCADE,
        CONSTRAINT [FK_TicketPersons_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([TicketId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
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

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
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
        [TicketId] int NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        [ModifiedOn] datetimeoffset NULL,
        CONSTRAINT [PK_Notes] PRIMARY KEY ([NoteId]),
        CONSTRAINT [FK_Notes_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([ClubId]),
        CONSTRAINT [FK_Notes_Invoices_InvoiceId] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices] ([InvoiceId]),
        CONSTRAINT [FK_Notes_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]),
        CONSTRAINT [FK_Notes_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]),
        CONSTRAINT [FK_Notes_Places_PlaceId] FOREIGN KEY ([PlaceId]) REFERENCES [Places] ([PlaceId]),
        CONSTRAINT [FK_Notes_Tickets_TicketId] FOREIGN KEY ([TicketId]) REFERENCES [Tickets] ([TicketId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE TABLE [OrderClubs] (
        [OrderId] int NOT NULL,
        [ClubId] int NOT NULL,
        CONSTRAINT [PK_OrderClubs] PRIMARY KEY ([OrderId], [ClubId]),
        CONSTRAINT [FK_OrderClubs_Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [Clubs] ([ClubId]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderClubs_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([OrderId]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_ClubPersons_ClubId] ON [ClubPersons] ([ClubId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_ClubPersons_PersonId] ON [ClubPersons] ([PersonId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_KitId] ON [Clubs] ([KitId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_PlaceId] ON [Clubs] ([PlaceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Clubs_TicketId] ON [Clubs] ([TicketId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_InvoicePersons_InvoiceId] ON [InvoicePersons] ([InvoiceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_InvoicePersons_PersonId] ON [InvoicePersons] ([PersonId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_KitPages_Url] ON [KitPages] ([Url]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Kits_KitPageId] ON [Kits] ([KitPageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Kits_OfferId] ON [Kits] ([OfferId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Kits_Year_Season_AgeLevel_Version] ON [Kits] ([Year], [Season], [AgeLevel], [Version]) WHERE [Version] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_ClubId] ON [Notes] ([ClubId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_InvoiceId] ON [Notes] ([InvoiceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_OrderId] ON [Notes] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_PersonId] ON [Notes] ([PersonId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_PlaceId] ON [Notes] ([PlaceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Notes_TicketId] ON [Notes] ([TicketId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OldNews_NewTableName_NewId] ON [OldNews] ([NewTableName], [NewId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OldNews_OldTableName_OldId] ON [OldNews] ([OldTableName], [OldId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_OrderClubs_ClubId] ON [OrderClubs] ([ClubId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Orders_InvoiceId] ON [Orders] ([InvoiceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Permissions_KitPageId] ON [Permissions] ([KitPageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Permissions_PersonId] ON [Permissions] ([PersonId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Persons_Email] ON [Persons] ([Email]) WHERE [Email] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Persons_PlaceId] ON [Persons] ([PlaceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Places_ParentId] ON [Places] ([ParentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Shipments_OrderId] ON [Shipments] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_TicketPersons_PersonId] ON [TicketPersons] ([PersonId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_TicketPersons_TicketId] ON [TicketPersons] ([TicketId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Tickets_InvoiceId] ON [Tickets] ([InvoiceId]) WHERE [InvoiceId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE INDEX [IX_Tickets_PlaceId] ON [Tickets] ([PlaceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Tickets_RequestId] ON [Tickets] ([RequestId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Unfinisheds_Code] ON [Unfinisheds] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250327175245_InitialSetup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250327175245_InitialSetup', N'9.0.2');
END;

COMMIT;
GO

