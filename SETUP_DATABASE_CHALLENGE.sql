-- ============================================
-- STOCK MANAGEMENT DATABASE CHALLENGE
-- ============================================
-- ============================================

USE master;
GO

-- 1. Crear base de datos si no existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'StockManagementDB')
BEGIN
    CREATE DATABASE StockManagementDB;
    PRINT 'Base de datos StockManagementDB creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos StockManagementDB ya existe.';
END
GO

USE StockManagementDB;
GO

-- ============================================
-- 2. CREACIÓN DE TABLAS
-- ============================================

-- Tabla Users
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[Users] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Username] NVARCHAR(100) NOT NULL,
        [PasswordHash] NVARCHAR(255) NOT NULL,
        [RowVersion] ROWVERSION NOT NULL,
        CONSTRAINT UQ_Users_Username UNIQUE (Username) 
    );
    PRINT 'Tabla Users creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'Tabla Users existente. Verificando estructura...';
    IF COL_LENGTH('dbo.Users', 'RowVersion') IS NULL
    BEGIN
        ALTER TABLE dbo.Users ADD RowVersion ROWVERSION NOT NULL;
        PRINT 'Columna RowVersion agregada a dbo.Users.';
    END
END
GO

-- Tabla Products
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type = 'U')
BEGIN
    CREATE TABLE [dbo].[Products] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Price] DECIMAL(18,2) NOT NULL CHECK (Price > 0),
        [LoadDate] DATETIME2 NOT NULL DEFAULT GETDATE(),
        [Category] NVARCHAR(50) NOT NULL CHECK (Category IN ('PRODUNO', 'PRODDOS')),
        [RowVersion] ROWVERSION NOT NULL
    );
    PRINT 'Tabla Products creada exitosamente.';
END
ELSE
BEGIN
    PRINT 'Tabla Products existente. Verificando estructura...';
    IF COL_LENGTH('dbo.Products', 'RowVersion') IS NULL
    BEGIN
        ALTER TABLE dbo.Products ADD RowVersion ROWVERSION NOT NULL;
        PRINT 'Columna RowVersion agregada a dbo.Products.';
    END
END
GO

-- ============================================
-- 3. ÍNDICES 
-- ============================================

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_Category_Price')
BEGIN
    CREATE NONCLUSTERED INDEX IX_Products_Category_Price
    ON [dbo].[Products] ([Category], [Price]);
    PRINT 'Índice compuesto IX_Products_Category_Price creado.';
END
GO

-- ============================================
-- 4. DATOS INICIALES 
-- ============================================

-- Insertar Usuario Admin con Hash de API (BCrypt Cost 11)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE Username = 'admin')
BEGIN
    -- Se utiliza el hash generado por tu API para garantizar compatibilidad total
    DECLARE @OfficialHash NVARCHAR(255) = '$2a$11$2S8szRq069iaUmegtin8derMWi4YVIm0V1YhwwKA6B6u.MPT3pRaW'; 

    INSERT INTO [dbo].[Users] ([Username], [PasswordHash]) 
    VALUES ('admin', @OfficialHash); 
    
    PRINT 'Usuario admin (seed) insertado con hash oficial.';
END
GO

-- Insertar Productos
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products])
BEGIN
    INSERT INTO [dbo].[Products] ([Price], [LoadDate], [Category]) VALUES
    (10.00, '2019-10-21', 'PRODDOS'),
    (60.00, '2019-10-21', 'PRODUNO'),
    (5.00, '2019-10-22', 'PRODDOS'),
    (5.00, '2019-10-29', 'PRODUNO'),
    (15.00, '2019-09-11', 'PRODDOS'),
    (25.50, '2024-01-15', 'PRODUNO'),
    (30.00, '2024-01-16', 'PRODDOS'),
    (45.75, '2024-01-17', 'PRODUNO'),
    (20.25, '2024-01-18', 'PRODDOS'),
    (100.00, '2024-01-19', 'PRODUNO'),
    (50.50, '2024-01-20', 'PRODDOS'),
    (35.00, '2024-01-21', 'PRODUNO'),
    (12.75, '2024-01-22', 'PRODDOS'),
    (80.00, '2024-01-23', 'PRODUNO'),
    (40.00, '2024-01-24', 'PRODDOS');

    PRINT 'Productos de ejemplo insertados.';
END
ELSE
BEGIN
    PRINT 'Ya existen productos en la base de datos.';
END
GO

PRINT '';
PRINT 'Usuario de prueba: admin';
PRINT 'Password de prueba: admin123';
PRINT '============================================';
GO