USE device_management;

--users
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[users]
    WHERE [id] = '11111111-1111-1111-1111-111111111111'
)
BEGIN
    INSERT INTO [dbo].[users] (
        [id], [name], [role], [location], [email], [password]
    )
    VALUES (
        '11111111-1111-1111-1111-111111111111',
        'Alice Johnson',
        'Admin',
        'Cluj',
        'alice@example.com',
        '$2a$12$Gti3zVFbWup/77sR3sCk6uQXCtCUEfyVnHjfQcLyn9u6hDQiMSMPC' --test123
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[users]
    WHERE [id] = '22222222-2222-2222-2222-222222222222'
)
BEGIN
    INSERT INTO [dbo].[users] (
        [id], [name], [role], [location], [email], [password]
    )
    VALUES (
        '22222222-2222-2222-2222-222222222222',
        'Bob Smith',
        'User',
        'Bucharest',
        'bob@example.com',
        '$2a$12$3URV9aP5CZmdiAFbyvusluqL6LtytH7pueoLy6QCdZDsA207lueMK' --test123
    );
END
GO

--devices
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
        'ThinkPad X1',
        'Lenovo',
        1,
        'Windows',
        '11',
        'Intel Core i7',
        16.00,
        'Work laptop',
        '11111111-1111-1111-1111-111111111111'
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb',
        'MacBook Pro',
        'Apple',
        1,
        'macOS',
        'Sonoma',
        'M2',
        16.00,
        'Personal laptop',
        '22222222-2222-2222-2222-222222222222'
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM [dbo].[devices]
    WHERE [id] = 'cccccccc-cccc-cccc-cccc-cccccccccccc'
)
BEGIN
    INSERT INTO [dbo].[devices] (
        [id], [name], [manufacturer], [device_type],
        [OS], [OS_version], [processor],
        [ram_amount], [description], [user_id]
    )
    VALUES (
        'cccccccc-cccc-cccc-cccc-cccccccccccc',
        'Galaxy S23',
        'Samsung',
        2,
        'Android',
        '14',
        'Snapdragon 8 Gen 2',
        8.00,
        'Mobile device',
        '11111111-1111-1111-1111-111111111111'
    );
END
GO