IF DB_ID('device_management') IS NULL
BEGIN
    CREATE DATABASE device_management;
END
GO

USE device_management;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'AspNetUsers'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE [dbo].[AspNetUsers](
        [Id] NVARCHAR(450) NOT NULL,

        [UserName] NVARCHAR(256) NULL,
        [NormalizedUserName] NVARCHAR(256) NULL,

        [Email] NVARCHAR(256) NULL,
        [NormalizedEmail] NVARCHAR(256) NULL,
        [EmailConfirmed] BIT NOT NULL DEFAULT 0,

        [PasswordHash] NVARCHAR(MAX) NULL,
        [SecurityStamp] NVARCHAR(MAX) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,

        [PhoneNumber] NVARCHAR(MAX) NULL,
        [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0,

        [TwoFactorEnabled] BIT NOT NULL DEFAULT 0,
        [LockoutEnd] DATETIMEOFFSET NULL,
        [LockoutEnabled] BIT NOT NULL DEFAULT 1,
        [AccessFailedCount] INT NOT NULL DEFAULT 0,

        [Name] VARCHAR(50) NULL,
        [Role] VARCHAR(50) NULL,
        [Location] VARCHAR(50) NULL,

        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UserNameIndex'
)
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers] ([NormalizedUserName])
    WHERE [NormalizedUserName] IS NOT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'EmailIndex'
)
BEGIN
    CREATE UNIQUE INDEX [EmailIndex]
    ON [dbo].[AspNetUsers] ([NormalizedEmail])
    WHERE [NormalizedEmail] IS NOT NULL;
END
GO

--devices

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'devices'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE [dbo].[devices] (
        [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [name] VARCHAR(50) NULL,
        [manufacturer] VARCHAR(50) NULL,
        [device_type] INT NULL,
        [OS] VARCHAR(50) NULL,
        [OS_version] VARCHAR(50) NULL,
        [processor] VARCHAR(50) NULL,
        [ram_amount] DECIMAL(10,2) NULL,
        [description] VARCHAR(50) NULL,
        [user_id] NVARCHAR(450) NULL,
        CONSTRAINT [PK_devices] PRIMARY KEY CLUSTERED ([id] ASC)
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_devices_users'
)
BEGIN
    ALTER TABLE [dbo].[devices]
    ADD CONSTRAINT [FK_devices_users]
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[AspNetUsers]([id]);
END
GO

--Database for integration tests

IF DB_ID('device_management_test') IS NULL
BEGIN
    CREATE DATABASE device_management_test;
END
GO

USE device_management_test;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'users'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE [dbo].[AspNetUsers](
        [Id] NVARCHAR(450) NOT NULL,

        [UserName] NVARCHAR(256) NULL,
        [NormalizedUserName] NVARCHAR(256) NULL,

        [Email] NVARCHAR(256) NULL,
        [NormalizedEmail] NVARCHAR(256) NULL,
        [EmailConfirmed] BIT NOT NULL DEFAULT 0,

        [PasswordHash] NVARCHAR(MAX) NULL,
        [SecurityStamp] NVARCHAR(MAX) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,

        [PhoneNumber] NVARCHAR(MAX) NULL,
        [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0,

        [TwoFactorEnabled] BIT NOT NULL DEFAULT 0,
        [LockoutEnd] DATETIMEOFFSET NULL,
        [LockoutEnabled] BIT NOT NULL DEFAULT 1,
        [AccessFailedCount] INT NOT NULL DEFAULT 0,

        [Name] VARCHAR(50) NULL,
        [Role] VARCHAR(50) NULL,
        [Location] VARCHAR(50) NULL,

        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'UserNameIndex'
)
BEGIN
    CREATE UNIQUE INDEX [UserNameIndex]
    ON [dbo].[AspNetUsers] ([NormalizedUserName])
    WHERE [NormalizedUserName] IS NOT NULL;
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE name = 'EmailIndex'
)
BEGIN
    CREATE UNIQUE INDEX [EmailIndex]
    ON [dbo].[AspNetUsers] ([NormalizedEmail])
    WHERE [NormalizedEmail] IS NOT NULL;
END
GO

GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.tables
    WHERE name = 'devices'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE [dbo].[devices] (
        [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [name] VARCHAR(50) NULL,
        [manufacturer] VARCHAR(50) NULL,
        [device_type] INT NULL,
        [OS] VARCHAR(50) NULL,
        [OS_version] VARCHAR(50) NULL,
        [processor] VARCHAR(50) NULL,
        [ram_amount] DECIMAL(10,2) NULL,
        [description] VARCHAR(50) NULL,
        [user_id] NVARCHAR(450) NULL,
        CONSTRAINT [PK_devices] PRIMARY KEY CLUSTERED ([id] ASC)
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_devices_users'
)
BEGIN
    ALTER TABLE [dbo].[devices]
    ADD CONSTRAINT [FK_devices_users]
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[AspNetUsers]([id]);
END
GO