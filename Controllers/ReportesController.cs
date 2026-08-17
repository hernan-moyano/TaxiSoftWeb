using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;
using TaxiSoftWeb.ViewModels;

namespace TaxiSoftWeb.Controllers
{
    public class ReportesController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public ReportesController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Reportes/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var primerDiaMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var ultimoDiaMes = primerDiaMes.AddMonths(1).AddDays(-1);

            var ingresosMes = await _context.RegistrosDeCajas
                .Where(r => r.IdCaja == 1
                            && r.FechaRegisCaja.HasValue
                            && r.FechaRegisCaja.Value.Date >= primerDiaMes.Date
                            && r.FechaRegisCaja.Value.Date <= ultimoDiaMes.Date)
                .SumAsync(r => (decimal?)r.Importe) ?? 0;

            var egresosMes = await _context.RegistrosDeCajas
                .Where(r => r.IdCaja == 2
                            && r.FechaRegisCaja.HasValue
                            && r.FechaRegisCaja.Value.Date >= primerDiaMes.Date
                            && r.FechaRegisCaja.Value.Date <= ultimoDiaMes.Date)
                .SumAsync(r => (decimal?)r.Importe) ?? 0;

            var gastosOperativosMes = await _context.GastosOperaciones
                .Where(g => g.FechaGasto.HasValue
                            && g.FechaGasto.Value.Date >= primerDiaMes.Date
                            && g.FechaGasto.Value.Date <= ultimoDiaMes.Date)
                .SumAsync(g => (decimal?)g.Importe) ?? 0;

            var flotaActiva = await _context.Vehiculos.CountAsync(v => v.Activo == true);
            var choferesActivos = await _context.Conductores.CountAsync(c => c.Activo == true);
            var vencimientosProximos = await _context.AlertasAutomaticas.CountAsync(a => a.Activa == true);

            var modelo = new ReporteDashboard
            {
                FlotaActiva = flotaActiva,
                ChoferesActivos = choferesActivos,
                IngresosMes = ingresosMes,
                EgresosMes = egresosMes + gastosOperativosMes,
                MargenMes = ingresosMes - egresosMes - gastosOperativosMes,
                VencimientosProximos = vencimientosProximos,
                TopVehiculosRentables = await CalcularMargenVehiculos(primerDiaMes, ultimoDiaMes, 5),
                TopVehiculosConGastos = await CalcularTopGastos(primerDiaMes, ultimoDiaMes, 5)
            };

            return View(modelo);
        }

        // GET: Reportes/MargenVehiculo
        public async Task<IActionResult> MargenVehiculo(DateTime? desde, DateTime? hasta)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var margenes = await CalcularMargenVehiculos(fechaInicio, fechaFin, null);

            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");

            return View(margenes);
        }

        // GET: Reportes/MargenChofer
        public async Task<IActionResult> MargenChofer(DateTime? desde, DateTime? hasta)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var registros = await _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation)
                .Include(r => r.IdVehiculoNavigation)
                .Where(r => r.FechaRegisCaja.HasValue
                            && r.FechaRegisCaja.Value.Date >= fechaInicio.Date
                            && r.FechaRegisCaja.Value.Date <= fechaFin.Date)
                .ToListAsync();

            var reporte = registros
                .GroupBy(r => r.Cuil)
                .Select(g =>
                {
                    var chofer = g.FirstOrDefault()?.CuilNavigation;
                    var totalIngresos = g.Where(r => r.IdCaja == 1).Sum(r => r.Importe);
                    var totalEgresos = g.Where(r => r.IdCaja == 2).Sum(r => r.Importe);
                    var neto = totalIngresos - totalEgresos;
                    decimal? alquiler = null;
                    decimal? comision = null;
                    if (chofer?.TipoAlquiler == "fijo")
                    {
                        alquiler = chofer.MontoAlquiler;
                    }
                    else if (chofer?.TipoAlquiler == "porcentaje" && chofer.PorcentajeRecaudacion.HasValue)
                    {
                        comision = neto * chofer.PorcentajeRecaudacion / 100m;
                    }

                    return new ReporteMargenChofer
                    {
                        Cuil = g.Key,
                        NombreCompleto = chofer != null ? $"{chofer.Apellido} {chofer.Nombre}".Trim() : g.Key,
                        Patente = g.FirstOrDefault()?.IdVehiculoNavigation?.Patente,
                        TotalIngresos = totalIngresos,
                        TotalEgresos = totalEgresos,
                        MargenNeto = neto,
                        TipoAlquiler = chofer?.TipoAlquiler,
                        Alquiler = alquiler,
                        Comision = comision
                    };
                })
                .OrderByDescending(c => c.MargenNeto)
                .ToList();

            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");

            return View(reporte);
        }

        // GET: Reportes/ComparativaEficiencia
        public async Task<IActionResult> ComparativaEficiencia(DateTime? desde, DateTime? hasta)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var margenes = await CalcularMargenVehiculos(fechaInicio, fechaFin, null);

            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");

            return View(margenes);
        }

        private async Task<List<ReporteMargenVehiculo>> CalcularMargenVehiculos(DateTime desde, DateTime hasta, int? top)
        {
            var registros = await _context.RegistrosDeCajas
                .Include(r => r.IdVehiculoNavigation)
                .Include(r => r.CuilNavigation)
                .Where(r => r.FechaRegisCaja.HasValue
                            && r.FechaRegisCaja.Value.Date >= desde.Date
                            && r.FechaRegisCaja.Value.Date <= hasta.Date)
                .ToListAsync();

            var gastos = await _context.GastosOperaciones
                .Where(g => g.FechaGasto.HasValue
                            && g.FechaGasto.Value.Date >= desde.Date
                            && g.FechaGasto.Value.Date <= hasta.Date)
                .ToListAsync();

            var resultado = registros
                .GroupBy(r => r.IdVehiculo)
                .Select(g =>
                {
                    var vehiculo = g.FirstOrDefault()?.IdVehiculoNavigation;
                    var chofer = g.FirstOrDefault()?.CuilNavigation;
                    var totalIngresos = g.Where(r => r.IdCaja == 1).Sum(r => r.Importe);
                    var totalEgresos = g.Where(r => r.IdCaja == 2).Sum(r => r.Importe);
                    var gastosVehiculo = gastos.Where(x => x.IdVehiculo == g.Key).Sum(x => x.Importe ?? 0);
                    var neto = totalIngresos - totalEgresos - gastosVehiculo;
                    return new ReporteMargenVehiculo
                    {
                        IdVehiculo = g.Key,
                        Patente = vehiculo?.Patente,
                        Marca = vehiculo?.Marca,
                        Modelo = vehiculo?.Modelo,
                        Chofer = chofer != null ? $"{chofer.Apellido} {chofer.Nombre}".Trim() : null,
                        TotalIngresos = totalIngresos,
                        TotalEgresos = totalEgresos + gastosVehiculo,
                        MargenNeto = neto,
                        PorcentajeMargen = totalIngresos != 0 ? Math.Round(neto / totalIngresos * 100m, 2) : 0
                    };
                })
                .OrderByDescending(r => r.MargenNeto)
                .ToList();

            return top.HasValue ? resultado.Take(top.Value).ToList() : resultado;
        }

        private async Task<List<ReporteMargenVehiculo>> CalcularTopGastos(DateTime desde, DateTime hasta, int top)
        {
            var gastos = await _context.GastosOperaciones
                .Include(g => g.IdVehiculoNavigation)
                .Where(g => g.FechaGasto.HasValue
                            && g.FechaGasto.Value.Date >= desde.Date
                            && g.FechaGasto.Value.Date <= hasta.Date)
                .ToListAsync();

            return gastos
                .GroupBy(g => g.IdVehiculo)
                .Select(g => new ReporteMargenVehiculo
                {
                    IdVehiculo = g.Key,
                    Patente = g.FirstOrDefault()?.IdVehiculoNavigation?.Patente,
                    Marca = g.FirstOrDefault()?.IdVehiculoNavigation?.Marca,
                    Modelo = g.FirstOrDefault()?.IdVehiculoNavigation?.Modelo,
                    TotalEgresos = g.Sum(x => x.Importe ?? 0),
                    TotalIngresos = 0,
                    MargenNeto = 0,
                    PorcentajeMargen = 0
                })
                .OrderByDescending(r => r.TotalEgresos)
                .Take(top)
                .ToList();
        }
    }
}