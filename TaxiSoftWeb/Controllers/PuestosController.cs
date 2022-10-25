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
    public class PuestosController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public PuestosController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Puestos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Puestos.ToListAsync());
        }

        // GET: Puestos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Puestos == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        // GET: Puestos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Puestos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPuesto,CalifProfesional,TarDesepeniada")] Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        // GET: Puestos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Puestos == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }
            return View(puesto);
        }

        // POST: Puestos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPuesto,CalifProfesional,TarDesepeniada")] Puesto puesto)
        {
            if (id != puesto.IdPuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestoExists(puesto.IdPuesto))
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
            return View(puesto);
        }

        // GET: Puestos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Puestos == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        // POST: Puestos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Puestos == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Puestos'  is null.");
            }
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto != null)
            {
                _context.Puestos.Remove(puesto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestoExists(int id)
        {
          return _context.Puestos.Any(e => e.IdPuesto == id);
        }
    }
}
