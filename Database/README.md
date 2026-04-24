# Database Schema - PersonasDb

## 📊 Esquema de Base de Datos

Este proyecto utiliza un enfoque **Database-First**, donde la base de datos fue diseñada y creada antes del desarrollo de la aplicación.

## 🗂️ Tablas

### Tabla: `persona`

Almacena la información principal de las personas.

| Columna | Tipo | Restricciones | Descripción |
|---------|------|---------------|-------------|
| `persona_id` | INT | PRIMARY KEY, IDENTITY(1,1) | Identificador único |
| `identificacion` | NVARCHAR(50) | NOT NULL, UNIQUE | Número de identificación (cédula) |
| `nombre` | NVARCHAR(100) | NOT NULL | Nombre(s) |
| `apellidos` | NVARCHAR(150) | NOT NULL | Apellido(s) |
| `fecha_nacimiento` | DATE | NOT NULL | Fecha de nacimiento |
| `correo` | NVARCHAR(150) | NOT NULL | Correo electrónico |
| `telefono` | NVARCHAR(50) | NULL | Número de teléfono |
| `activo` | BIT | NOT NULL, DEFAULT(1) | Estado activo/inactivo |
| `fecha_registro` | DATETIME2 | NOT NULL, DEFAULT(SYSUTCDATETIME()) | Fecha de registro |

**Índices:**
- `UX_persona_identificacion` - Índice único en `identificacion` (mejora rendimiento y garantiza unicidad)

---

### Tabla: `direccion`

Almacena las direcciones asociadas a cada persona.

| Columna | Tipo | Restricciones | Descripción |
|---------|------|---------------|-------------|
| `direccion_id` | INT | PRIMARY KEY, IDENTITY(1,1) | Identificador único |
| `persona_id` | INT | NOT NULL, FOREIGN KEY | Referencia a persona |
| `provincia` | NVARCHAR(100) | NOT NULL | Provincia |
| `canton` | NVARCHAR(100) | NOT NULL | Cantón |
| `distrito` | NVARCHAR(100) | NOT NULL | Distrito |
| `direccion_exacta` | NVARCHAR(250) | NOT NULL | Dirección exacta |
| `es_principal` | BIT | NOT NULL, DEFAULT(0) | Indica si es dirección principal |

**Relaciones:**
- `FK_direccion_persona` - Relación con tabla `persona` (persona_id)

**Índices:**
- `UX_direccion_principal_por_persona` - Índice único filtrado en `persona_id` WHERE `es_principal = 1` (garantiza solo una dirección principal por persona)
- `IX_direccion_persona_id` - Índice en `persona_id` (mejora rendimiento en consultas)

---

## 🔗 Diagrama de Relaciones

```
┌─────────────────────────────────┐
│          persona                │
├─────────────────────────────────┤
│ PK  persona_id (INT)            │
│ UQ  identificacion (NVARCHAR)   │
│     nombre (NVARCHAR)           │
│     apellidos (NVARCHAR)        │
│     fecha_nacimiento (DATE)     │
│     correo (NVARCHAR)           │
│     telefono (NVARCHAR)         │
│     activo (BIT)                │
│     fecha_registro (DATETIME2)  │
└─────────────────────────────────┘
                │
                │ 1:N
                │
                ▼
┌─────────────────────────────────┐
│         direccion               │
├─────────────────────────────────┤
│ PK  direccion_id (INT)          │
│ FK  persona_id (INT)            │
│     provincia (NVARCHAR)        │
│     canton (NVARCHAR)           │
│     distrito (NVARCHAR)         │
│     direccion_exacta (NVARCHAR) │
│     es_principal (BIT)          │
└─────────────────────────────────┘
```

**Relación:** Una persona puede tener múltiples direcciones (1:N), pero solo una puede ser principal.

---

## 📝 Scripts Disponibles

### 1. `CreateDatabase.sql`
Script principal para crear la base de datos y todas las tablas con sus índices y restricciones.

**Uso:**
```sql
-- En SSMS o cualquier cliente SQL
-- Ejecutar el script completo
```

**Incluye:**
- Creación de base de datos `PersonasDb`
- Creación de tabla `persona` con índices
- Creación de tabla `direccion` con índices y relaciones
- Queries de verificación

### 2. `SeedData.sql`
Script para insertar datos de prueba en la base de datos.

**Uso:**
```sql
-- Ejecutar después de CreateDatabase.sql
USE PersonasDb;
-- Ejecutar el script completo
```

**Incluye:**
- 5 personas de ejemplo
- 7 direcciones de ejemplo
- Queries de verificación

---

## 🔍 Características Especiales

### 1. Índice Único Filtrado
```sql
CREATE UNIQUE INDEX UX_direccion_principal_por_persona
ON dbo.direccion(persona_id)
WHERE es_principal = 1;
```
Este índice garantiza que cada persona tenga **solo una dirección principal**, mejorando la integridad de datos sin necesidad de triggers.

### 2. Valores por Defecto
- `persona.activo` → `1` (activo por defecto)
- `persona.fecha_registro` → `SYSUTCDATETIME()` (timestamp UTC automático)
- `direccion.es_principal` → `0` (no principal por defecto)

### 3. Optimización de Consultas
Los índices están diseñados para optimizar las consultas más comunes:
- Búsqueda por identificación (único)
- Consultas de direcciones por persona
- Filtrado de dirección principal

---

## 🔧 Mantenimiento

### Verificar Integridad
```sql
-- Verificar que no hay personas sin dirección principal activa
SELECT p.persona_id, p.nombre, p.apellidos
FROM dbo.persona p
WHERE p.activo = 1
  AND NOT EXISTS (
    SELECT 1 FROM dbo.direccion d 
    WHERE d.persona_id = p.persona_id 
      AND d.es_principal = 1
  );
```

### Estadísticas
```sql
-- Contar registros
SELECT 
    (SELECT COUNT(*) FROM dbo.persona) AS TotalPersonas,
    (SELECT COUNT(*) FROM dbo.persona WHERE activo = 1) AS PersonasActivas,
    (SELECT COUNT(*) FROM dbo.direccion) AS TotalDirecciones,
    (SELECT COUNT(DISTINCT persona_id) FROM dbo.direccion) AS PersonasConDireccion;
```

### Backup Recomendado
```sql
-- Backup completo
BACKUP DATABASE PersonasDb
TO DISK = 'C:\Backups\PersonasDb_Full.bak'
WITH FORMAT, INIT, NAME = 'PersonasDb-Full Database Backup';
```

---

## 📚 Notas de Diseño

1. **Database-First Approach**: La base de datos fue diseñada primero, luego se crearon las entidades de Entity Framework para mapear las tablas existentes.

2. **Normalización**: El esquema está en 3FN (Tercera Forma Normal) para evitar redundancia de datos.

3. **Integridad Referencial**: Se utilizan foreign keys para mantener la integridad entre tablas.

4. **Performance**: Los índices están estratégicamente colocados en columnas frecuentemente consultadas.

5. **Escalabilidad**: El diseño permite agregar fácilmente nuevas tablas relacionadas (ej: `telefono`, `email_adicional`, etc.).

---

## 🔗 Referencias

- [SQL Server Best Practices](https://docs.microsoft.com/sql/relational-databases/best-practices)
- [Index Design Guidelines](https://docs.microsoft.com/sql/relational-databases/sql-server-index-design-guide)
- [Filtered Indexes](https://docs.microsoft.com/sql/relational-databases/indexes/create-filtered-indexes)
