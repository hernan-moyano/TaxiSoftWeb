using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaxiSoftWeb.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//referencia a cadena de conexion
builder.Services.AddDbContext<TaxisoftDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));


var app = builder.Build();
//Configuración para el uso de decimales con punto
app.UseRequestLocalization("es-US");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
