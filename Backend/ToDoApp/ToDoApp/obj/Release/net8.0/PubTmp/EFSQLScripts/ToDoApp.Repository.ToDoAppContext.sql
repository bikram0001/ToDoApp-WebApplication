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
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE TABLE [Status] (
        [Id] int NOT NULL IDENTITY,
        [Status_Name] varchar(20) NOT NULL,
        CONSTRAINT [PK__Status__3214EC07170C10AE] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE TABLE [TaskInfo] (
        [Id] int NOT NULL IDENTITY,
        [Title] varchar(100) NOT NULL,
        [Description] varchar(100) NULL,
        CONSTRAINT [PK__TaskInfo__3214EC07656AE9A5] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] int NOT NULL IDENTITY,
        [UserName] varchar(50) NOT NULL,
        [Password] varchar(50) NOT NULL,
        CONSTRAINT [PK__Users__3214EC07A608EC06] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE TABLE [UserTask] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [TaskId] int NOT NULL,
        [CreatedOn] datetime NULL DEFAULT ((getdate())),
        [CompletedOn] datetime NULL DEFAULT ((NULL)),
        [StatusId] int NULL,
        [Flag] bit NULL DEFAULT CAST(0 AS bit),
        CONSTRAINT [PK__UserTask__3214EC07E66BF108] PRIMARY KEY ([Id]),
        CONSTRAINT [FK__UserTask__Flag__5535A963] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]),
        CONSTRAINT [FK__UserTask__Status__571DF1D5] FOREIGN KEY ([StatusId]) REFERENCES [Status] ([Id]),
        CONSTRAINT [FK__UserTask__TaskId__5629CD9C] FOREIGN KEY ([TaskId]) REFERENCES [TaskInfo] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE INDEX [IX_UserTask_StatusId] ON [UserTask] ([StatusId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE INDEX [IX_UserTask_TaskId] ON [UserTask] ([TaskId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    CREATE INDEX [IX_UserTask_UserId] ON [UserTask] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240701101312_initialdb'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240701101312_initialdb', N'8.0.6');
END;
GO

COMMIT;
GO

