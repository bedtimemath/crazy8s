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
    WHERE [MigrationId] = N'20240722174157_TestSetup'
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
        CONSTRAINT [PK_Coaches] PRIMARY KEY ([CoachId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240722174157_TestSetup'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Coaches_Email] ON [Coaches] ([Email]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240722174157_TestSetup'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240722174157_TestSetup', N'8.0.7');
END;
GO

COMMIT;
GO

