-- =============================================
-- Script: Create PersonasDb Database
-- Description: Creates the database schema for Personas API
-- Author: John Castro Sanabria
-- Date: 2026-04-24
-- =============================================

-- Create Database
CREATE DATABASE PersonasDb;
GO

USE PersonasDb;
GO

-- =============================================
-- Table: persona
-- Description: Stores person information
-- =============================================
CREATE TABLE dbo.persona
(
    persona_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    identificacion NVARCHAR(50) NOT NULL,  -- Unique identification number
    nombre NVARCHAR(100) NOT NULL,         -- First name
    apellidos NVARCHAR(150) NOT NULL,      -- Last name(s)
    fecha_nacimiento DATE NOT NULL,        -- Date of birth
    correo NVARCHAR(150) NOT NULL,         -- Email address
    telefono NVARCHAR(50) NULL,            -- Phone number (optional)
    activo BIT NOT NULL CONSTRAINT DF_persona_activo DEFAULT (1),  -- Active status
    fecha_registro DATETIME2 NOT NULL CONSTRAINT DF_persona_fecha_registro DEFAULT (SYSUTCDATETIME())  -- Registration timestamp
);
GO

-- Unique index on identification number (improves performance and ensures uniqueness)
CREATE UNIQUE INDEX UX_persona_identificacion
ON dbo.persona(identificacion);
GO

-- =============================================
-- Table: direccion
-- Description: Stores addresses for persons
-- =============================================
CREATE TABLE dbo.direccion
(
    direccion_id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    persona_id INT NOT NULL,               -- Foreign key to persona
    provincia NVARCHAR(100) NOT NULL,      -- Province
    canton NVARCHAR(100) NOT NULL,         -- Canton
    distrito NVARCHAR(100) NOT NULL,       -- District
    direccion_exacta NVARCHAR(250) NOT NULL,  -- Exact address
    es_principal BIT NOT NULL CONSTRAINT DF_direccion_principal DEFAULT (0),  -- Is primary address
    CONSTRAINT FK_direccion_persona
        FOREIGN KEY (persona_id) REFERENCES dbo.persona(persona_id)
);
GO

-- Unique filtered index: Only one primary address per person
CREATE UNIQUE INDEX UX_direccion_principal_por_persona
ON dbo.direccion(persona_id)
WHERE es_principal = 1;
GO

-- Index to improve queries by persona_id
CREATE INDEX IX_direccion_persona_id
ON dbo.direccion(persona_id);
GO

-- =============================================
-- Verification Queries
-- =============================================

-- Verify tables were created
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'dbo'
ORDER BY TABLE_NAME;
GO

-- Verify indexes
SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    i.is_unique AS IsUnique,
    i.has_filter AS HasFilter,
    i.filter_definition AS FilterDefinition
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE t.schema_id = SCHEMA_ID('dbo')
    AND t.name IN ('persona', 'direccion')
ORDER BY t.name, i.name;
GO

-- Verify foreign keys
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
WHERE fk.schema_id = SCHEMA_ID('dbo');
GO

PRINT 'Database PersonasDb created successfully!';
GO
