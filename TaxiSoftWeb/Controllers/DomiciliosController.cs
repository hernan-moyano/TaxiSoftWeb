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
    public class DomiciliosController : Controller
    {
        private readonly TaxisoftDbContext _context;

        public DomiciliosController(TaxisoftDbContext context)
        {
            _context = context;
        }

        // GET: Domicilios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Domicilios.ToListAsync());
        }

        // GET: Domicilios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Domicilios == null)
            {
                return NotFound();
            }

            var domicilio = await _context.Domicilios
                .FirstOrDefaultAsync(m => m.IdDomicilio == id);
            if (domicilio == null)
            {
                return NotFound();
            }

            return View(domicilio);
        }

        // GET: Domicilios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Domicilios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDomicilio,Calle,Numero,Piso,Departamento,Ciudad")] Domicilio domicilio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(domicilio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(domicilio);
        }

        // GET: Domicilios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Domicilios == null)
            {
                return NotFound();
            }

            var domicilio = await _context.Domicilios.FindAsync(id);
            if (domicilio == null)
            {
                return NotFound();
            }
            return View(domicilio);
        }

        // POST: Domicilios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDomicilio,Calle,Numero,Piso,Departamento,Ciudad")] Domicilio domicilio)
        {
            if (id != domicilio.IdDomicilio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domicilio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DomicilioExists(domicilio.IdDomicilio))
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
            return View(domicilio);
        }

        // GET: Domicilios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Domicilios == null)
            {
                return NotFound();
            }

            var domicilio = await _context.Domicilios
                .FirstOrDefaultAsync(m => m.IdDomicilio == id);
            if (domicilio == null)
            {
                return NotFound();
            }

            return View(domicilio);
        }

        // POST: Domicilios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Domicilios == null)
            {
                return Problem("Entity set 'TaxisoftDbContext.Domicilios'  is null.");
            }
            var domicilio = await _context.Domicilios.FindAsync(id);
            if (domicilio != null)
            {
                _context.Domicilios.Remove(domicilio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DomicilioExists(int id)
        {
          return _context.Domicilios.Any(e => e.IdDomicilio == id);
        }
    }
}
