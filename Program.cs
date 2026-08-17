using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using TaxiSoftWeb.Models;
using TaxiSoftWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración local opcional (gitignored): credenciales de desarrollo de la máquina
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllersWithViews();

#if DEBUG
if (builder.Environment.IsDevelopment())
{
    // Compilación de vistas en caliente (solo desarrollo)
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}
#endif

// Referencia a cadena de conexión
builder.Services.AddDbContext<TaxisoftDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("conexion"),
            npgsql => npgsql.EnableRetryOnFailure(5)));

builder.Services.AddHealthChecks();

builder.Services.AddHostedService<AlertaMonitorService>();

var app = builder.Build();

// Configuración para el uso de decimales con punto
app.UseRequestLocalization("es-US");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHttpsRedirection();
}
else
{
    // Aplicación interna en red local: acceso por HTTP, sin redirección a HTTPS
    app.UseExceptionHandler("/Home/Error");
}

// Cabeceras de seguridad básicas
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    await next();
});

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHealthChecks("/healthz");

// Aplica migraciones pendientes al arrancar (crea tablas y datos iniciales en despliegues nuevos)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaxisoftDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "No se pudo aplicar la migración de la base de datos. Verifique que el contenedor de PostgreSQL esté disponible.");
        throw;
    }
}

app.Run();
