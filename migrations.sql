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
    WHERE [MigrationId] = N'20240923184034_addproducttables'
)
BEGIN
    CREATE TABLE [ProductBrands] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_ProductBrands] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240923184034_addproducttables'
)
BEGIN
    CREATE TABLE [ProductTypes] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_ProductTypes] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240923184034_addproducttables'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [PictureUrl] nvarchar(max) NOT NULL,
        [TypeId] int NOT NULL,
        [BrandId] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Products_ProductBrands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [ProductBrands] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Products_ProductTypes_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [ProductTypes] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240923184034_addproducttables'
)
BEGIN
    CREATE INDEX [IX_Products_BrandId] ON [Products] ([BrandId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240923184034_addproducttables'
)
BEGIN
    CREATE INDEX [IX_Products_TypeId] ON [Products] ([TypeId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240923184034_addproducttables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240923184034_addproducttables', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241004202025_AddDelivryMethodTable'
)
BEGIN
    CREATE TABLE [DeliveryMethods] (
        [Id] int NOT NULL IDENTITY,
        [ShortName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [DeliveryTime] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_DeliveryMethods] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241004202025_AddDelivryMethodTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241004202025_AddDelivryMethodTable', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011120832_AddOrderTables'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] uniqueidentifier NOT NULL,
        [BuyerEmail] nvarchar(max) NOT NULL,
        [OrderDate] datetimeoffset NOT NULL,
        [ShippingAddress_FirstName] nvarchar(max) NOT NULL,
        [ShippingAddress_LastName] nvarchar(max) NOT NULL,
        [ShippingAddress_Street] nvarchar(max) NOT NULL,
        [ShippingAddress_City] nvarchar(max) NOT NULL,
        [ShippingAddress_State] nvarchar(max) NOT NULL,
        [ShippingAddress_PostalCode] nvarchar(max) NOT NULL,
        [DeliveryMethodId] int NULL,
        [Status] int NOT NULL,
        [OrderPaymentStatus] int NOT NULL,
        [Subtotal] decimal(18,2) NOT NULL,
        [BasketId] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_DeliveryMethods_DeliveryMethodId] FOREIGN KEY ([DeliveryMethodId]) REFERENCES [DeliveryMethods] ([Id]) ON DELETE SET NULL
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011120832_AddOrderTables'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] uniqueidentifier NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [Quantity] int NOT NULL,
        [ProductItem_ProductId] int NOT NULL,
        [ProductItem_ProductName] nvarchar(max) NOT NULL,
        [ProductItem_PictureUrl] nvarchar(max) NOT NULL,
        [OrderId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011120832_AddOrderTables'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011120832_AddOrderTables'
)
BEGIN
    CREATE INDEX [IX_Orders_DeliveryMethodId] ON [Orders] ([DeliveryMethodId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241011120832_AddOrderTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241011120832_AddOrderTables', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241012194332_AddpaymentintentidToOrderTable'
)
BEGIN
    ALTER TABLE [Orders] ADD [PaymentIntentId] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241012194332_AddpaymentintentidToOrderTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241012194332_AddpaymentintentidToOrderTable', N'8.0.8');
END;
GO

COMMIT;
GO

