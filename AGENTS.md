# AGENTS.md

ASP.NET Core MVC (net10.0) taxi fleet management app. EF Core 10 + PostgreSQL (Npgsql), database-first. All code/UI is in Spanish. Runs on Windows with a local PostgreSQL server. Deployed to client PCs via Docker (`docker-compose.yml`).

## Git state gotcha (important)

The project was moved from a `TaxiSoftWeb/` subfolder up to the repo root, but the git index was never updated. `git status` shows the whole `TaxiSoftWeb/wwwroot/...` tree as deleted plus all current source files as untracked. This is expected. Do not try to "repair" the index, and use explicit paths when staging.

## Commands

- Build: `dotnet build TaxiSoftWeb.csproj` (only nullable CS8602 warnings from views; 0 errors)
- Run: `dotnet run` (Development profile: https://localhost:7156, http://localhost:5156)
- No test project, no linter/typecheck config exists. Build is the only verification.
- SDK note: machine has .NET 10 SDK; the project targets net10.0.
- NuGet audit: `dotnet list TaxiSoftWeb.csproj package --vulnerable --include-transitive`
- Docker: `docker compose up -d --build` (app en http://localhost:8080, `db` postgres 17 con volumen en `./data/postgres`, `backup` automático diario en `./backups`)
- Instalación en la PC del cliente: `instalar.ps1` (no tocar los datos de `./data` ni `./backups` en actualizaciones)

## Configuración y secretos

- `appsettings.json` **no contiene credenciales** (`conexion` vacía). En producción el compose inyecta `ConnectionStrings__conexion` por variable de entorno (credenciales en `.env`, gitignored).
- Para desarrollo local: las credenciales van en `appsettings.Local.json` (gitignored, ya creado localmente). `Program.cs` carga ese archivo opcional.
- `Program.cs` ejecuta `db.Database.Migrate()` al arrancar: en Docker crea las tablas + datos semilla automáticamente.
- Paquetes solo-de-desarrollo (`Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`, `Microsoft.EntityFrameworkCore.Tools`) están condicionados a `'$(Configuration)' == 'Debug'`; el publish Release sale sin ellas. Si se necesita volver a generar CRUD con `dotnet aspnet-codegenerator`, re-agregar temporalmente `Microsoft.VisualStudio.Web.CodeGeneration.Design` (fue removido del csproj porque filtraba DLLs de dev al output).

## Database

- Conexión de desarrollo apunta a PostgreSQL local `Host=localhost;Port=5432;Database=taxisoft` (ver `appsettings.Local.json`). App cannot start without that DB.
- `Models/TaxisoftDbContext.cs` is a fully scaffolded DB-first DbContext (column mappings, FK constraint names, PK names). Don't hand-edit the mappings; regenerate with EF scaffolding (`dotnet ef dbcontext scaffold`) when the schema changes.

## Conventions to preserve

- Spanish everywhere: controllers, models, views, UI text, user-facing messages.
- Entity classes keep the awkward singular scaffolded names (`Conductore`, `EstadosActividade`, `TiposDeOperacione`). Don't "fix" pluralization.
- CRUD controllers are standard scaffold style: `[Bind(...)]` on POST actions, `SelectList` populated into `ViewData["X"]` for dropdowns, `Index`/`Details` use `.Include(...)` for navigations.
- Delete flow: `DeleteConfirmed` wraps `SaveChangesAsync` in try/catch and sets `TempData["Mensaje"]` (Spanish message) when FK constraints block deletion. Every Index view renders an alert from `TempData["Mensaje"]`. Keep this pattern.
- `Program.cs` calls `app.UseRequestLocalization("es-US")` so decimal inputs parse with `.` (dot), not comma. Don't remove it.
- `Views/Shared/_Layout.cshtml` auto-initializes any `.table` with DataTables (export buttons, Spanish lang). Index views give tables `id="XxxTable"`.
- Layout is AdminLTE vendored in `wwwroot/Theme-Adm/`; layout partials live in `Views/Shared/_ThemeAdm/` (`_TopBar`, `_SideBar`, `_Footer`). Add new menu entries in `_SideBar.cshtml`.
- Views are scaffolded `@Html.DisplayFor` style. Razor RuntimeCompilation is enabled only in Debug (views hot-reload in Development).
- `_Layout.cshtml` no referencia recursos externos (todo local/offline).

## Don't touch

- `wwwroot/Theme-Adm/` and `wwwroot/lib/` are vendored third-party assets (AdminLTE theme, DataTables, jquery) committed to the repo. Do not refactor or restructure them.
