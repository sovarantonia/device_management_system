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
    WHERE name = 'users'
      AND schema_id = SCHEMA_ID('dbo')
)
BEGIN
    CREATE TABLE [dbo].[users](
        [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [name] VARCHAR(50) NULL,
        [role] VARCHAR(50) NULL,
        [location] VARCHAR(50) NULL,
        [email] VARCHAR(50) NOT NULL,
        [password] VARCHAR(255) NULL,

        CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([id] ASC),

        CONSTRAINT [uc_email] UNIQUE NONCLUSTERED ([email] ASC)
    );
END
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
        [user_id] UNIQUEIDENTIFIER NULL,
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
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id]);
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
    CREATE TABLE [dbo].[users](
        [id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
        [name] VARCHAR(50) NULL,
        [role] VARCHAR(50) NULL,
        [location] VARCHAR(50) NULL,
        [email] VARCHAR(50) NOT NULL,
        [password] VARCHAR(255) NULL,

        CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED ([id] ASC),

        CONSTRAINT [uc_email] UNIQUE NONCLUSTERED ([email] ASC)
    );
END
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
        [user_id] UNIQUEIDENTIFIER NULL,
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
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[users]([id]);
END
GO