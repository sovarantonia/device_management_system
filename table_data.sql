USE device_management;

--users
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[AspNetUsers]
    WHERE [id] = '11111111-1111-1111-1111-111111111111'
)
BEGIN
    INSERT INTO [dbo].[AspNetUsers] (
        [Id],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount],
        [Name],
        [Role],
        [Location]
    )
    VALUES (
        '11111111-1111-1111-1111-111111111111',
        'alice@example.com',
        'ALICE@EXAMPLE.COM',
        'alice@example.com',
        'ALICE@EXAMPLE.COM',
        1,
        NULL,
        NEWID(),
        NEWID(),
        NULL,
        0,
        0,
        NULL,
        1,
        0,
        'Alice Johnson',
        'Admin',
        'Cluj'
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[AspNetUsers]
    WHERE [id] = '22222222-2222-2222-2222-222222222222'
)
BEGIN
    INSERT INTO [dbo].[AspNetUsers] (
        [Id],
        [UserName],
        [NormalizedUserName],
        [Email],
        [NormalizedEmail],
        [EmailConfirmed],
        [PasswordHash],
        [SecurityStamp],
        [ConcurrencyStamp],
        [PhoneNumber],
        [PhoneNumberConfirmed],
        [TwoFactorEnabled],
        [LockoutEnd],
        [LockoutEnabled],
        [AccessFailedCount],
        [Name],
        [Role],
        [Location]
    )
    VALUES (
        '22222222-2222-2222-2222-222222222222',
        'bob@example.com',
        'BOB@EXAMPLE.COM',
        'bob@example.com',
        'BOB@EXAMPLE.COM',
        1,
        NULL,
        NEWID(),
        NEWID(),
        NULL,
        0,
        0,
        NULL,
        1,
        0,
        'Bob Smith',
        'User',
        'Bucharest'
    );
END
GO

--devices
GO


IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = '11111111-aaaa-bbbb-cccc-111111111111'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        '11111111-aaaa-bbbb-cccc-111111111111',
        'iPhone 15',
        'Apple',
        0,
        'iOS',
        '17',
        'A16 Bionic',
        6.00,
        'Latest iPhone',
        '11111111-1111-1111-1111-111111111111'
    );
END
GO

-- Phone
IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = '22222222-aaaa-bbbb-cccc-222222222222'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        '22222222-aaaa-bbbb-cccc-222222222222',
        'Galaxy S24',
        'Samsung',
        0,
        'Android',
        '14',
        'Exynos 2400',
        8.00,
        'Flagship Samsung phone',
        '22222222-2222-2222-2222-222222222222'
    );
END
GO

-- Phone
IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = '33333333-aaaa-bbbb-cccc-333333333333'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        '33333333-aaaa-bbbb-cccc-333333333333',
        'Pixel 8 Pro',
        'Google',
        0,
        'Android',
        '14',
        'Tensor G3',
        12.00,
        'Google flagship phone',
        null
    );
END
GO

-- Tablet
IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = '44444444-aaaa-bbbb-cccc-444444444444'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        '44444444-aaaa-bbbb-cccc-444444444444',
        'iPad Pro',
        'Apple',
        1,
        'iPadOS',
        '17',
        'M2',
        8.00,
        'High-end tablet',
        '11111111-1111-1111-1111-111111111111'
    );
END
GO

-- Tablet
IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = '55555555-aaaa-bbbb-cccc-555555555555'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        '55555555-aaaa-bbbb-cccc-555555555555',
        'Galaxy Tab S9',
        'Samsung',
        1,
        'Android',
        '13',
        'Snapdragon 8 Gen 2',
        8.00,
        'Samsung premium tablet',
        '22222222-2222-2222-2222-222222222222'
    );
END
GO

-- Tablet
IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = '66666666-aaaa-bbbb-cccc-666666666666'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        '66666666-aaaa-bbbb-cccc-666666666666',
        'Lenovo Tab M10',
        'Lenovo',
        1,
        'Android',
        '12',
        'MediaTek Helio P22T',
        4.00,
        'Budget tablet',
        null
    );
END
GO