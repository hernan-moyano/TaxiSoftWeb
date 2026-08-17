using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;

namespace TaxiSoftWeb.Controllers
{
    public class CalibracionesController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public CalibracionesController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Calibraciones
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.CalibracionesTaximetro.Include(c => c.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Calibraciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CalibracionesTaximetro == null)
            {
                return NotFound();
            }

            var calibracionTaximetro = await _context.CalibracionesTaximetro
                .Include(c => c.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdCalibracion == id);
            if (calibracionTaximetro == null)
            {
                return NotFound();
            }

            return View(calibracionTaximetro);
        }

        // GET: Calibraciones/Create
        public IActionResult Create()
        {
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente");
            return View();
        }

        // POST: Calibraciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCalibracion,IdVehiculo,FechaCalibracion,ProximaCalibracion,EntidadCalibradora,NroCertificado,Valor")] CalibracionTaximetro calibracionTaximetro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calibracionTaximetro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", calibracionTaximetro.IdVehiculo);
            return View(calibracionTaximetro);
        }

        // GET: Calibraciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CalibracionesTaximetro == null)
            {
                return NotFound();
            }

            var calibracionTaximetro = await _context.CalibracionesTaximetro.FindAsync(id);
            if (calibracionTaximetro == null)
            {
                return NotFound();
            }
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", calibracionTaximetro.IdVehiculo);
            return View(calibracionTaximetro);
        }

        // POST: Calibraciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCalibracion,IdVehiculo,FechaCalibracion,ProximaCalibracion,EntidadCalibradora,NroCertificado,Valor")] CalibracionTaximetro calibracionTaximetro)
        {
            if (id != calibracionTaximetro.IdCalibracion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calibracionTaximetro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalibracionTaximetroExists(calibracionTaximetro.IdCalibracion))
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
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "Patente", calibracionTaximetro.IdVehiculo);
            return View(calibracionTaximetro);
        }

        // GET: Calibraciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CalibracionesTaximetro == null)
            {
                return NotFound();
            }

            var calibracionTaximetro = await _context.CalibracionesTaximetro
                .Include(c => c.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdCalibracion == id);
            if (calibracionTaximetro == null)
            {
                return NotFound();
            }

            return View(calibracionTaximetro);
        }

        // POST: Calibraciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CalibracionesTaximetro == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.CalibracionesTaximetro'  is null.");
            }
            var calibracionTaximetro = await _context.CalibracionesTaximetro.FindAsync(id);
            if (calibracionTaximetro != null)
            {
                _context.CalibracionesTaximetro.Remove(calibracionTaximetro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalibracionTaximetroExists(int id)
        {
            return _context.CalibracionesTaximetro.Any(e => e.IdCalibracion == id);
        }
    }
}