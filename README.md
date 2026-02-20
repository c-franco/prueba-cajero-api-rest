# Cajero API REST

API REST de cajero automático desarrollada como prueba técnica. Permite realizar operaciones bancarias básicas como consulta de saldo, depósitos y retiros a través de endpoints RESTful.

---

## Tecnologías utilizadas

- **.NET / ASP.NET Core 8**
- **C#**
- **Entity Framework Core**
- **Swagger**
- **Patrón CQRS** con **MediatR**
- **Autenticación y autorización con JWT**
- Tests con **xUnit**, **FluentAssertions** y **Moq**

---

## Arquitectura

El proyecto sigue los principios de **Clean Architecture**, separando las responsabilidades en cuatro capas independientes:

```
prueba-cajero-api-rest/
├── prueba-cajero-api-rest.API/             # Capa de presentación (Controllers, Middleware)
├── prueba-cajero-api-rest.Application/     # Capa de aplicación (Casos de uso, DTOs, Interfaces)
├── prueba-cajero-api-rest.Domain/          # Capa de dominio (Entidades, Reglas de negocio)
└── prueba-cajero-api-rest.Infrastructure/  # Capa de infraestructura (Repositorios, BD, etc.)
```

## Endpoints

- `POST /api/auth` — Iniciar sesión y recibir un token JWT.

- `GET /api/bank` — Obtener todas las cuentas bancarias.
- `POST /api/bank/getByAccountNumber` — Obtener una cuenta bancaria por número de cuenta.

- `POST /api/bank/deposit` — Depositar dinero en una cuenta.
- `POST /api/bank/withdraw` — Retirar dinero de una cuenta.

---

## Cómo ejecutar el proyecto

1. **Clonar el repositorio:**

   ```bash
   git clone https://github.com/c-franco/prueba-cajero-api-rest.git
   cd prueba-cajero-api-rest.API
   ```

2. **Ejecutar la API**

   - Abre la solución en **Visual Studio 2022+** o usa la terminal.
   - Ejecuta:
     ```bash
     dotnet restore
     dotnet build
     dotnet run --project src/prueba-cajero-api-rest.API
     ```

3. **Swagger UI:**

   - Navega a: `https://localhost:7251/swagger`

4. **Autenticación:**

   - Usa `/api/auth` con las siguientes credenciales para obtener un token JWT.

   ```bash
   username = "admin"
   password = "123456"
   ```

   - Usa el botón "Authorize" en Swagger para autenticarte.

---

## Notas

- Todos los secretos y configuraciones de JWT están almacenados en `appsettings.json` con fines de demostración.
- La base de datos está configurada en memoria; los datos se resetean en cada reinicio de la aplicación.