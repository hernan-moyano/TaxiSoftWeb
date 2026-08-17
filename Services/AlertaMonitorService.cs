using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaxiSoftWeb.Models;

namespace TaxiSoftWeb.Services;

public class AlertaMonitorService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AlertaMonitorService> _logger;
    private readonly TimeSpan _periodo = TimeSpan.FromHours(6);

    public AlertaMonitorService(IServiceScopeFactory scopeFactory, ILogger<AlertaMonitorService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EjecutarRevision(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar la revision de alertas de vencimientos.");
            }
            await Task.Delay(_periodo, stoppingToken);
        }
    }

    private async Task EjecutarRevision(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TaxisoftDbContext>();
        var hoy = DateTime.Today;
        const int diasAntelacionPorDefecto = 30;

        var alertasAutomaticas = await context.AlertasAutomaticas
            .Where(a => a.Activa == true)
            .ToListAsync(ct);

        var nuevasAlertas = new List<AlertaAutomatica>();

        // ITVs
        var itvs = await context.Itvs
            .Include(i => i.IdVehiculoNavigation)
            .Where(i => i.VigenciaHasta.HasValue && i.VigenciaHasta.Value >= hoy)
            .ToListAsync(ct);
        foreach (var itv in itvs)
        {
            var vto = itv.VigenciaHasta!.Value;
            if (!ExisteAlerta(alertasAutomaticas, "itv", itv.IdItv))
            {
                nuevasAlertas.Add(CrearAlerta("itv", itv.IdItv, vto, itv.IdVehiculo, null,
                    $"Vencimiento de I.T.V. del móvil {itv.IdVehiculoNavigation?.Patente}"));
            }
        }

        // Seguros
        var seguros = await context.Seguros
            .Include(s => s.IdVehiculoNavigation)
            .Where(s => s.VigenciaHasta.HasValue && s.VigenciaHasta.Value >= hoy)
            .ToListAsync(ct);
        foreach (var seguro in seguros)
        {
            var vto = seguro.VigenciaHasta!.Value;
            if (!ExisteAlerta(alertasAutomaticas, "seguro", seguro.IdSeguro))
            {
                nuevasAlertas.Add(CrearAlerta("seguro", seguro.IdSeguro, vto, seguro.IdVehiculo, null,
                    $"Vencimiento del seguro del móvil {seguro.IdVehiculoNavigation?.Patente}"));
            }
        }

        // Impuestos
        var impuestos = await context.Impuestos
            .Include(i => i.IdVehiculoNavigation)
            .Where(i => i.FechaVto.HasValue && i.FechaVto.Value >= hoy)
            .ToListAsync(ct);
        foreach (var impuesto in impuestos)
        {
            var vto = impuesto.FechaVto!.Value;
            if (!ExisteAlerta(alertasAutomaticas, "impuesto", impuesto.IdImpuesto))
            {
                nuevasAlertas.Add(CrearAlerta("impuesto", impuesto.IdImpuesto, vto, impuesto.IdVehiculo, null,
                    $"Vencimiento de impuesto del móvil {impuesto.IdVehiculoNavigation?.Patente}: {impuesto.Descripcion}"));
            }
        }

        // Carnets de conductores
        var carnets = await context.Carnets
            .Where(c => c.VtoCarnet.HasValue && c.VtoCarnet.Value >= hoy)
            .ToListAsync(ct);
        var conductores = await context.Conductores.ToListAsync(ct);
        foreach (var carnet in carnets)
        {
            var vto = carnet.VtoCarnet!.Value;
            if (!ExisteAlerta(alertasAutomaticas, "carnet", carnet.IdCarnet))
            {
                var chofer = conductores.FirstOrDefault(c => c.IdCarnet == carnet.IdCarnet);
                nuevasAlertas.Add(CrearAlerta("carnet", carnet.IdCarnet, vto, null, chofer?.Cuil,
                    $"Vencimiento del carnet N° {carnet.NroCarnet} del chofer {chofer?.Apellido} {chofer?.Nombre}".Trim()));
            }
        }

        // Calibraciones de taximetro
        var calibraciones = await context.CalibracionesTaximetro
            .Include(c => c.IdVehiculoNavigation)
            .Where(c => c.ProximaCalibracion.HasValue && c.ProximaCalibracion.Value >= hoy)
            .ToListAsync(ct);
        foreach (var calibracion in calibraciones)
        {
            var vto = calibracion.ProximaCalibracion!.Value;
            if (!ExisteAlerta(alertasAutomaticas, "calibracion", calibracion.IdCalibracion))
            {
                nuevasAlertas.Add(CrearAlerta("calibracion", calibracion.IdCalibracion, vto, calibracion.IdVehiculo, null,
                    $"Próxima calibración de taxímetro del móvil {calibracion.IdVehiculoNavigation?.Patente}"));
            }
        }

        // Mantenimientos: próximos services
        var mantenimientos = await context.Mantenimientos
            .Include(m => m.IdVehiculoNavigation)
            .Where(m => m.FechaMant.HasValue && m.FechaMant.Value >= hoy)
            .ToListAsync(ct);
        foreach (var mant in mantenimientos)
        {
            var vto = mant.FechaMant!.Value;
            if (!ExisteAlerta(alertasAutomaticas, "service", mant.IdMant))
            {
                nuevasAlertas.Add(CrearAlerta("service", mant.IdMant, vto, mant.IdVehiculo, null,
                    $"Service técnico del móvil {mant.IdVehiculoNavigation?.Patente}: {mant.Descripcion}"));
            }
        }

        if (nuevasAlertas.Count > 0)
        {
            context.AlertasAutomaticas.AddRange(nuevasAlertas);
            await context.SaveChangesAsync(ct);
            _logger.LogInformation("Se generaron {Cantidad} alertas automaticas de vencimientos.", nuevasAlertas.Count);
        }
    }

    private static bool ExisteAlerta(List<AlertaAutomatica> existentes, string tipo, int idOrigen)
    {
        return existentes.Any(a => a.TipoDocumento == tipo && a.IdRegistroOrigen == idOrigen);
    }

    private static AlertaAutomatica CrearAlerta(string tipo, int idOrigen, DateTime vto, int? idVehiculo, string? cuil, string descripcion)
    {
        return new AlertaAutomatica
        {
            TipoDocumento = tipo,
            IdRegistroOrigen = idOrigen,
            FechaVencimiento = vto,
            IdVehiculo = idVehiculo,
            Cuil = cuil,
            Descripcion = descripcion,
            DiasAntelacion = 30,
            Activa = true
        };
    }
}