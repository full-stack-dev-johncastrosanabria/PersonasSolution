-- =============================================
-- Script: Seed Data for PersonasDb
-- Description: Inserts sample data for testing
-- Author: John Castro Sanabria
-- Date: 2026-04-24
-- =============================================

USE PersonasDb;
GO

-- =============================================
-- Insert Sample Personas
-- =============================================

PRINT 'Inserting sample personas...';
GO

INSERT INTO dbo.persona (identificacion, nombre, apellidos, fecha_nacimiento, correo, telefono, activo)
VALUES 
    ('123456789', 'Juan', 'Pérez García', '1990-05-15', 'juan.perez@example.com', '88887777', 1),
    ('987654321', 'María', 'González López', '1985-08-22', 'maria.gonzalez@example.com', '88889999', 1),
    ('456789123', 'Carlos', 'Rodríguez Mora', '1992-03-10', 'carlos.rodriguez@example.com', '88881111', 1),
    ('789123456', 'Ana', 'Martínez Solís', '1988-11-30', 'ana.martinez@example.com', '88882222', 1),
    ('321654987', 'Luis', 'Hernández Castro', '1995-07-18', 'luis.hernandez@example.com', NULL, 0);
GO

-- =============================================
-- Insert Sample Direcciones
-- =============================================

PRINT 'Inserting sample addresses...';
GO

DECLARE @persona1_id INT = (SELECT persona_id FROM dbo.persona WHERE identificacion = '123456789');
DECLARE @persona2_id INT = (SELECT persona_id FROM dbo.persona WHERE identificacion = '987654321');
DECLARE @persona3_id INT = (SELECT persona_id FROM dbo.persona WHERE identificacion = '456789123');
DECLARE @persona4_id INT = (SELECT persona_id FROM dbo.persona WHERE identificacion = '789123456');

-- Direcciones para Juan Pérez
INSERT INTO dbo.direccion (persona_id, provincia, canton, distrito, direccion_exacta, es_principal)
VALUES 
    (@persona1_id, 'San José', 'Central', 'Carmen', 'Avenida Central, Calle 5, Casa 123', 1),
    (@persona1_id, 'San José', 'Escazú', 'San Rafael', 'Residencial Los Laureles, Casa 45', 0);

-- Dirección para María González
INSERT INTO dbo.direccion (persona_id, provincia, canton, distrito, direccion_exacta, es_principal)
VALUES 
    (@persona2_id, 'Alajuela', 'Central', 'Alajuela', 'Barrio San José, 200 metros norte de la iglesia', 1);

-- Direcciones para Carlos Rodríguez
INSERT INTO dbo.direccion (persona_id, provincia, canton, distrito, direccion_exacta, es_principal)
VALUES 
    (@persona3_id, 'Heredia', 'Central', 'Heredia', 'Residencial El Bosque, Apartamento 301', 1),
    (@persona3_id, 'Heredia', 'Santo Domingo', 'Santo Domingo', 'Condominio Las Flores, Casa 12', 0),
    (@persona3_id, 'San José', 'Curridabat', 'Granadilla', 'Oficina Central, Edificio Torre Norte', 0);

-- Dirección para Ana Martínez
INSERT INTO dbo.direccion (persona_id, provincia, canton, distrito, direccion_exacta, es_principal)
VALUES 
    (@persona4_id, 'Cartago', 'Central', 'Oriental', 'Frente al parque central, Casa esquinera', 1);

GO

-- =============================================
-- Verification Queries
-- =============================================

PRINT 'Verifying inserted data...';
GO

-- Count records
SELECT 
    'Personas' AS TableName,
    COUNT(*) AS RecordCount
FROM dbo.persona
UNION ALL
SELECT 
    'Direcciones' AS TableName,
    COUNT(*) AS RecordCount
FROM dbo.direccion;
GO

-- Show all personas with their addresses
SELECT 
    p.persona_id,
    p.identificacion,
    p.nombre + ' ' + p.apellidos AS NombreCompleto,
    p.correo,
    p.telefono,
    p.activo,
    p.fecha_registro,
    COUNT(d.direccion_id) AS CantidadDirecciones
FROM dbo.persona p
LEFT JOIN dbo.direccion d ON p.persona_id = d.persona_id
GROUP BY 
    p.persona_id,
    p.identificacion,
    p.nombre,
    p.apellidos,
    p.correo,
    p.telefono,
    p.activo,
    p.fecha_registro
ORDER BY p.persona_id;
GO

-- Show primary addresses
SELECT 
    p.identificacion,
    p.nombre + ' ' + p.apellidos AS NombreCompleto,
    d.provincia,
    d.canton,
    d.distrito,
    d.direccion_exacta
FROM dbo.persona p
INNER JOIN dbo.direccion d ON p.persona_id = d.persona_id
WHERE d.es_principal = 1
ORDER BY p.persona_id;
GO

PRINT 'Sample data inserted successfully!';
GO
