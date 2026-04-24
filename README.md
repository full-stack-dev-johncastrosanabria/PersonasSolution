# Personas API

RESTful API para gestión de personas y direcciones desarrollada con **VB.NET**, **ASP.NET Core 8.0**, **Entity Framework Core** y **SQL Server**.

## 🚀 Características

- ✅ **Arquitectura en Capas** (Clean Architecture)
  - API Layer (Controllers, Middleware)
  - Business Layer (Services, Validators, DTOs)
  - Data Layer (EF Core, Repositories)
- ✅ **Entity Framework Core** con SQL Server
- ✅ **Repository Pattern** para abstracción de datos
- ✅ **FluentValidation** para validación de datos
- ✅ **Global Exception Handling** con middleware personalizado
- ✅ **Swagger/OpenAPI** para documentación interactiva
- ✅ **Health Checks** para monitoreo
- ✅ **CORS** configurado para integración con frontend
- ✅ **Paginación y búsqueda** en listados
- ✅ **Operaciones CRUD** completas

## 📋 Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, o superior)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)

## 🏗️ Estructura del Proyecto

```
PersonasSolution/
├── Personas.Api/              # Capa de presentación (API REST)
│   ├── Controllers/           # Controladores de API
│   ├── Middleware/            # Middleware personalizado
│   ├── Extensions/            # Extensiones de configuración
│   └── Models/                # Modelos de respuesta
├── Personas.Business/         # Capa de negocio
│   ├── Services/              # Lógica de negocio
│   ├── Validators/            # Validadores FluentValidation
│   └── DTOs/                  # Data Transfer Objects
├── Personas.Repository/       # Capa de acceso a datos
│   └── Repositories/          # Implementación de repositorios
└── Personas.EF/               # Capa de Entity Framework
    ├── Entities/              # Entidades del dominio
    └── DbContext/             # Contexto de base de datos
```

## 🔧 Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/full-stack-dev-johncastrosanabria/PersonasSolution.git
cd PersonasSolution
```

### 2. Configurar la cadena de conexión

Editar `Personas.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PersonasDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Crear la base de datos

```bash
cd Personas.Api
dotnet ef database update
```

### 4. Ejecutar la aplicación

```bash
dotnet run
```

La API estará disponible en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `http://localhost:5000` (raíz)

## 📚 Endpoints de la API

### Personas

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/personas` | Obtener lista paginada de personas |
| GET | `/api/personas/{id}` | Obtener persona por ID |
| POST | `/api/personas` | Crear nueva persona |
| PUT | `/api/personas/{id}` | Actualizar persona |
| PATCH | `/api/personas/{id}/activo` | Activar/desactivar persona |
| POST | `/api/personas/{id}/direcciones` | Agregar dirección a persona |

### Direcciones

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| PUT | `/api/direcciones/{id}` | Actualizar dirección |
| DELETE | `/api/direcciones/{id}` | Eliminar dirección |

### Health Check

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/health` | Estado de salud de la API |

## 📝 Ejemplos de Uso

### Crear una persona

```bash
POST /api/personas
Content-Type: application/json

{
  "identificacion": "123456789",
  "nombre": "Juan",
  "apellidos": "Pérez García",
  "fechaNacimiento": "1990-05-15",
  "correo": "juan.perez@example.com",
  "telefono": "88887777",
  "activo": true
}
```

### Obtener personas con paginación

```bash
GET /api/personas?search=juan&page=1&pageSize=10
```

### Agregar dirección a una persona

```bash
POST /api/personas/1/direcciones
Content-Type: application/json

{
  "provincia": "San José",
  "canton": "Central",
  "distrito": "Carmen",
  "direccionExacta": "Avenida Central, Calle 5",
  "esPrincipal": true
}
```

## 🛡️ Validaciones

La API utiliza **FluentValidation** para validar los datos de entrada:

### Persona
- ✅ Identificación: requerida, máximo 50 caracteres
- ✅ Nombre: requerido, máximo 100 caracteres
- ✅ Apellidos: requeridos, máximo 150 caracteres
- ✅ Fecha de nacimiento: requerida, debe ser anterior a hoy
- ✅ Correo: requerido, formato válido, máximo 150 caracteres
- ✅ Teléfono: opcional, máximo 50 caracteres

### Dirección
- ✅ Provincia: requerida, máximo 100 caracteres
- ✅ Cantón: requerido, máximo 100 caracteres
- ✅ Distrito: requerido, máximo 100 caracteres
- ✅ Dirección exacta: requerida, máximo 250 caracteres

## 🔍 Manejo de Errores

La API implementa un middleware global de manejo de excepciones que retorna respuestas consistentes:

```json
{
  "statusCode": 404,
  "message": "Persona no encontrada.",
  "timestamp": "2026-04-24T10:30:00Z"
}
```

## 🧪 Testing

```bash
# Ejecutar pruebas unitarias (cuando estén disponibles)
dotnet test
```

## 📦 Dependencias Principales

- **Microsoft.AspNetCore.App** (8.0)
- **Microsoft.EntityFrameworkCore.SqlServer** (8.0)
- **FluentValidation.AspNetCore** (11.3.0)
- **Swashbuckle.AspNetCore** (6.5.0)
- **AspNetCore.HealthChecks.SqlServer** (8.0.0)

## 🤝 Contribuir

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT.

## 👤 Autor

**John Castro Sanabria**
- GitHub: [@full-stack-dev-johncastrosanabria](https://github.com/full-stack-dev-johncastrosanabria)
- Email: castrosanabriajohn2@gmail.com

## 🙏 Agradecimientos

- ASP.NET Core Team
- Entity Framework Core Team
- FluentValidation Community

---

⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub!
