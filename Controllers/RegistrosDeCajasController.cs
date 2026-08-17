using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;
using TaxiSoftWeb.ViewModels;

namespace TaxiSoftWeb.Controllers
{
    public class RegistrosDeCajasController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public RegistrosDeCajasController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: RegistrosDeCajas
        public async Task<IActionResult> Index(DateTime? desde, DateTime? hasta)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var taxisoftDbContext = _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation).Include(r => r.IdCajaNavigation).Include(r => r.IdOperacionNavigation).Include(r => r.IdTurnoNavigation).Include(r => r.IdVehiculoNavigation)
                .Where(r => r.FechaRegisCaja >= fechaInicio && r.FechaRegisCaja <= fechaFin);

            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");

            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: RegistrosDeCajas/Liquidacion
        public async Task<IActionResult> Liquidacion(DateTime? desde, DateTime? hasta, string cuil)
        {
            var fechaInicio = desde ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var fechaFin = hasta ?? DateTime.Today;
            if (fechaFin < fechaInicio)
            {
                fechaFin = fechaInicio;
            }

            var consulta = _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation)
                .Include(r => r.IdCajaNavigation)
                .Include(r => r.IdVehiculoNavigation)
                .Include(r => r.IdTurnoNavigation)
                .Where(r => r.FechaRegisCaja.HasValue
                            && r.FechaRegisCaja.Value.Date >= fechaInicio.Date
                            && r.FechaRegisCaja.Value.Date <= fechaFin.Date);

            if (!string.IsNullOrEmpty(cuil))
            {
                consulta = consulta.Where(r => r.Cuil == cuil);
            }

            var registros = await consulta.ToListAsync();

            var liquidacion = registros
                .GroupBy(r => new
                {
                    r.Cuil,
                    r.IdVehiculo
                })
                .Select(g =>
                {
                    var chofer = g.FirstOrDefault()?.CuilNavigation;
                    var totalIngresos = g.Where(r => r.IdCaja == 1).Sum(r => r.Importe);
                    var totalEgresos = g.Where(r => r.IdCaja == 2).Sum(r => r.Importe);
                    var recaudacionNeta = totalIngresos - totalEgresos;
                    var tipoAlquiler = chofer?.TipoAlquiler;
                    decimal? alquiler = null;
                    decimal? comision = null;
                    if (tipoAlquiler == "fijo")
                    {
                        alquiler = chofer?.MontoAlquiler;
                    }
                    else if (tipoAlquiler == "porcentaje" && chofer?.PorcentajeRecaudacion.HasValue == true)
                    {
                        comision = recaudacionNeta * chofer.PorcentajeRecaudacion / 100m;
                    }

                    return new LiquidacionChofer
                    {
                        Cuil = g.Key.Cuil,
                        NombreChofer = chofer != null ? $"{chofer.Apellido} {chofer.Nombre}".Trim() : g.Key.Cuil,
                        Patente = g.FirstOrDefault()?.IdVehiculoNavigation?.Patente,
                        TotalIngresos = totalIngresos,
                        TotalEgresos = totalEgresos,
                        RecaudacionNeta = recaudacionNeta,
                        TipoAlquiler = tipoAlquiler,
                        Alquiler = alquiler,
                        Comision = comision
                    };
                })
                .OrderBy(l => l.NombreChofer)
                .ToList();

            ViewData["Cuil"] = new SelectList(
                _context.Conductores.OrderBy(c => c.Apellido),
                "Cuil",
                "Cuil");
            ViewData["desde"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["hasta"] = fechaFin.ToString("yyyy-MM-dd");
            ViewData["cuilSeleccionado"] = cuil;

            return View(liquidacion);
        }

        // GET: RegistrosDeCajas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RegistrosDeCajas == null)
            {
                return NotFound();
            }

            var registrosDeCaja = await _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation)
                .Include(r => r.IdCajaNavigation)
                .Include(r => r.IdOperacionNavigation)
                .Include(r => r.IdTurnoNavigation)
                .Include(r => r.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdRegistroCaja == id);
            if (registrosDeCaja == null)
            {
                return NotFound();
            }

            return View(registrosDeCaja);
        }

        // GET: RegistrosDeCajas/Create
        public IActionResult Create()
        {
            ViewData["Cuil"] = new SelectList(_context.Conductores.OrderBy(c => c.Apellido), "Cuil", "Cuil");
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja");
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion");
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente");
            return View();
        }

        // POST: RegistrosDeCajas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistroCaja,FechaRegisCaja,Concepto,Importe,IdTurno,Cuil,IdVehiculo,IdCaja,IdOperacion")] RegistrosDeCaja registrosDeCaja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registrosDeCaja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil", registrosDeCaja.Cuil);
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja", registrosDeCaja.IdCaja);
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion", registrosDeCaja.IdOperacion);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", registrosDeCaja.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", registrosDeCaja.IdVehiculo);
            return View(registrosDeCaja);
        }

        // GET: RegistrosDeCajas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RegistrosDeCajas == null)
            {
                return NotFound();
            }

            var registrosDeCaja = await _context.RegistrosDeCajas.FindAsync(id);
            if (registrosDeCaja == null)
            {
                return NotFound();
            }
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil", registrosDeCaja.Cuil);
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja", registrosDeCaja.IdCaja);
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion", registrosDeCaja.IdOperacion);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", registrosDeCaja.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", registrosDeCaja.IdVehiculo);
            return View(registrosDeCaja);
        }

        // POST: RegistrosDeCajas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRegistroCaja,FechaRegisCaja,Concepto,Importe,IdTurno,Cuil,IdVehiculo,IdCaja,IdOperacion")] RegistrosDeCaja registrosDeCaja)
        {
            if (id != registrosDeCaja.IdRegistroCaja)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrosDeCaja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrosDeCajaExists(registrosDeCaja.IdRegistroCaja))
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
            ViewData["Cuil"] = new SelectList(_context.Conductores, "Cuil", "Cuil", registrosDeCaja.Cuil);
            ViewData["IdCaja"] = new SelectList(_context.TiposDeCajas, "IdCaja", "NomCaja", registrosDeCaja.IdCaja);
            ViewData["IdOperacion"] = new SelectList(_context.TiposDeOperaciones, "IdOperacion", "NomOperacion", registrosDeCaja.IdOperacion);
            ViewData["IdTurno"] = new SelectList(_context.Turnos, "IdTurno", "NomTurno", registrosDeCaja.IdTurno);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", registrosDeCaja.IdVehiculo);
            return View(registrosDeCaja);
        }

        // GET: RegistrosDeCajas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RegistrosDeCajas == null)
            {
                return NotFound();
            }

            var registrosDeCaja = await _context.RegistrosDeCajas
                .Include(r => r.CuilNavigation)
                .Include(r => r.IdCajaNavigation)
                .Include(r => r.IdOperacionNavigation)
                .Include(r => r.IdTurnoNavigation)
                .Include(r => r.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdRegistroCaja == id);
            if (registrosDeCaja == null)
            {
                return NotFound();
            }

            return View(registrosDeCaja);
        }

        // POST: RegistrosDeCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RegistrosDeCajas == null)
            {
                return Problem("El conjunto de entidades 'TaxisoftDbContext.RegistrosDeCajas' es nulo.");
            }
            var registrosDeCaja = await _context.RegistrosDeCajas.FindAsync(id);
            try
            {
                if (registrosDeCaja != null)
                {
                    _context.RegistrosDeCajas.Remove(registrosDeCaja);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                TempData["Mensaje"] = "No es posible eliminar el registro porque posee movimientos asociados.";
            }
            return RedirectToAction(nameof(Index));           
           
        }

        private bool RegistrosDeCajaExists(int id)
        {
          return _context.RegistrosDeCajas.Any(e => e.IdRegistroCaja == id);
        }
    }
}
