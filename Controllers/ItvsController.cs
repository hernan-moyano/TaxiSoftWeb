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
    public class ItvsController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public ItvsController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Itvs
        public async Task<IActionResult> Index()
        {
            var taxisoftDbContext = _context.Itvs.Include(i => i.IdVehiculoNavigation);
            return View(await taxisoftDbContext.ToListAsync());
        }

        // GET: Itvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Itvs == null)
            {
                return NotFound();
            }

            var itv = await _context.Itvs
                .Include(i => i.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdItv == id);
            if (itv == null)
            {
                return NotFound();
            }

            return View(itv);
        }

        // GET: Itvs/Create
        public IActionResult Create()
        {
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo");
            return View();
        }

        // POST: Itvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdItv,VigenciaDesde,VigenciaHasta,Descripcion,Valor,IdVehiculo")] Itv itv)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", itv.IdVehiculo);
            return View(itv);
        }

        // GET: Itvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Itvs == null)
            {
                return NotFound();
            }

            var itv = await _context.Itvs.FindAsync(id);
            if (itv == null)
            {
                return NotFound();
            }
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", itv.IdVehiculo);
            return View(itv);
        }

        // POST: Itvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdItv,VigenciaDesde,VigenciaHasta,Descripcion,Valor,IdVehiculo")] Itv itv)
        {
            if (id != itv.IdItv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItvExists(itv.IdItv))
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
            ViewData["IdVehiculo"] = new SelectList(_context.Vehiculos, "IdVehiculo", "IdVehiculo", itv.IdVehiculo);
            return View(itv);
        }

        // GET: Itvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Itvs == null)
            {
                return NotFound();
            }

            var itv = await _context.Itvs
                .Include(i => i.IdVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.IdItv == id);
            if (itv == null)
            {
                return NotFound();
            }

            return View(itv);
        }

        // POST: Itvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Itvs == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Itvs'  is null.");
            }
            var itv = await _context.Itvs.FindAsync(id);
            if (itv != null)
            {
                _context.Itvs.Remove(itv);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItvExists(int id)
        {
          return _context.Itvs.Any(e => e.IdItv == id);
        }
    }
}
