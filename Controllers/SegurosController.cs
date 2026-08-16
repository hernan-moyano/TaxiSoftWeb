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
    public class SegurosController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public SegurosController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Seguros
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Seguros.Include(s => s.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Seguros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Seguros == null)
            {
                return NotFound();
            }

            var seguro = await _context.Seguros
                .Include(s => s.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdSeguro == id);
            if (seguro == null)
            {
                return NotFound();
            }

            return View(seguro);
        }

        // GET: Seguros/Create
        public IActionResult Create()
        {
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: Seguros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSeguro,NroPoliza,Aseguradora,VigenciaDesde,VigenciaHasta,IdVehiculo")] Seguro seguro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seguro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", seguro.IdVehiculo);
            return View(seguro);
        }

        // GET: Seguros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Seguros == null)
            {
                return NotFound();
            }

            var seguro = await _context.Seguros.FindAsync(id);
            if (seguro == null)
            {
                return NotFound();
            }
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", seguro.IdVehiculo);
            return View(seguro);
        }

        // POST: Seguros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSeguro,NroPoliza,Aseguradora,VigenciaDesde,VigenciaHasta,IdVehiculo")] Seguro seguro)
        {
            if (id != seguro.IdSeguro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seguro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeguroExists(seguro.IdSeguro))
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
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", seguro.IdVehiculo);
            return View(seguro);
        }

        // GET: Seguros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Seguros == null)
            {
                return NotFound();
            }

            var seguro = await _context.Seguros
                .Include(s => s.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdSeguro == id);
            if (seguro == null)
            {
                return NotFound();
            }

            return View(seguro);
        }

        // POST: Seguros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Seguros == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Seguros'  is null.");
            }
            var seguro = await _context.Seguros.FindAsync(id);
            if (seguro != null)
            {
                _context.Seguros.Remove(seguro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeguroExists(int id)
        {
          return _context.Seguros.Any(e => e.IdSeguro == id);
        }
    }
}
