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
    public class MultasController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public MultasController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Multas
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Multas.Include(m => m.IdEstadoPNavigation).Include(m => m.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Multas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Multas == null)
            {
                return NotFound();
            }

            var multa = await _context.Multas
                .Include(m => m.IdEstadoPNavigation)
                .Include(m => m.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdMulta == id);
            if (multa == null)
            {
                return NotFound();
            }

            return View(multa);
        }

        // GET: Multas/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP");
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: Multas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMulta,FechaVto,Descripcion,Valor,IdVehiculo,IdEstadoP")] Multa multa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(multa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP", multa.IdEstadoP);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", multa.IdVehiculo);
            return View(multa);
        }

        // GET: Multas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Multas == null)
            {
                return NotFound();
            }

            var multa = await _context.Multas.FindAsync(id);
            if (multa == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP", multa.IdEstadoP);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", multa.IdVehiculo);
            return View(multa);
        }

        // POST: Multas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMulta,FechaVto,Descripcion,Valor,IdVehiculo,IdEstadoP")] Multa multa)
        {
            if (id != multa.IdMulta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultaExists(multa.IdMulta))
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
            ViewData["IdEstadoP"] = new SelectList(_context.EstadosPagos, "IdEstadoP", "IdEstadoP", multa.IdEstadoP);
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", multa.IdVehiculo);
            return View(multa);
        }

        // GET: Multas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Multas == null)
            {
                return NotFound();
            }

            var multa = await _context.Multas
                .Include(m => m.IdEstadoPNavigation)
                .Include(m => m.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdMulta == id);
            if (multa == null)
            {
                return NotFound();
            }

            return View(multa);
        }

        // POST: Multas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Multas == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Multas'  is null.");
            }
            var multa = await _context.Multas.FindAsync(id);
            if (multa != null)
            {
                _context.Multas.Remove(multa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MultaExists(int id)
        {
          return _context.Multas.Any(e => e.IdMulta == id);
        }
    }
}
