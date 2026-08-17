using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;
using TaxiSoftWeb.ViewModels;

namespace TaxiSoftWeb.Controllers
{
    public class GastosOperacionesController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public GastosOperacionesController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: GastosOperaciones
        public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var taxisoftDbContext = _context.GastosOperaciones
                .Include(g => g.CuilNavigation)
                .Include(g => g.IdTipoGastoNavigation)
                .Include(g => g.IdVehiculoNavigation)
                .Where(g => g.FechaGasto >= fechaInicio && g.FechaGasto <= fechaFin);

            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");

            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: GastosOperaciones/Resumen
        public async Task<IActionResult> Resumen(DateTime? desde, DateTime? hasta, int? idVehiculo)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, 1, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var consulta = _context.GastosOperaciones
                .Include(g => g.IdTipoGastoNavigation)
                .Include(g => g.IdVehiculoNavigation)
                .Where(g => g.FechaGasto.HasValue
                            && g.FechaGasto.Value.Date >= fechaInicio.Date
                            && g.FechaGasto.Value.Date <= fechaFin.Date);

            if (idVehiculo.HasValue)
            {
                consulta = consulta.Where(g => g.IdVehiculo == idVehiculo);
            }

            var gastos = await consulta.ToListAsync();

            var resumen = gastos
                .GroupBy(g => g.IdVehiculo)
                .Select(g => new ResumenGastoVehiculo
                {
                    IdVehiculo = g.Key,
                    Patente = g.FirstOrDefault()?.IdVehiculoNavigation?.Patente,
                    Total = g.Sum(x => x.Importe ?? 0),
                    TotalCombustible = g.Where(x => x.IdTipoGasto == 1).Sum(x => x.Importe ?? 0),
                    TotalLavados = g.Where(x => x.IdTipoGasto == 2).Sum(x => x.Importe ?? 0),
                    TotalNeumaticos = g.Where(x => x.IdTipoGasto == 3).Sum(x => x.Importe ?? 0),
                    TotalReparaciones = g.Where(x => x.IdTipoGasto == 4 || x.IdTipoGasto == 5).Sum(x => x.Importe ?? 0),
                    TotalServices = g.Where(x => x.IdTipoGasto == 6).Sum(x => x.Importe ?? 0),
                    TotalImprevistos = g.Where(x => x.IdTipoGasto == 7).Sum(x => x.Importe ?? 0)
                })
                .OrderBy(r => r.Patente)
                .ToList();

            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", idVehiculo);

            return View(resumen);
        }

        // GET: GastosOperaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GastosOperaciones == null)
            {
                return NotFound();
            }

            var gastoOperacion = await _context.GastosOperaciones
                .Include(g => g.CuilNavigation)
                .Include(g => g.IdTipoGastoNavigation)
                .Include(g => g.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdGasto == id);
            if (gastoOperacion == null)
            {
                return NotFound();
            }

            return View(gastoOperacion);
        }

        // GET: GastosOperaciones/Create
        public IActionResult Create()
        {
            ViewData["Cuil"] = new SelectList(_context.Conductores.OrderBy(c => c.Apellido), "Cuil", "Cuil");
            ViewData["IdTipoGasto"] = new SelectList(_context.TiposDeGastos, "IdTipoGasto", "Nombre");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente");
            return View();
        }

        // POST: GastosOperaciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGasto,IdVehiculo,Cuil,FechaGasto,IdTipoGasto,Descripcion,Importe,Kilometraje,NroFactura")] GastoOperacion gastoOperacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gastoOperacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores.OrderBy(c => c.Apellido), "Cuil", "Cuil", gastoOperacion.Cuil);
            ViewData["IdTipoGasto"] = new SelectList(_context.TiposDeGastos, "IdTipoGasto", "Nombre", gastoOperacion.IdTipoGasto);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", gastoOperacion.IdVehiculo);
            return View(gastoOperacion);
        }

        // GET: GastosOperaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GastosOperaciones == null)
            {
                return NotFound();
            }

            var gastoOperacion = await _context.GastosOperaciones.FindAsync(id);
            if (gastoOperacion == null)
            {
                return NotFound();
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores.OrderBy(c => c.Apellido), "Cuil", "Cuil", gastoOperacion.Cuil);
            ViewData["IdTipoGasto"] = new SelectList(_context.TiposDeGastos, "IdTipoGasto", "Nombre", gastoOperacion.IdTipoGasto);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", gastoOperacion.IdVehiculo);
            return View(gastoOperacion);
        }

        // POST: GastosOperaciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGasto,IdVehiculo,Cuil,FechaGasto,IdTipoGasto,Descripcion,Importe,Kilometraje,NroFactura")] GastoOperacion gastoOperacion)
        {
            if (id != gastoOperacion.IdGasto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gastoOperacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoOperacionExists(gastoOperacion.IdGasto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores.OrderBy(c => c.Apellido), "Cuil", "Cuil", gastoOperacion.Cuil);
            ViewData["IdTipoGasto"] = new SelectList(_context.TiposDeGastos, "IdTipoGasto", "Nombre", gastoOperacion.IdTipoGasto);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", gastoOperacion.IdVehiculo);
            return View(gastoOperacion);
        }

        // GET: GastosOperaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GastosOperaciones == null)
            {
                return NotFound();
            }

            var gastoOperacion = await _context.GastosOperaciones
                .Include(g => g.CuilNavigation)
                .Include(g => g.IdTipoGastoNavigation)
                .Include(g => g.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdGasto == id);
            if (gastoOperacion == null)
            {
                return NotFound();
            }

            return View(gastoOperacion);
        }

        // POST: GastosOperaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GastosOperaciones == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.GastosOperaciones'  is null.");
            }
            var gastoOperacion = await _context.GastosOperaciones.FindAsync(id);
            try
            {
                if (gastoOperacion != null)
                {
                    _context.GastosOperaciones.Remove(gastoOperacion);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["Mensaje"] = "No es posible eliminar el registro porque posee movimientos asociados.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GastoOperacionExists(int id)
        {
            return _context.GastosOperaciones.Any(e => e.IdGasto == id);
        }
    }
}