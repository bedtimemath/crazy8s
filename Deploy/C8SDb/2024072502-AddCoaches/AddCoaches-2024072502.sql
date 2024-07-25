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
    WHERE [MigrationId] = N'20240724180745_InitialSetup'
)
BEGIN
    CREATE TABLE [Coaches] (
        [CoachId] int NOT NULL IDENTITY,
        [Name] nvarchar(512) NOT NULL,
        [Email] nvarchar(255) NOT NULL,
        [Phone] nvarchar(25) NOT NULL,
        [Status] nvarchar(25) NOT NULL,
        [Image] nvarchar(512) NOT NULL,
        [TagLine] nvarchar(512) NULL,
        [Bio] nvarchar(max) NULL,
        [AuthId] nvarchar(255) NULL,
        [CreatedOn] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Coaches] PRIMARY KEY ([CoachId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240724180745_InitialSetup'
)
BEGIN
    CREATE TABLE [Organizations] (
        [OrganizationId] int NOT NULL IDENTITY,
        [OldSystemCompanyId] uniqueidentifier NULL,
        [OldSystemOrganizationId] uniqueidentifier NOT NULL,
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
    WHERE [MigrationId] = N'20240724180745_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Coaches_Email] ON [Coaches] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240724180745_InitialSetup'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Organizations_OldSystemCompanyId] ON [Organizations] ([OldSystemCompanyId]) WHERE [OldSystemCompanyId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240724180745_InitialSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Organizations_OldSystemOrganizationId] ON [Organizations] ([OldSystemOrganizationId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240724180745_InitialSetup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240724180745_InitialSetup', N'8.0.7');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DROP INDEX [IX_Organizations_OldSystemCompanyId] ON [Organizations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DROP INDEX [IX_Organizations_OldSystemOrganizationId] ON [Organizations];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'AuthId');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Coaches] DROP COLUMN [AuthId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'Bio');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [Coaches] DROP COLUMN [Bio];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'Image');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Coaches] DROP COLUMN [Image];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'Name');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Coaches] DROP COLUMN [Name];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'Status');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [Coaches] DROP COLUMN [Status];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'TagLine');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [Coaches] DROP COLUMN [TagLine];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Organizations]') AND [c].[name] = N'OldSystemOrganizationId');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Organizations] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [Organizations] ALTER COLUMN [OldSystemOrganizationId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Coaches]') AND [c].[name] = N'Phone');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Coaches] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [Coaches] ALTER COLUMN [Phone] nvarchar(25) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [FirstName] nvarchar(255) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [LastName] nvarchar(255) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [OldSystemCoachId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [OldSystemCompanyId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [OldSystemNotes] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [OldSystemOrganizationId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [OldSystemUserId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [PhoneExt] nvarchar(25) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    ALTER TABLE [Coaches] ADD [TimeZone] nvarchar(50) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Organizations_OldSystemOrganizationId] ON [Organizations] ([OldSystemOrganizationId]) WHERE [OldSystemOrganizationId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Coaches_OldSystemCoachId] ON [Coaches] ([OldSystemCoachId]) WHERE [OldSystemCoachId] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240725022844_AddCoaches'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240725022844_AddCoaches', N'8.0.7');
END;
GO

COMMIT;
GO

