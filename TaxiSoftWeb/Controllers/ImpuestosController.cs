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
    public class ImpuestosController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public ImpuestosController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Impuestos
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Impuestos.Include(i => i.IdEstadoPNavigation).Include(i => i.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Impuestos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Impuestos == null)
            {
                return NotFound();
            }

            var impuesto = await _context.Impuestos
                .Include(i => i.IdEstadoPNavigation)
                .Include(i => i.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdImpuesto == id);
            if (impuesto == null)
            {
                return NotFound();
            }

            return View(impuesto);
        }

        // GET: Impuestos/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: Impuestos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdImpuesto,FechaVto,Valor,Descripcion,IdEstadoP,IdVehiculo")] Impuesto impuesto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(impuesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP", impuesto.IdEstadoP);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", impuesto.IdVehiculo);
            return View(impuesto);
        }

        // GET: Impuestos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Impuestos == null)
            {
                return NotFound();
            }

            var impuesto = await _context.Impuestos.FindAsync(id);
            if (impuesto == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP", impuesto.IdEstadoP);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", impuesto.IdVehiculo);
            return View(impuesto);
        }

        // POST: Impuestos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdImpuesto,FechaVto,Valor,Descripcion,IdEstadoP,IdVehiculo")] Impuesto impuesto)
        {
            if (id != impuesto.IdImpuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(impuesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImpuestoExists(impuesto.IdImpuesto))
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
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP", impuesto.IdEstadoP);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", impuesto.IdVehiculo);
            return View(impuesto);
        }

        // GET: Impuestos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Impuestos == null)
            {
                return NotFound();
            }

            var impuesto = await _context.Impuestos
                .Include(i => i.IdEstadoPNavigation)
                .Include(i => i.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdImpuesto == id);
            if (impuesto == null)
            {
                return NotFound();
            }

            return View(impuesto);
        }

        // POST: Impuestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Impuestos == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Impuestos'  is null.");
            }
            var impuesto = await _context.Impuestos.FindAsync(id);
            if (impuesto != null)
            {
                _context.Impuestos.Remove(impuesto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImpuestoExists(int id)
        {
          return _context.Impuestos.Any(e => e.IdImpuesto == id);
        }
    }
}
