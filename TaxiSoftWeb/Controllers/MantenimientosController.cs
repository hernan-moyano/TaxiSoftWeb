using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiSoftWeb.Models;

namespace TaxiSoftWeb.Controllers
{
    public class MantenimientosController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public MantenimientosController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Mantenimientos
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Mantenimientos.Include(m => m.IdEstadoANavigation).Include(m => m.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Mantenimientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Mantenimientos == null)
            {
                return NotFound();
            }

            var mantenimiento = await _context.Mantenimientos
                .Include(m => m.IdEstadoANavigation)
                .Include(m => m.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdMant == id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return View(mantenimiento);
        }

        // GET: Mantenimientos/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "IdEstadoA");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: Mantenimientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMant,FechaMant,Descripcion,Valor,IdVehiculo,IdEstadoA")] Mantenimiento mantenimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mantenimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "IdEstadoA", mantenimiento.IdEstadoA);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", mantenimiento.IdVehiculo);
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mantenimientos == null)
            {
                return NotFound();
            }

            var mantenimiento = await _context.Mantenimientos.FindAsync(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "IdEstadoA", mantenimiento.IdEstadoA);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", mantenimiento.IdVehiculo);
            return View(mantenimiento);
        }

        // POST: Mantenimientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMant,FechaMant,Descripcion,Valor,IdVehiculo,IdEstadoA")] Mantenimiento mantenimiento)
        {
            if (id != mantenimiento.IdMant)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mantenimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MantenimientoExists(mantenimiento.IdMant))
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
            ViewData["IdEstadoA"] = new SelectList(_context.EstadosActividades, "IdEstadoA", "IdEstadoA", mantenimiento.IdEstadoA);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", mantenimiento.IdVehiculo);
            return View(mantenimiento);
        }

        // GET: Mantenimientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mantenimientos == null)
            {
                return NotFound();
            }

            var mantenimiento = await _context.Mantenimientos
                .Include(m => m.IdEstadoANavigation)
                .Include(m => m.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdMant == id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            return View(mantenimiento);
        }

        // POST: Mantenimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mantenimientos == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Mantenimientos'  is null.");
            }
            var mantenimiento = await _context.Mantenimientos.FindAsync(id);
            if (mantenimiento != null)
            {
                _context.Mantenimientos.Remove(mantenimiento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MantenimientoExists(int id)
        {
          return _context.Mantenimientos.Any(e => e.IdMant == id);
        }
    }
}
