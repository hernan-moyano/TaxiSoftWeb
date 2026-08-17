# TaxiSoft

Sistema web de administración para flotas de taxis. Aplicación interna pensada para redes locales; toda la interfaz y el código están en español.

> **Instalación en la PC del cliente:** ver [`INSTALACION.md`](INSTALACION.md).
> **Convenciones para desarrollo/agentes:** ver [`AGENTS.md`](AGENTS.md).

## Funcionalidades

| Módulo | Descripción |
|---|---|
| **Alertas personales** | Alertas definidas a mano por el usuario, con estado, frecuencia y vencimiento. |
| **Vencimientos automáticos** | Un servicio en segundo plano (`AlertaMonitorService`) revisa cada 6 h ITVs, seguros, impuestos, carnets, calibraciones y services, y genera alertas con 30 días de antelación. |
| **Calibraciones** | Registro de calibraciones de taxímetro y próxima calibración. |
| **Vehículos** | Móviles, seguros, I.T.V., multas, impuestos y mantenimientos por vehículo. |
| **Gastos de operación** | Gastos por móvil y tipo de gasto (combustible, lavado, neumáticos, reparación, repuesto, service, imprevisto), con resumen por móvil. |
| **Conductores** | Choferes, domicilios, carnets, turnos y puestos. Alquiler fijo o por porcentaje de recaudación. |
| **Registros / caja** | Cierres de caja (ingresos/egresos), tipos de caja y liquidación por chofer. |
| **Reportes** | Dashboard mensual, margen por móvil, margen por chofer y comparativa de eficiencia (filtrables por rango de fechas). |

## Tecnologías

- **Backend:** ASP.NET Core MVC (net10.0), C#.
- **ORM:** EF Core 10 + Npgsql — PostgreSQL, enfoque *database-first* (DbContext scaffolded, no editar a mano).
- **Frontend:** Razor views + AdminLTE 3 (vendored en `wwwroot/Theme-Adm/`) y DataTables con exportación. Sin recursos externos (todo offline).
- **Despliegue:** Docker Compose (`app` + `db` PostgreSQL 17 + `backup` automático).

## Estructura del proyecto

```
Controllers/    Controladores MVC (CRUD scaffold + Reportes)
Models/         Entidades y TaxisoftDbContext (DB-first, nombres singularizados del schema)
Migrations/     Migraciones EF Core (crean tablas y datos semilla al arrancar)
Services/       Servicios en segundo plano (AlertaMonitorService)
ViewModels/     Modelos para reportes, dashboard y notificaciones
Components/     View components (campanita de notificaciones)
Views/          Vistas Razor (scaffold @Html.DisplayFor) + layout AdminLTE
wwwroot/        Assets vendored (Theme-Adm/, lib/) — no refactorizar
Program.cs      Configuración, migración automática al arranque, /healthz
```

## Requisitos para desarrollo

- Windows con **.NET 10 SDK**.
- PostgreSQL local con la base `taxisoft` creada.

La app no arranca sin la base de datos. `appsettings.json` no lleva credenciales; en desarrollo se cargan desde `appsettings.Local.json` (gitignored, se lee en `Program.cs`):

```json
{
  "ConnectionStrings": {
    "conexion": "Host=localhost;Port=5432;Database=taxisoft;Username=taxisoft;Password=TU_PASSWORD"
  }
}
```

## Puesta en marcha (desarrollo)

```powershell
dotnet restore
dotnet run
```

- HTTP: `http://localhost:5156` · HTTPS: `https://localhost:7156` (perfil Development).
- Al arrancar, `Program.cs` aplica `db.Database.Migrate()`: crea tablas y datos iniciales.
- La conexión local usa el puerto **5432** (`appsettings.Local.json`); el compose de Docker usa **5434** desde `.env`.

## Despliegue con Docker (PC del cliente)

La app se distribuye como contenedores: la PC del cliente solo necesita Docker Desktop. La base se crea y migra sola en el primer arranque; los datos persisten en `./data/postgres` y los backups en `./backups`.

```powershell
docker compose up -d --build     # http://localhost:8080
```

- `instalar.ps1` — instalación primera vez en la PC del cliente.
- `actualizar.ps1` — actualizar a una nueva versión sin tocar datos.
- `backup-manual.ps1` — backup manual de la BD.
- `restore.ps1` — restaurar un backup (reemplaza datos actuales).

Detalles y solución de problemas en [`INSTALACION.md`](INSTALACION.md).

## Backups

- **Automático:** diario 02:00 en `./backups`, conservando 7 diarios y 4 semanales (configurable en `.env` → `BACKUP_SCHEDULE`, `BACKUP_KEEP_DAYS`, `BACKUP_KEEP_WEEKS`).
- **Manual:** `.\backup-manual.ps1`.
- **Restaurar:** `.\restore.ps1` (último backup, o `-Archivo backups\manual_XXXX.dump`).

## Seguridad

- App interna por **HTTP** en la red local; no exponer el puerto 8080 a Internet.
- El puerto de PostgreSQL (5434/5433) solo se expone en `127.0.0.1`, para administración.
- Credenciales solo en `.env` y `appsettings.Local.json`, ambos gitignored.
- Cabeceras básicas: `nosniff`, `X-Frame-Options: DENY`, `Referrer-Policy`.

## Notas de desarrollo

- **Verificación:** solo `dotnet build TaxiSoftWeb.csproj` (no hay proyecto de tests ni linter configurado). Auditoría de paquetes: `dotnet list TaxiSoftWeb.csproj package --vulnerable --include-transitive`.
- Los paquetes de solo-desarrollo (RuntimeCompilation, EF Tools) están condicionados a configuración `Debug`; el publish Release no los incluye.
- Al cambiar el schema, regenerar el DbContext con `dotnet ef dbcontext scaffold` y crear la migración correspondiente.